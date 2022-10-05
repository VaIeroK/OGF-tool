#pragma once

template<typename P>
struct BearRef
{
public:
	BearRef(P*p, bool auto_free = true) :m_p(p), m_free(auto_free){}
	BearRef() :m_p(0), m_free(false){};
	BearRef(const BearRef& ptr): m_p(ptr.m_p), m_free(ptr.m_free){ ptr.m_p = 0; ptr.m_free = false; };
	BearRef(BearRef&&ptr) : m_p(ptr.m_p), m_free(ptr.m_free) { ptr.m_p = 0; ptr.m_free = false;};

	void clear()
	{
		if (!m_p)return;
		if (m_free) {
			
				bear_delete(m_p);
		}
		m_free = false;
		m_p = 0;
	}

	void clear_no_free() { m_free = false; m_p = 0; }

	~BearRef() { clear(); }
	P*operator->() { return m_p; }
	const P*operator->()const { return m_p; }
	P*operator*() { return m_p; }
	const P*operator*()const { return m_p; }


	BearRef&operator=(BearRef&&ptr) { clear(); swap(ptr);    return*this; };
	BearRef&operator=(const BearRef&ptr) { clear(); copy(ptr);    return*this; };
	void copy(const BearRef& ptr) { clear();     m_p = ptr.m_p; ptr.m_p = 0; m_free = ptr.m_free; ptr.m_free = false; }
	void swap(BearRef& ptr) { bear_swap(ptr.m_p, m_p);  bear_swap(ptr.m_free, m_free);  }


	P&operator[](bsize id) { return m_p[id]; }
	const P&operator[](bsize id) const { return m_p[id]; }
private:
	mutable	P*m_p;
	mutable bool m_free;
};

template<>
struct BearRef<void*>
{
public:
	BearRef(void*p, bool auto_free = true) :m_p(p), m_free(auto_free)
	{}
	BearRef() :m_p(0), m_free(false) {};
	BearRef(const BearRef&ptr) :m_p(ptr.m_p), m_free(ptr.m_free) { ptr.m_p = 0; ptr.m_free = false; };
	BearRef(BearRef&&ptr) :m_p(ptr.m_p), m_free(ptr.m_free) { ptr.m_p = 0; ptr.m_free = false; };

	void clear()
	{
		if (!m_p)return;
		if (m_free) {

			bear_free(m_p);
		}
		m_free = false;
		m_p = 0;
	}

	void clear_no_free() { m_free = false; m_p = 0; }

	~BearRef() { clear(); }

	void*operator*() { return m_p; }
	const void*operator*()const { return m_p; }


	BearRef&operator=(BearRef&&ptr) { clear(); swap(ptr);    return*this; };
	BearRef&operator=(const BearRef&ptr) { clear(); swap(ptr);    return*this; };
	void copy(const BearRef&ptr) { clear(); m_p = ptr.m_p; ptr.m_p = 0; m_free = ptr.m_free; ptr.m_free = false; }
	void swap(const BearRef&ptr) { bear_swap(ptr.m_p, m_p);  bear_swap(ptr.m_free, m_free); }

private:
	mutable	void*m_p;
	mutable bool m_free;
};
