#pragma once
#ifdef min
#undef min
#endif 
#ifdef max
#undef max
#endif
class BearMath
{
	BEAR_CLASS_STATIC(BearMath);
public:
	constexpr static float PI = 3.141592653589793238463f;
	inline static float cos(float a)
	{
		return ::cosf(a);
	}

	inline static float sin(float a)
	{
		return ::sinf(a);
	}
	inline static float tan(float a)
	{
		return ::tanf(a);
	}
	inline static float sqrt(float a)
	{
		return ::sqrtf(a);
	}
	inline static float qrt(float a)
	{
		return a * a;
	}
	inline static float round(float a)
	{
		return ::roundf(a);
	}

	inline static float floor(float a)
	{
		return ::floorf(a);
	}

	template<typename T>
	inline static T abs(T a)
	{
		return a<0?-a:a;
	}
	inline static float abs(float a)
	{
		return ::fabsf(a);
	}
	template<typename A>
	inline static A max(A a,A b)
	{
		if(a>b)
			return a;
		return b; 
	}
	template<typename A>
	inline static A min(A a,A b)
	{
		if(a>b)
			return b;
		return a; 
	}
#ifdef MSVC
	template<typename T>
	inline static bool isnan( T a)
	{
		return ::isnan(a);
	}
	inline static bool isvalid(float x)
	{
		const int cls = _fpclass(static_cast<double>(x));
		if (cls & (_FPCLASS_SNAN + _FPCLASS_QNAN + _FPCLASS_NINF + _FPCLASS_PINF + _FPCLASS_ND + _FPCLASS_PD))
			return false;
		return true;
	}
#else
	template<typename T>
	inline static bool isnan( T a)
	{
		return a==std::numeric_limits<T>::quiet_NaN();
	}
	template<typename T>
	inline static bool isvalid(T x)
	{
		if(isnan(x))return false;
		const int cls = std::fpclassify(x);
		switch (cls)
		{
		case FP_INFINITE:
			[[fallthrough]];
		case FP_SUBNORMAL:
			return false;
		default:
			break;
		}
		return true;
	}
	
#endif

	template <typename T>
	inline static T clamp(const T val, const T _low, const T _high) {
		if (val < _low) return _low;
		else if (val > _high)
			return _high;
		return val;
	};
};