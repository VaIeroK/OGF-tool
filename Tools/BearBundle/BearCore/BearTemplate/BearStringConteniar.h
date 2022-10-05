#pragma once
namespace Impl
{
	struct BearStringConteniarData
	{
		uint32 CRC;
		bsize Depth;
		bsize Size;
		BearStringConteniarData* Next;
		bchar8 String[];
	};
	class BEARTOOL_API BearStringConteniarManager
	{
		BEAR_CLASS_STATIC(BearStringConteniarManager);
	public:
		static void Initialize();
		static const BearStringConteniarData*Get(const bchar8*str);
		static const BearStringConteniarData*Get(const bchar16*str);
		static void Destroy();
	private:
		static bool Equal(BearStringConteniarData*, const bchar8* str, bsize len);
		static bool Equal(BearStringConteniarData*, const bchar16* str, bsize len);
	};
}
template<typename C>
class BearStringConteniarUnknown
{
public:
	BearStringConteniarUnknown() :m_data(0)
	{

	}
	BearStringConteniarUnknown(const C*text):m_data(0)
	{
		set(text);
	}
	BearStringConteniarUnknown( BearStringConteniarUnknown<C>&&right) :m_data(0)
	{
		swap(right);
	}
	inline void swap( BearStringConteniarUnknown<C>&right)
	{
		bear_swap(m_data, right.m_data);
	}
	inline void copy(const BearStringConteniarUnknown<C>&right)
	{
		m_data = right.m_data;
	}
	BearStringConteniarUnknown(const  BearStringConteniarUnknown<C>&right) :m_data(0)
	{
		copy(right);
	}
	inline BearStringConteniarUnknown& operator=(const  BearStringConteniarUnknown<C>&right)
	{
		copy(right);
		return *this;
	}
	inline BearStringConteniarUnknown& operator=( BearStringConteniarUnknown<C>&&right)
	{
		swap(right);
		return *this;
	}
	inline bsize size()const
	{
		if (m_data)return m_data->Size;
		return 0;
	}
	inline uint32 GetCRC32()const { if (m_data)return m_data->CRC; return 0; }
	inline void set(const C*str)
	{
		m_data = Impl::BearStringConteniarManager::Get(str);
	}
	static inline const C* Reserve(const C* str)
	{
		return (const C *) Impl::BearStringConteniarManager::Get(str)->String;
	}
	inline const C* operator*() const
	{
		if (m_data)return (const C*)m_data->String;
		return (const C*)L"";
	}
	inline bool operator<(const  BearStringConteniarUnknown& strc)const
	{
		if (strc.m_data == 0 || m_data == 0)return strc.m_data < m_data;
		if (m_data->CRC != strc.GetCRC32())
			return m_data->CRC < strc.GetCRC32();
		return m_data->Depth < strc.m_data->Depth;
	}
	inline bool operator==(const  BearStringConteniarUnknown& strc)const
	{
		return m_data == strc.m_data;
	}
	inline bool operator!=(const  BearStringConteniarUnknown& strc)const
	{
		return m_data != strc.m_data;
	}
	inline bool operator<(const C* str)const
	{
		if (BearString::Compare(**this, str) < 0)
			return true;
		return false;
	}
	inline bool operator==(const C* str)const
	{
		if (BearString::Compare(**this, str) == 0)
			return true;
		return false;
	}

	~BearStringConteniarUnknown()
	{
	}
private:
	const Impl::BearStringConteniarData*m_data;
};
typedef BearStringConteniarUnknown<bchar16> BearStringConteniarUnicode;
typedef BearStringConteniarUnknown<bchar8> BearStringConteniarAnsi;
typedef BearStringConteniarUnknown<bchar> BearStringConteniar;