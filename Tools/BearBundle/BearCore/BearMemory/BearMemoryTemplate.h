#pragma once
#pragma warning(disable:4984)

template<typename C>
inline void bear_copy(C*dst, const C*source, bsize count)
{
	memcpy(reinterpret_cast<void*>(dst), reinterpret_cast<const void*>(source), count * sizeof(C));
}

inline void bear_copy(void*dst, const void*source, bsize size)
{
	memcpy(dst, source, size);
}

template<typename C>
inline void bear_move(C*dst, bint move, bsize count)
{
	memmove(dst + move, dst, sizeof(C)*count);
}
inline void bear_move(void*dst, bint move, bsize size)
{
	memmove(reinterpret_cast<char*>(dst) + move, dst, size);
}
template<typename C>
inline void bear_move(C*dst, const C * src, bsize count)
{
	memmove(dst , src, sizeof(C)*count);
}
inline void bear_move(void*dst,const void * src, bsize size)
{
	memmove(dst, src, size);
}
template<typename C>
inline void bear_fill(C*c, bsize count, uint8 byte = 0)
{
	memset(c, byte, sizeof(C)*count);
}
inline void bear_fill(void*dst, bsize size, uint8 byte = 0)
{
	memset(dst, byte, size);
}
template<typename C>
inline void bear_fill(C&c)
{
	bear_fill(&c, 1);
}
template<typename C>
inline void bear_swap(C&a, C&b)
{
	if constexpr  (sizeof(C) == sizeof(uint64))
	{
		uint64 c = *reinterpret_cast<uint64*>(&a);
		*reinterpret_cast<uint64*>(&a) = *reinterpret_cast<uint64*>(&b);
		*reinterpret_cast<uint64*>(&b) = c;
	}
	else if constexpr  (sizeof(C) == sizeof(uint32))
	{
		uint32 c = *reinterpret_cast<uint32*>(&a);
		*reinterpret_cast<uint32*>(&a) = *reinterpret_cast<uint32*>(&b);
		*reinterpret_cast<uint32*>(&b) = c;
	}
	else if constexpr (sizeof(C) == sizeof(uint16))
	{
		uint16 c = *reinterpret_cast<uint16*>(&a);
		*reinterpret_cast<uint16*>(&a) = *reinterpret_cast<uint16*>(&b);
		*reinterpret_cast<uint16*>(&b) = c;
	}
	else if constexpr (sizeof(C) == sizeof(uint8))
	{
		uint8 c = *reinterpret_cast<uint8*>(&a);
		*reinterpret_cast<uint8*>(&a) = *reinterpret_cast<uint8*>(&b);
		*reinterpret_cast<uint8*>(&b) = c;
	}
	else
	{
		BearCore::GetMemory()->Swap(&a, &b, sizeof(C));
	}
}
template<typename C>
inline void bear_swap(C*a, C*b, bsize count)
{
	C*end = a + count;
	while (a != end)
	{
		bear_swap<C>(*a, *b);
		a++; b++;
	}
}

inline void bear_swap(void*a, void*b, bsize size)
{
	bear_swap<char>(reinterpret_cast<char*>(a), reinterpret_cast<char*>(b), size);
}

template<typename C>
inline bint bear_compare(const C*a, const C*b, bsize count)
{
	return static_cast<bint>(memcmp(reinterpret_cast<const void*>(a), reinterpret_cast<const void*>(b), sizeof(C)*count));
}
inline bint bear_compare(const void*a, const void*b, bsize size)
{
	return static_cast<bint>(memcmp(a, b, size));
}

template<typename C>
inline C*bear_alloc(bsize count)
{
	return reinterpret_cast<C*>(BearCore::GetMemory()->Malloc(sizeof(C)*count));
}

template<typename C>
inline C*bear_new()
{
	C*c = reinterpret_cast<C*>(BearCore::GetMemory()->Malloc(sizeof(C)));
	new(c)C();
	return c;
}
template<typename C, typename...D>
inline C*bear_new(const D&...d)
{
	C*c = reinterpret_cast<C*>(BearCore::GetMemory()->Malloc(sizeof(C)));
	new(c)C(d...);
	return c;
}
#ifdef MSVC 
template<typename C, typename...D>
inline C*bear_new(D&&...d)
{
	C*c = reinterpret_cast<C*>(BearCore::GetMemory()->Malloc(sizeof(C)));
	new(c)C(d...);
	return c;
}
#endif
template<typename C>
inline C*bear_realloc(C*old, bsize new_count)
{
	return reinterpret_cast<C*>(BearCore::GetMemory()->Realloc(reinterpret_cast<void*>(old), sizeof(C)*new_count));
}

template<typename C>
inline void bear_free(C*old)
{
	if (old)
	{
		if constexpr (bear_is_polymorphic<C>::value)
		{
			const void* real_ptr = dynamic_cast<const void*>(old);
			BearCore::GetMemory()->Free(const_cast<void*>(real_ptr));
		}
		else
		{
			BearCore::GetMemory()->Free(const_cast<void*>(reinterpret_cast<const void*>(old)));
		}
	}

}
template<typename C>
inline void bear_delete(C*old)
{
	if (old)
	{
	
		if constexpr (bear_is_polymorphic<C>::value)
		{
			void* real_ptr = dynamic_cast<void*>(old);
			old->~C();
			BearCore::GetMemory()->Free(real_ptr);
		}
		else
		{
			old->~C();
			BearCore::GetMemory()->Free(reinterpret_cast<void*>(old));
		}
	}
		
	
}
inline void bear_delete(void*old)
{
	BearCore::GetMemory()->Free(old);
}
inline bsize bear_recommended_size(bsize size)
{
	bsize out = 1;
	while (out <= size)
	{
		out <<= 1;
	}
	return out;
}
