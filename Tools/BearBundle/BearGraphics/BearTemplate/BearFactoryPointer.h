#pragma once
template<typename T>
class BearFactoryPointer
{
	struct SContainer
	{
		SContainer():Object(0), Count(1){}
		SContainer(void *object, BearGraphicsObjectType objectType):Object(object), ObjectType(objectType), Count(1){}
		~SContainer() { Impl::BearRemoverObject::Remove<T>(Object, ObjectType); Object = 0; }
		void *Object;
		BearGraphicsObjectType          ObjectType;
		bsize Count;
	} 
	*m_container;
	void *m_CurObject;
	BearGraphicsObjectType          m_CurrentType;
protected:
	inline T*get_object() { if (m_CurObject) return reinterpret_cast<T*>(m_CurObject); if (m_container) return reinterpret_cast<T*>(m_container->Object); return 0; }
	const inline T*get_object()  const  { if (m_CurObject) return reinterpret_cast<T*>(m_CurObject); if (m_container) return reinterpret_cast<T*>(m_container->Object); return 0; }
public:
	BearFactoryPointer(void*data, void*CurObj) :m_container(reinterpret_cast<SContainer*>(data)), m_CurObject(CurObj), m_CurrentType(Impl::BearTypeManager::GetType<T>()) {  }
public:
	BearFactoryPointer() :m_container(0), m_CurObject(0){}
	BearFactoryPointer(T*c) :m_container(0), m_CurObject(0) { m_container = bear_new< SContainer>(reinterpret_cast<T*>(c), Impl::BearTypeManager::GetType<T>()); m_CurrentType = Impl::BearTypeManager::GetType<T>(); }
	inline void clear() 
	{ 
		if (m_container)
		{
			m_container->Count--;
			if (m_container->Count == 0)
			{
				bear_delete (m_container);
			}
			m_container = 0; m_CurObject= 0;
		}
	}
	inline ~BearFactoryPointer() { clear(); }
	inline	bool empty()const { if (m_container)return m_container->Object==0; return m_container == 0; }

	inline T*get() { return get_object(); }
	inline const T*get()const { return get_object(); }
	inline T& operator*() { BEAR_CHECK(empty() == false); return *get_object(); }
	inline const T& operator*() const { BEAR_CHECK(empty() == false);   return *get_object(); }
	inline T* operator->() { BEAR_CHECK(empty() == false);  return get_object(); }
	inline const T* operator->() const { BEAR_CHECK(empty() == false);  return get_object(); }

	template<typename C>
	inline BearFactoryPointer<C> cast() const
	{
		if (empty())
		{
			return  BearFactoryPointer<C>();
		}
		void* out = Impl::BearCasterObject::Cast(m_container->Object, m_container->ObjectType, Impl::BearTypeManager::GetType<C>());
		if (out)
		{
			m_container->Count++;

			BearFactoryPointer<C> result(reinterpret_cast<void*>(const_cast<SContainer*>(m_container)), out);
			return result;
		}
		else
		{
			BearFactoryPointer<C> result;
			return result;
		}
		
	}
		
	inline void replace(BearFactoryPointer&right)
	{
		if(right.empty()&& empty())return ;

		if (right.empty())
		{
			right.m_container = m_container =bear_new< SContainer>();
		}
		if (empty())
		{
				m_container = bear_new< SContainer>();
		}
		bear_swap(right.m_container->Object, m_container->Object);
		bear_swap(right.m_container->ObjectType, m_container->ObjectType);
	}
	inline void copy(const BearFactoryPointer&right)
	{
		clear();
		m_CurrentType = right.m_CurrentType;
		if (right.empty())return;
		m_container = right.m_container;
		m_container->Count++;
		m_CurObject = right.m_CurObject;
	}
	inline void swap(BearFactoryPointer&right)
	{
		bear_swap(m_CurrentType, right.m_CurrentType);
		bear_swap(m_container, right.m_container);
		bear_swap(m_CurObject, right.m_CurObject);
	}
	template<class C>
	BearFactoryPointer(const BearFactoryPointer<C>& right) :m_container(0) { auto temp = right.template  cast<T>(); swap(temp); ; }
	BearFactoryPointer(const BearFactoryPointer&right) :m_container(0) { copy(right); }
	BearFactoryPointer(BearFactoryPointer&&right) :m_container(0) { swap(right); }
	BearFactoryPointer&operator=(const BearFactoryPointer&right) { copy(right); return*this; }
	BearFactoryPointer&operator=(BearFactoryPointer&&right) { swap(right); return*this; }
	inline bool operator==(const BearFactoryPointer&right)const { return right.m_container == m_container; };
	inline bool operator!=(const BearFactoryPointer&right)const { return right.m_container != m_container; };
	inline bool operator<(const BearFactoryPointer&right)const { return right.m_container < m_container; };
	template<class C>
	inline C*StaticCast() { return static_cast<C*>(get_object()); }
};