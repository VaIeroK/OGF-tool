#pragma once

template<class cl> class BearVector3
{
public:
	BearVector3():x(0),y(0),z(0) {};
	BearVector3(cl x, cl y, cl z)
	{
		set( x,  y,z);
	}
	BearVector3(const BearVector3& right) :x(0), y(0), z(0) { copy(right); };
	BearVector3( BearVector3&& right) :x(0), y(0), z(0) { swap(right); };
	void swap( BearVector3& right)
	{
		bear_swap(x, right.x);
		bear_swap(y, right.y);
		bear_swap(z, right.z);
	}
	void copy(const BearVector3& right)
	{
		x = right.x;
		y = right.y;
		z = right.z;
	}

	inline void set(cl x_, cl y_,  cl z_)
	{
		x = x_; y = y_; z = z_;
	}
	inline void normalize()
	{
		float mag = static_cast<float>(BearMath::sqrt((float)1 / (x*x + y*y + z*z)));
		x *= mag;
		y *= mag;
		z *= mag;
	}
	inline BearVector3& operator+=(const BearVector3& right)
	{
		x += right.x;
		y += right.y;
		z += right.z;
		return *this;
	}
	inline BearVector3& operator=(const BearVector3& right)
	{
		copy(right);
		return *this;
	}
	inline BearVector3& operator=( BearVector3&& right)
	{
		swap(right);
		return *this;
	}
	inline BearVector3& operator-=(const BearVector3& right)
	{
		x -= right.x;
		y -= right.y;
		z -= right.z;
		return *this;
	}
	inline BearVector3& operator*=(const BearVector3& right)
	{
		x=x *right.x;
		y=y * right.y;
		z= z* right.z;
		return *this;
	}
	inline BearVector3& operator/=(const BearVector3& right)
	{
		x=x / right.x;
		y=y / right.y;
		z = z/ right.z;
		return *this;
	}
	union
	{
		struct
		{
			cl x;
			cl y;
			cl z;
		};
		struct
		{
			cl array[3];
		};
	};


};
typedef BearVector3<float> BearFVector;
typedef BearVector3<int32> BearIVector;
