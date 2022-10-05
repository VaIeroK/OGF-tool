#pragma once
template < class U> class BearFlags
{
public:
	BearFlags() :m_flags(0) {}
	BearFlags(U f) :m_flags(f) {}
	inline BearFlags&clear() { m_flags = 0; return*this; }
	inline BearFlags&fill() { m_flags = U(-1); return*this;}
	inline BearFlags&invert() { m_flags = ~m_flags; return*this; }
	inline BearFlags&invert(const BearFlags& right) { m_flags = ~right.m_flags;  return*this;}
	inline BearFlags&invert(U right) { m_flags ^= right;  return*this;}
	inline void copy(const BearFlags&right) { m_flags = right.m_flags; }
	inline BearFlags&assign(U p) { m_flags = p; return*this;}
	inline BearFlags&set(bool bit, U mask) {if (bit) m_flags |= mask; else m_flags = m_flags & ~mask; return*this;}
	inline bool is(U p)const { return p==(m_flags & p); }
	inline bool test(U p)const { return m_flags & p; }
	inline BearFlags&AND(U p) { m_flags &=p; return*this; }
	inline BearFlags&OR(U p) { m_flags |= p; return*this; }
	inline BearFlags&AND(const BearFlags& right) { m_flags &= right.m_flags; return*this; }
	inline BearFlags& OR (const BearFlags& right) { m_flags |= right.m_flags; return*this; }
	inline void swap( BearFlags&right) { m_flags = right.m_flags; }
	BearFlags(const BearFlags& right) :m_flags(0) { copy(right); }
	BearFlags(BearFlags&& right) :m_flags(0) { swap(right); }
	inline BearFlags&operator=(const BearFlags&right){ copy(right); return*this;}
	inline BearFlags&operator=( BearFlags&&right) { swap(right); return*this; }
	inline BearFlags&operator=(U right) { m_flags=right; return*this; }
	inline bool operator==(const BearFlags&right) { return m_flags == right.m_flags; }
	inline U operator*()const { return m_flags; }

private:
	U m_flags;
};
