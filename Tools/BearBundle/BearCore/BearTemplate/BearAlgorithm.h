#pragma once
namespace Impl
{
	template<class T, typename C, typename S>
	inline void impl_downItemInHeap(T a, bsize id, bsize size, C&Comp,S &swap)
	{
	//	BEAR_ASSERT(id < size);
		bsize child = 0;
		while ((child = id * 2 + 1) < size)
		{
			if (child < size - 1 && Comp(a[child], a[child + 1]))
				child++;
			if (Comp(a[child], a[id]))break;
			swap(a[id], a[child]);
			id = child;
		}
	}
	template<typename T, typename C>
	inline T  impl_find_if(T begin, T end, C&Comp)
	{
		while (begin != end)
		{
			if (Comp(*begin))
				break;
			begin++;
		}
		return begin;
	}
	template<typename T, typename C>
	inline  T  impl_find_if_of_end(T begin, T end, C&Comp)
	{
		T end_ = end;
		end_--;
		while (end_ >= begin)
		{
			if (Comp(*end_))
				break;
			end_--;
		}
		if(end_ >= begin)
		return end_;
		else return end;
	}
	template<typename T, typename C, typename M>
	inline T  impl_find_if(T begin, T end,M&m, C&Comp)
	{
		while (begin != end)
		{
			if (Comp(*begin,m))
				break;
			begin++;
		}
		return begin;
	}
	template<typename T, typename C, typename M>
	inline  T  impl_find_if_of_end(T begin, T end, M&m, C&Comp)
	{
		T end_ = end;
		end_--;
		while (end_ >= begin)
		{
			if (Comp(*end_,m))
				break;
			end_--;
		}
		if (end_ >= begin)
			return end_;
		else return end;
	}

	template<class T,typename C, typename S>
	inline void downItemInHeap(T a, bsize id, bsize size,C&Comp, S &swap)
	{

		impl_downItemInHeap(a, id, size, Comp, swap);
	}
	template<class T, typename C, typename S>
	inline void downItemInHeap(T a, bsize id, bsize size, C&&Comp, S &swap)
	{

		impl_downItemInHeap(a, id, size, Comp, swap);
	}
	template<typename T, typename C, typename S>
	inline void impl_pop_heap(T begin, T end, C&Comp,S&swap)
	{
		end--;
		bear_swap(*begin, *end);
		downItemInHeap(begin, 0, end - begin, Comp,swap);
	}
	template <typename T, typename C, typename L>
	inline T impl_upper_bound(T begin, T end, C& val, L&comp)
	{
		T it;
		bsize count = end - begin, step = 0;
		while (count > 0)
		{
			it = begin;
			step = count / 2; it += step;
			if (!comp(val, *it))
			{
				begin = it + 1;
				count -= step + 1;
			}
			else
			{
				count = step;
			}
		}
		return begin;
	}
	template <typename T, typename C, typename L>
	inline T impl_lower_bound(T begin, T end, C& val, L&comp)
	{
		T it;
		bsize count = end - begin, step = 0;
		while (count > 0)
		{
			it = begin;
			step = count / 2; it += step;
			if (comp(*it, val))
			{
				begin = it + 1;
				count -= step + 1;
			}
			else
			{
				count = step;
			}
		}
		return begin;
	}
	template<typename T, typename C,typename S>
	inline void impl_make_heap(T begin, T end, C&Comp,S&swap )
	{
		for (bsize i = (end - begin) / 2; i != 0; i--)
			downItemInHeap(begin, i - 1, end - begin, Comp, swap);
	}
	template<typename T, typename C, typename S>
	inline void impl_push_heap(T begin, T end, C&Comp, S&swap)
	{
		bsize size = (end - begin);

		for (bsize id = size / 2; id != 0; id = id / 2)
		{
			id--;
			bsize child = id * 2 + 1;
			if (child < size - 1 && Comp(begin[child], begin[child + 1]))
				child++;
			if (Comp(begin[child], begin[id]))break;
			swap(begin[id], begin[child]);
			id++;
		}

	}
	template<typename T, typename C, typename S>
	inline void  impl_sort_heap(T begin, T end, C&Comp, S&swap)
	{
		for (bsize i = (end - begin); i > 1; i--)
			impl_pop_heap(begin, begin + i, Comp, swap);
	}
	template<typename T, typename C, typename S >
	inline void impl_sort(T begin, T end, C&Comp, S&swap)
	{
		bsize size = end - begin;
		if (size == 1)return;
		if (size == 2) { if (Comp(begin[1], begin[0]))swap(begin[0], begin[1]); return; }
		T mid = begin + (size >> 1);
		T first = begin, second = end - 1;
		do
		{
			while (Comp(*first, *mid))first++;
			while (Comp(*mid, *second))second--;
			if (first == second) { first++; second--; }
			else
				if (first < second)
				{
					if (first == mid)mid = second;
					else if (second == mid)mid = first;
					swap(*first, *second); first++; second--;
				}

		} while (first <= second);
		if (second - begin > 32)
			impl_sort(begin, second + 1, Comp, swap);
		else if (second - begin>0)
		{
			impl_make_heap(begin, second + 1,Comp, swap);
			impl_sort_heap(begin, second + 1, Comp, swap);
		}
		if (end - first>32)
			impl_sort(first, end, Comp, swap);
		else if (end - first >0)
		{
			impl_make_heap(first, end, Comp, swap);
			impl_sort_heap(first, end, Comp, swap);
		}
		return;
	}
}
template <typename T, typename C,typename L=BearDefaultCompare>
inline T bear_upper_bound(T begin, T end, C&& val,L&&comp = BearDefaultCompare())
{
	return	Impl::impl_upper_bound(begin, end, val, comp);
}
template <typename T, typename C, typename L=BearDefaultCompare>
inline T bear_upper_bound(T begin, T end, C& val, L&&comp = BearDefaultCompare())
{
	return	Impl::impl_upper_bound(begin, end, val, comp);
}
template <typename T, typename C, typename L>
inline T bear_upper_bound(T begin, T end, C&& val, L&comp)
{
	return	Impl::impl_upper_bound(begin, end, val, comp);
}
template <typename T, typename C, typename L>
inline T bear_upper_bound(T begin, T end, C& val, L&comp )
{
	return	Impl::impl_upper_bound(begin, end, val, comp);
}

template <typename T, typename C, typename L=BearDefaultCompare>
inline T bear_lower_bound(T begin, T end, C&& val, L&&comp=BearDefaultCompare())
{
	return	Impl::impl_lower_bound(begin, end, val, comp);
}
template <typename T, typename C, typename L=BearDefaultCompare>
inline T bear_lower_bound(T begin, T end, C& val, L&&comp= BearDefaultCompare())
{
return	Impl::impl_lower_bound(begin, end, val, comp);
}
template <typename T, typename C, typename L>
inline T bear_lower_bound(T begin, T end, C&& val, L&comp)
{
	return	Impl::impl_lower_bound(begin, end, val, comp);
}
template <typename T, typename C, typename L>
inline T bear_lower_bound(T begin, T end, C& val, L&comp)
{
	return	Impl::impl_lower_bound(begin, end, val, comp);
}





template<typename T, typename C ,typename S=BearDefaultSwap>
inline void bear_make_heap(T begin, T end, C&Comp,S&&Swap=BearDefaultSwap())
{
	Impl::impl_make_heap(begin, end, Comp,Swap );
}
template<typename T, typename C ,typename S>
inline void bear_make_heap(T begin, T end, C&Comp,S&Swap)
{
	Impl::impl_make_heap(begin, end, Comp,Swap );
}
template<typename T, typename C=BearDefaultCompare,typename S=BearDefaultSwap>
inline void bear_make_heap(T begin, T end, C&&Comp = BearDefaultCompare() ,S&&Swap = BearDefaultSwap() )
{
	Impl::impl_make_heap(begin, end, Comp, Swap);
}

	
template<typename T, typename C ,typename S=BearDefaultSwap>
inline void bear_pop_heap(T begin, T end, C&Comp,S&&Swap=BearDefaultSwap())
{
	Impl::impl_pop_heap(begin, end, Comp,Swap );
}
template<typename T, typename C ,typename S>
inline void bear_pop_heap(T begin, T end, C&Comp,S&Swap)
{
	Impl::impl_pop_heap(begin, end, Comp,Swap );
}
template<typename T, typename C=BearDefaultCompare,typename S=BearDefaultSwap>
inline void bear_pop_heap(T begin, T end, C&&Comp = BearDefaultCompare() ,S&&Swap = BearDefaultSwap() )
{
	Impl::impl_pop_heap(begin, end, Comp, Swap);
}
	
template<typename T, typename C ,typename S=BearDefaultSwap>
inline void bear_sort_heap(T begin, T end, C&Comp,S&&Swap=BearDefaultSwap())
{
	Impl::impl_sort_heap(begin, end, Comp,Swap );
}
template<typename T, typename C ,typename S>
inline void bear_sort_heap(T begin, T end, C&Comp,S&Swap)
{
	Impl::impl_sort_heap(begin, end, Comp,Swap );
}
template<typename T, typename C=BearDefaultCompare,typename S=BearDefaultSwap>
inline void bear_sort_heap(T begin, T end, C&&Comp = BearDefaultCompare() ,S&&Swap = BearDefaultSwap() )
{
	Impl::impl_sort_heap(begin, end, Comp, Swap);
}


		
template<typename T, typename C ,typename S=BearDefaultSwap>
inline void bear_push_heap(T begin, T end, C&Comp,S&&Swap=BearDefaultSwap())
{
	Impl::impl_push_heap(begin, end, Comp,Swap );
}
template<typename T, typename C ,typename S>
inline void bear_push_heap(T begin, T end, C&Comp,S&Swap)
{
	Impl::impl_push_heap(begin, end, Comp,Swap );
}
template<typename T, typename C=BearDefaultCompare,typename S=BearDefaultSwap>
inline void bear_push_heap(T begin, T end, C&&Comp = BearDefaultCompare() ,S&&Swap = BearDefaultSwap() )
{
	Impl::impl_push_heap(begin, end, Comp, Swap);
}


template<typename T, typename C,typename S >
inline void bear_sort(T begin, T end, C&Comp,S&Swap  )
{
	Impl::impl_sort(begin, end, Comp, Swap);
}
	template<typename T, typename C ,typename S=BearDefaultSwap>
inline void bear_sort(T begin, T end, C&Comp ,S&&Swap = BearDefaultSwap() )
{
	Impl::impl_sort(begin, end, Comp,Swap);
}
template<typename T, typename C=BearDefaultCompare,typename S=BearDefaultSwap>
inline void bear_sort(T begin, T end, C&&Comp = BearDefaultCompare(),S&&Swap = BearDefaultSwap())
{
	Impl::impl_sort(begin, end, Comp, Swap);
}


template<typename T, typename C>
inline T  bear_find_if(T begin, T end, C&&Comp)
{
	return Impl::impl_find_if(begin, end, Comp);
}
template<typename T, typename C>
inline T  bear_find_if(T begin, T end, C&Comp)
{
	return Impl::impl_find_if(begin, end, Comp);
}
template<typename T, typename C>
inline T  bear_find_if_of_end(T begin, T end, C&Comp)
{
	return Impl::impl_find_if_of_end(begin, end, Comp);
}
template<typename T, typename C>
inline T  bear_find_if_of_end(T begin, T end, C&&Comp)
{
	return Impl::impl_find_if_of_end(begin, end, Comp);
}


template<typename T, typename C, typename M>
inline T  bear_find_if(T begin, T end, M&&m, C&&Comp)
{
	return Impl::impl_find_if(begin, end, m, Comp);
}

template<typename T, typename C, typename M>
inline T  bear_find_if(T begin, T end, M&&m, C&Comp)
{
	return Impl::impl_find_if(begin, end, m, Comp);
}

template<typename T, typename C, typename M>
inline T  bear_find_if(T begin, T end, M&m, C&&Comp)
{
	return Impl::impl_find_if(begin, end, m, Comp);
}

template<typename T, typename C, typename M>
inline T  bear_find_if(T begin, T end,  M&m, C&Comp)
{
	return Impl::impl_find_if(begin, end,m, Comp);
}

template<typename T, typename C, typename M>
inline T  bear_find_if_of_end(T begin, T end,  M&m, C&Comp)
{
	return Impl::impl_find_if_of_end(begin, end,m, Comp);
}
template<typename T, typename C, typename M>
inline T  bear_find_if_of_end(T begin, T end, M&m, C&&Comp)
{
	return Impl::impl_find_if_of_end(begin, end, m, Comp);
}
template<typename T, typename C, typename M>
inline T  bear_find_if_of_end(T begin, T end, M&&m, C&Comp)
{
	return Impl::impl_find_if_of_end(begin, end, m, Comp);
}
template<typename T, typename C, typename M>
inline T  bear_find_if_of_end(T begin, T end, M&&m, C&&Comp)
{
	return Impl::impl_find_if_of_end(begin, end, m, Comp);
}