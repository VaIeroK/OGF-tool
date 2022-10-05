#pragma once
template<class C>
class BearVector :public std::vector<C,BearMemoryAllocator<C>>
{
private:
	typedef	std::vector<C, BearMemoryAllocator<C>> vector;
public:
	BearVector(BearVector&&vector) : vector::vector(vector) {}
	BearVector(const BearVector&vector) : vector::vector(vector) {}
	void copy(const BearVector&right) { *this = right; }
	inline 	BearVector&operator=(BearVector&&str) { swap(str); return*this; }
	inline 	BearVector&operator=(const BearVector&right)
	{
		vector::operator=(right);
		return *this;
	}
	BearVector() : vector::vector() {}
	BearVector(bsize count, const C& value) :vector:: vector(count, value) {}
	explicit BearVector(bsize count) :vector:: vector(count) {}

	typename vector::const_reference operator[] (bsize pos) const { BEAR_ASSERT(pos<vector::size());  return (*(vector::begin() + pos)); }
	typename vector::reference operator[] (bsize pos) { BEAR_ASSERT(pos < vector::size());  return (*(vector::begin() + pos)); }
	void clear_not_free() { vector::erase(vector::begin(), vector::end()); }
	BearRef<C> pop_data() { auto ptr = BearRef<C>(&vector::at(0), true); this->_Myfirst() = vector::pointer(); this->_Mylast() = vector::pointer(); this->_Myend() = vector::pointer(); return ptr; }

};