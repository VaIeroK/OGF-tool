#pragma once

template<class P>
struct BearMultiRef
{
private:
	struct data
	{
		bsize m_count;
		P m_data;
	};
	data*m_data;
public:
	BearMultiRef():m_data(0)
	{
	}
	~BearMultiRef()
	{
		clear();
	}
	BearMultiRef(const BearMultiRef&ptr) :m_data(0)
	{
		copy(ptr);
	}
	BearMultiRef( BearMultiRef&&ptr) :m_data(0)
	{
		swap(ptr);
	}
	bool inline empty()const
	{
		return !m_data;
	}
	inline void create()
	{
		clear();
		m_data = bear_new<data>();
		m_data->m_count = 1;
	}
	inline void clear()
	{
		if (!m_data ) { return; }
		m_data->m_count--;
		if (!m_data->m_count)
		{
			m_data->m_data.~P();
			bear_free(m_data);
				
		}
		m_data = 0;
		return;
	}
	inline P*get()
	{
		if (!m_data ) {  create(); }
		return &m_data->m_data;
	}
	inline P*get() const
	{
		if (!m_data ) { return 0; }
		return &m_data->m_data;
	}
	const P*operator->()const { return get(); }
	P*operator->() { return get(); }
	bool is_one()const { if (m_data)return m_data->m_count == 1; return false; }
	BearMultiRef&operator=(const BearMultiRef&ptr) { copy(ptr); return*this; };
	BearMultiRef&operator=( BearMultiRef&&ptr) { swap(ptr); return*this; };
	void swap(BearMultiRef&ptr)
	{
		bear_swap(ptr.m_data, m_data);
	}
	void copy(const BearMultiRef&ptr)
	{
		if (ptr.m_data != m_data)
		{
			clear();
			m_data = ptr.m_data;
			if (!m_data ) { return; }
			m_data->m_count++;
		}
	}


};