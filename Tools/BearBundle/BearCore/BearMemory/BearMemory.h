#pragma once
class BEARTOOL_API BearMemory
{
	BEAR_CLASS_WITHOUT_COPY(BearMemory)
public:
	BearMemory();
	virtual ~BearMemory();
	virtual void* Malloc(bsize size);
	virtual void* Realloc(void*ptr, bsize new_size);
	virtual void Free(void*ptr);
	virtual void Swap(void*a, void*b, bsize size);
};