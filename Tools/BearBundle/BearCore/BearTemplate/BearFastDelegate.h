#pragma once
namespace Impl
{
	template<typename Return, typename...Args>
	class BearFastDelegateCaller
	{
	public:
		virtual ~BearFastDelegateCaller() {}
		virtual Return call(Args...args)const = 0;
		virtual BearFastDelegateCaller* copy()const = 0;
		virtual bool equal(const BearFastDelegateCaller* right) const= 0;
	};

	template<typename Return, typename...Args>
	class BearFastDelegateCallerFunction:public BearFastDelegateCaller<Return,Args...>
	{
	public:
		template<typename F>
		BearFastDelegateCallerFunction(F f):m_function(f){};
		virtual ~BearFastDelegateCallerFunction() {}
		virtual Return call(Args...args) const { return m_function.call(args...); }
		virtual BearFastDelegateCaller<Return, Args...>* copy()const
		{
			return bear_new< BearFastDelegateCallerFunction>(m_function);
		}
		virtual bool equal(const BearFastDelegateCaller<Return, Args...>* right)const
		{
			return m_function == static_cast<const BearFastDelegateCallerFunction*>(right)->m_function;
		}
	private:
		BearFunction< Return(Args...)> m_function;
	};

	template<typename Return,typename Class, typename...Args>
	class BearFastDelegateCallerFunctionFromClass :public BearFastDelegateCaller<Return, Args...>
	{
	public:
		template<typename F>
		BearFastDelegateCallerFunctionFromClass(Class*ptr,F f) :m_function(f),m_ptr(ptr) {};
		virtual ~BearFastDelegateCallerFunctionFromClass() {}
		virtual Return call(Args...args) const {return m_function.call(m_ptr,args...); }
		virtual BearFastDelegateCaller<Return, Args...>* copy()const
		{
			return bear_new< BearFastDelegateCallerFunctionFromClass>(m_ptr,m_function);
		}
		virtual bool equal(const BearFastDelegateCaller<Return, Args...>* right)const
		{
			return m_function == static_cast<const BearFastDelegateCallerFunctionFromClass*>(right)->m_function&& m_ptr== static_cast<const BearFastDelegateCallerFunctionFromClass*>(right)->m_ptr;
		}
	private:
		BearFunction< Return(Class::*)(Args...)> m_function;
		Class* m_ptr;
	};

}
template<typename Return,typename...Args>
class BearFastDelegate
{
public:
	BearFastDelegate(std::nullptr_t) { }
	BearFastDelegate(){ }
	inline ~BearFastDelegate() {}
	inline void copy(const BearFastDelegate& right)
	{
		m_caller.clear();
		if (*right.m_caller)m_caller = right.m_caller->copy();
	}
	inline void clear()
	{
		m_caller.clear();
	}
	inline void swap( BearFastDelegate& right)
	{
		right.m_caller.swap(m_caller);
	} 
	BearFastDelegate(const BearFastDelegate& right)
	{
		copy(right);
	}
	BearFastDelegate(BearFastDelegate&& right)
	{
		swap(right);
	}
	BearFastDelegate& operator=(const BearFastDelegate& right)
	{
		copy(right);
		return*this;
	}
	BearFastDelegate& operator=(BearFastDelegate&& right)
	{
		swap(right);
		return*this;
	}
	bool  operator==(const BearFastDelegate& right)const
	{
		if (*m_caller == 0 || *right.m_caller == 0)return *m_caller == *right.m_caller;
		
		return m_caller->equal(*right.m_caller);
	}
	inline Return call(Args...args) const
	{
		return m_caller->call(args...);
	}
	template<typename F>
	BearFastDelegate(F f) { m_caller = bear_new<Impl::BearFastDelegateCallerFunction<Return, Args...>>(f); }
	template<typename F>
	 inline  void bind(F f)
	{
		 m_caller.clear();
		 m_caller = bear_new<Impl::BearFastDelegateCallerFunction<Return,Args...>>(f);
	}
	template<typename Class, typename F>
	BearFastDelegate(const Class& ptr, F f)
	{
		if constexpr (bear_is_pointer<Class>::value)
		{
			m_caller = bear_new < Impl::BearFastDelegateCallerFunctionFromClass < Return,typename  bear_remove_pointer<Class>::type, Args... >>(const_cast<Class>(ptr), f);
		}
		else
		{
			m_caller = bear_new < Impl::BearFastDelegateCallerFunctionFromClass < Return, Class, Args... >>(&const_cast<Class>(ptr), f);
		}
	}
	template<typename Class, typename F>
	 inline  void bind(const Class&ptr,F f)
	{
		 m_caller.clear();
		if constexpr (bear_is_pointer<Class>::value)
		{
			m_caller = bear_new < Impl::BearFastDelegateCallerFunctionFromClass < Return,typename bear_remove_pointer<Class>::type, Args... >>(const_cast<Class>(ptr), f);
		}
		else
		{
			m_caller = bear_new < Impl::BearFastDelegateCallerFunctionFromClass < Return, Class, Args... >>(&const_cast<Class>(ptr), f);
		}
		
	}

	
	inline bool empty()const { return *m_caller == 0;}
	explicit operator bool() const { return !empty(); }
private:
	BearRef< Impl::BearFastDelegateCaller< Return,Args ...>> m_caller;
};