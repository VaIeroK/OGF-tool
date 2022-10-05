#pragma once
template<class Key,class T,class C=BearLess>
class BearMap :public std::map<Key,T,C, BearMemoryAllocator<std::pair<const Key,T>>>
{
private:
	typedef	std::map<Key, T, C, BearMemoryAllocator<std::pair<const Key, T>>> map;
public:
	BearMap(BearMap&&right) : 	map::map(right) {}
	BearMap(const BearMap&right) : 	map::map(right) {}
	inline void copy(const BearMap&right) { (*this) = right; }
	inline void copy(BearMap&&right) { (*this) = right; }
	inline BearMap&operator=(const BearMap&right)
	{
		map::operator=(right);
		return *this;
	}
	inline BearMap&operator=(BearMap&&right)
	{
		map::operator=(right);
		return *this;
	}
	BearMap() : map() {}
	inline  void insert(const Key&k, const T&t)
	{
		map::insert(std::pair<Key, T>(k, t));
	}
	inline  void insert( Key&&k, const T&t)
	{
		map::insert(std::pair<Key, T>(k, t));
	}
	inline  void insert(const Key&k,  T&&t)
	{
		map::insert(std::pair<Key, T>(k, t));
	}
	inline  void insert( Key&&k,  T&&t)
	{
		map::insert(std::pair<Key, T>(k, t));
	}
	inline  void insert(Key&&k)
	{
		map::insert(std::pair<Key, T>(k, T()));
	}
	inline  void insert(const Key&k)
	{
		map::insert(std::pair<Key, T>(k, T()));
	}
	void clear_not_free() { 	map::erase(	map::begin(), 	map::end()); }
};