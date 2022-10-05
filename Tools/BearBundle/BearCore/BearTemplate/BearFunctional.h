#pragma once
template <typename T> struct bear_remove_reference
{
	typedef T type;
};
template <typename T> struct bear_remove_reference<T&> {
	typedef T type;
};
template <typename T> struct bear_remove_reference<T&&> {
	typedef T type;
};
template <typename T> struct bear_remove_pointer
{
	typedef T type;
};
template <typename T> struct bear_remove_pointer<T*> {
	typedef T type;
};
template <typename T> struct bear_remove_const
{
	typedef T type;
};
template <typename T> struct bear_remove_const<T const> {
	typedef T type;
};


template <typename T> struct bear_remove_void {
	typedef T type;
};
template <>
struct bear_remove_void<void> {
	typedef int type;
};


template <typename T> struct bear_is_const {
	enum { value = false };
};

template <typename T> struct bear_is_const<T const> {
	enum { value = true };
};

template <typename T> struct bear_is_reference {
	enum { value = false };
};

template <typename T> struct bear_is_reference<T &> {
	enum { value = true };
};
template <typename T> struct bear_is_reference<T &&> {
	enum { value = true };
};
template <typename T> struct bear_is_void {
	enum { value = false };
};

template <> struct bear_is_void<void> {
	enum { value = true };
};

template <typename T> struct bear_is_pointer {
	enum { value = false };
};

template <typename T> struct bear_is_pointer<T *> {
	enum { value = true };
};


template<typename A>
struct bear_is_void_function
{
	struct yes { int8 a; };
	struct no { int16 a; };
	template<typename U, typename ...args>
	static no test(U(*a)(args...)) { return no(); };
	template<typename ...args>
	static yes test(void(*a)(args...)) { return yes(); };
	enum { ressult = sizeof(yes) == sizeof(test((A)0)) };
};

namespace Impl {
 
template <class T>
std::true_type detect_is_polymorphic(
    decltype(dynamic_cast<const volatile void*>(static_cast<T*>(nullptr)))
);
template <class T>
std::false_type detect_is_polymorphic(...);
 
} 
 
template <class T>
struct bear_is_polymorphic : decltype(Impl::detect_is_polymorphic<T>(nullptr)) {};


namespace Impl {
	template <class T> char test(int T::*);
	struct two { char c[2]; };
	template <class T> two test(...);
}

template <class T>
struct bear_is_class : std::integral_constant<bool, sizeof(Impl::test<T>(0)) == 1
	&& !std::is_union<T>::value> {};

struct BearDefaultSwap
{
	template < class P>
	void operator()(P&p1, P&p2)
	{
		bear_swap(p1, p2);
	}
};

struct BearClassSwap
{
	template < class P>
	void operator()(P&p1, P&p2)
	{
		p1.swap(p2);
	}
};


struct BearLess
{
	template<typename C,typename M>
	bool operator() (const C&c1, const M&c2)const
	{
		return c1 < c2;
	}
};
typedef BearLess BearDefaultCompare;
struct BearGreater
{
	template<typename C, typename M>
	bool operator() (const C&c1, const M&c2)const
	{
		return c1 > c2;
	}
};
struct BearEqual
{
	template<typename C, typename M>
	bool operator()(const C&c1, const M&c2)const
	{
		return c1 == c2;
	}
};

