#pragma once
template <typename T>
class BearMemoryAllocator
{
public:
	typedef bsize size_type;
	typedef bint difference_type;
	typedef T* pointer;
	typedef const T* const_pointer;
	typedef T& reference;
	typedef const T& const_reference;
	typedef T value_type;

public:
	template<class _Other>
	struct rebind { typedef BearMemoryAllocator<_Other> other; };
public:
	pointer address(reference _Val) const { return (&_Val); }
	const_pointer address(const_reference _Val) const { return (&_Val); }
	BearMemoryAllocator() { }
	BearMemoryAllocator(const BearMemoryAllocator<T>&) { }
	template<class _Other> BearMemoryAllocator(const BearMemoryAllocator<_Other>&) { }
	template<class _Other> BearMemoryAllocator<T>& operator= (const BearMemoryAllocator<_Other>&) { return (*this); }
	bool operator== (const BearMemoryAllocator&) { return true; }
	bool operator!= (const BearMemoryAllocator&) { return false; }
	template<class _Other>
	bool operator== (const BearMemoryAllocator<_Other>&) { return false; }
	template<class _Other>
	bool operator!= (const BearMemoryAllocator<_Other>&) { return true; }
	pointer allocate(size_type n, const void* p = 0) const { return bear_alloc<T>(n); }
	char* _charalloc(size_type n) { return (char*)allocate(n); }
	void deallocate(pointer p, size_type n) const { bear_free(p); }
	void deallocate(void* p, size_type n) const { bear_free(p); }
	template<class C, class U>
	void construct(C* p, const U& _Val) { ::new ((void*)p) C(_Val); }
	template<class C>
	void destroy(C* p) { p->~C(); }
	size_type max_size() const { size_type _Count = (size_type)(-1) / sizeof(T); return (0 < _Count ? _Count : 1); }


};