#include "BearCore.hpp"

BearMemory::BearMemory()
{
}

BearMemory::~BearMemory()
{
}

void * BearMemory::Malloc(bsize size)
{
	return malloc(size);
}

void * BearMemory::Realloc(void * ptr, bsize new_size)
{
	return realloc(ptr, new_size);
}

void BearMemory::Free(void * ptr)
{
	 free(ptr);
}

void BearMemory::Swap(void * a, void * b, bsize size)
{
	char*c = reinterpret_cast<char*>(a);
	char*d = reinterpret_cast<char*>(b);
	while (size--)
	{
		char m = *c;
		*c = *d;
		*d = m;
		c++;
		d++;
	}
}