#pragma once

template<class cl> class BearVector2
{
public:
	BearVector2():x(0),y(0) {};


	BearVector2(const BearVector2& right) :x(0), y(0) { copy(right); };
	BearVector2( BearVector2&& right) :x(0), y(0) { swap(right); };


	void copy(const BearVector2& right)
	{
		x = right.x;
		y = right.y;
	}
	void swap( BearVector2& right)
	{
		bear_swap(x, right.x);
		bear_swap(y, right.y);
	}
	inline BearVector2 &operator=(const BearVector2& right)
	{
		copy(right);
		return *this;
	}
	inline BearVector2 &operator=(BearVector2&& right)
	{
		swap(right);
		return *this;
	}

	BearVector2(cl cl1, cl cl2)
	{
		set( cl1,  cl2);
	}
	inline	void set(cl cl1, cl cl2)
	{
		x = cl1; y = cl2;
	}
	inline bool operator==(const BearVector2& right) const
	{
		return x == right.x
		&&y == right.y;
	}
	inline bool operator!=(const BearVector2& right) const
	{
		return x != right.x
			||y!= right.y;
	}
	inline BearVector2& operator+=(const BearVector2& right)
	{
		x += right.x;
		y += right.y;
		return *this;
	}
	inline BearVector2 operator+(const BearVector2& right) const
	{
		BearVector2 result(*this);
		result.x += right.x;
		result.y += right.y;
		return result;
	}
	inline BearVector2 &operator-=(const BearVector2 &right)
	{
		x -= right.x;
		y -= right.y;
		return *this;
	}
	inline BearVector2 operator-(const BearVector2& right) const
	{
		BearVector2 result(*this);
		result.x -= right.x;
		result.y -= right.y;
		return result;
	}
	inline	BearVector2& operator*=(const BearVector2 &right)
	{
		x=x *right.x;
		y=y * right.y;
		return *this;
	}
	inline BearVector2 operator*(const BearVector2& right) const
	{
		BearVector2 result(*this);
		result.x *= right.x;
		result.y *= right.y;
		return result;
	}
	inline BearVector2 &operator/=(const BearVector2& right)
	{
		x=x / right.x;
		y=y / right.y;
		return *this;
	}
	inline BearVector2 operator/(const BearVector2& right) const
	{
		BearVector2 result(*this);
		result.x /= right.x;
		result.y /= right.y;
		return result;
	}
	union
	{
		struct
		{
			cl x;
			cl y;
		};
		struct
		{
			cl array[2];
		};
	};
	inline BearVector2  &getCenter() const
	{
		return BearVector2((x ) / 2, (y) / 2);
	}
};
typedef BearVector2<float> BearFVector2;
typedef BearVector2<int32> BearIVector2;


