#pragma once
namespace Impl
{
	template<class C>
	struct BearFunctionReturner
	{
		inline C&operator()(void*a)
		{
			return *(C*)a;
		}
	};
	template<>
	struct BearFunctionReturner<void>
	{
		inline void operator()(void*a)
		{
			(void)a;
		}
	};
	template<class C>
	struct BearFunctionRefCaller
	{
		template<typename  F,class M,class...A>
		inline void operator()(F f, M&c,A...a)
		{
			c = f(*a...);
		}
		template<typename  F, class M>
		inline void operator()(F f, M&c)
		{
			c = f();
		}
	};

	template<>
	struct BearFunctionRefCaller<void>
	{
		template<typename  F, class...A>
		inline void operator()(F f, int , A...a)
		{
			f(*a...);
		}
		template<typename  F>
		inline void operator()(F f, int)
		{
				f();
		}
	};


	template<class C>
	struct BearClassFunctionRefCaller
	{
		template<typename  F, class CL,class M, class...A>
		inline void operator()(F f, CL *cl, M&c, A...a)
		{
			c = (cl->*f)(*a...);
		}
		template<typename  F, class CL, class M>
		inline void operator()(F f, CL *cl, M&c)
		{
			c = (cl->*f);
		}
	};

	template<>
	struct BearClassFunctionRefCaller<void>
	{
		template<typename  F, class CL, class...A>
		inline void operator()(F f, CL *cl, int, A...a)
		{
			(cl->*f)(*a...);
		}
		template<typename  F, class CL>
		inline void operator()(F f, CL *cl, int)
		{
			(cl->*f)();
		}
	};

	template<class C>
	struct BearClassFunctionRefCaller2
	{
		template<typename  F, class CL, class M, class...A>
		inline void operator()(F f, CL *cl, M&c, A...a)
		{
			c = f(cl,*a...);
		}
		template<typename  F, class CL, class M>
		inline void operator()(F f, CL *cl, M&c)
		{
			c = f(cl);
		}
	};

	template<>
	struct BearClassFunctionRefCaller2<void>
	{
		template<typename  F, class CL, class...A>
		inline void operator()(F f, CL *cl, int, A...a)
		{
			f(cl,*a...);
		}
		template<typename  F, class CL>
		inline void operator()(F f, CL *cl, int)
		{
			f(cl);
		}
	};
};

class BearArgumentContainer
{

public:
	virtual ~BearArgumentContainer() {}
	virtual void*Get()=0;
private:
};


template<class C>
class BearArgumentContainerMaker:public BearArgumentContainer
{
public:
	BearArgumentContainerMaker(const C&c):m_c(c){}
	virtual ~BearArgumentContainerMaker() {};
	virtual void*Get()
	{
		return &m_c;
	}
private:
	C m_c;
};

class BearArgumentsList :public BearVector<BearArgumentContainer*>
{
public:
	inline void clear()
	{
		auto b = begin();
		auto e = end();
		while (b != e)
		{
			bear_delete(*b);
			b++;
		}
		BearVector<BearArgumentContainer*>::clear();
	}
	inline ~BearArgumentsList() { clear(); }
};

template<typename ...Args >
void bear_make_arguments(BearArgumentsList&args_list, Args&&... args)
{
	bear_make_arguments_impl(args_list, args...);
}
template<typename T, typename ...Args>
void bear_make_arguments_impl(BearArgumentsList&args_list,  T&&t, Args&&... args)
{
	args_list.push_back(bear_new<BearArgumentContainerMaker<typename bear_remove_const<typename bear_remove_reference<T>::type>::type>>(t));
	bear_make_arguments_impl(args_list, args...);
}
template<typename T>
void bear_make_arguments_impl(BearArgumentsList&args_list, T&&t)
{
	args_list.push_back(bear_new<BearArgumentContainerMaker<typename bear_remove_const<typename bear_remove_reference<T>::type>::type>>( t));
}



class BearFunctionRef
{
public:
	virtual ~BearFunctionRef(){}
	template<class R>
	inline R CallWithArgumentsList(void**stack)
	{
		return Impl::BearFunctionReturner<typename bear_remove_reference<R>::type>()(Call(stack));
	}
	template<class R>
	inline R CallWithArgumentsList(const BearArgumentsList&args)
	{
		void*stack[256];
		auto begin = args.begin();
		auto end = args.end();
		bsize i = 0;
		while (begin != end)
		{
			stack[i++] = (*begin)->Get();
			begin++;
		}
		return Impl::BearFunctionReturner<typename bear_remove_reference<R>::type>()(Call(stack));
	}
#define CALL_IN_REF
#define NAMEINCLUDE "BearFunctionReflectorMaker.h"
#include "BearMultiplicator.h"
#undef CALL_IN_REF
protected:
	virtual void* Call(void**args) = 0;

private:
};
template<class C,typename F>
class BearFunctionRefMaker:public BearFunctionRef
{
public:
	BearFunctionRefMaker(F f) :m_f(f) {}
	virtual ~BearFunctionRefMaker() {}
private:

#define CALL_IN_MAKER
#define NAMEINCLUDE "BearFunctionReflectorMaker.h"
#include "BearMultiplicator.h"
#undef CALL_IN_MAKER
	virtual void* Call(void**args)
	{
		call_impl(m_f, args);
		return &m_return;
	}

	F  m_f;
	C m_return;
};
template<typename P,class...A>
inline  BearFunctionRef*bear_create_function_ref(P(*f)(A...))
{
	return bear_new< BearFunctionRefMaker<typename bear_remove_reference<typename bear_remove_void<P>::type>::type, decltype(f)>>(f);
}
template<typename P>
inline BearFunctionRef*bear_create_function_ref(P(*f)())
{
	return bear_new< BearFunctionRefMaker<typename bear_remove_reference<typename bear_remove_void<P>::type>::type, decltype(f)>>(f);
}


class BearClassFunctionRef
{
public:
	virtual ~BearClassFunctionRef(){}
	template<class R>
	inline R CallWithArgumentsList(void*cl,void**stack)
	{
		return Impl::BearFunctionReturner<typename bear_remove_reference<R>::type>()(Call(cl,stack));
	}
	template<class CL,class R>
	inline R CallWithArgumentsList(CL*cl,const BearArgumentsList&args)
	{
		void*stack[256];
		auto begin = args.begin();
		auto end = args.end();
		bsize i = 0;
		while (begin != end)
		{
			stack[i++] = (*begin)->Get();
			begin++;
		}
		return Impl::BearFunctionReturner<typename bear_remove_reference<R>::type>()(Call((void*)cl,stack));
	}
#define CALL_IN_REF_CL
#define NAMEINCLUDE "BearFunctionReflectorMaker.h"
#include "BearMultiplicator.h"
#undef CALL_IN_REF_CL
protected:
	virtual void* Call(void*cl,void**args) = 0;

private:
};


template<class C, typename F>
class BearClassFunctionRefMaker :public BearClassFunctionRef
{
public:
	BearClassFunctionRefMaker(F f) :m_f(f) {}
	virtual ~BearClassFunctionRefMaker(){}
private:
#define CALL_IN_MAKER_CL
#define NAMEINCLUDE "BearFunctionReflectorMaker.h"
#include "BearMultiplicator.h"
#undef CALL_IN_MAKER_CL
	virtual void* Call(void*cl,void**args)
	{
		call_impl(m_f, cl, args);
		return &m_return;
	}

	F  m_f;
	C m_return;
};

template<class CL, typename P, class...A>
inline  BearClassFunctionRef*bear_create_class_function_ref(P(CL::*f)(A...))
{
	return bear_new< BearClassFunctionRefMaker<typename bear_remove_reference<typename bear_remove_void<P>::type>::type, decltype(f)>>(f);
}
template<class CL, typename P>
inline BearClassFunctionRef*bear_create_class_function_ref(P(CL::*f)())
{
	return bear_new< BearClassFunctionRefMaker<typename bear_remove_reference<typename bear_remove_void<P>::type>::type, decltype(f)>>(f);
}

template<class CL, typename P, class...A>
inline  BearClassFunctionRef*bear_create_class_function_ref(P(*f)(CL*,A...))
{
	return bear_new< BearClassFunctionRefMaker<typename bear_remove_reference<typename bear_remove_void<P>::type>::type, decltype(f)>>(f);
}
template<class CL, typename P>
inline BearClassFunctionRef*bear_create_class_function_ref(P(*f)(CL*))
{
	return bear_new< BearClassFunctionRefMaker<typename bear_remove_reference<typename bear_remove_void<P>::type>::type, decltype(f)>>(f);
}