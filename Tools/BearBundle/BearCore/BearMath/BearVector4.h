#pragma once

template<class cl> class BearVector4
{
public:
	BearVector4() :x(0), y(0),x1(0),y1(0) {};
	BearVector4(const BearVector4&right) { copy(right); };
	BearVector4(BearVector4&&right) { swap(right); };
	BearVector4(cl x, cl y, cl x1, cl y1)
	{
		set(x, y, x1, y1);
	}
	inline void set(cl x_, cl y_,  cl x1_, cl y1_)
	{
		x = x_; y = y_; x1 = x1_; y1 = y1_;
	}
	void swap(BearVector4 &right)
	{
		bear_swap(x, right.x);
		bear_swap(y, right.y);
		bear_swap(x1, right.x1);
		bear_swap(y1, right.y1);
	}
	void copy(const BearVector4 &right)
	{
		x = right.x;
		y = right.y;
		x1 = right.x1;
		y1 = right.y1;
	}
	inline BearVector4 &operator=(const BearVector4& right)
	{
		copy(right);
		return *this;
	}
	inline BearVector4 &operator=( BearVector4&& right)
	{
		swap(right);
		return *this;
	}
	inline BearVector4 &operator+=(const BearVector4& right)
	{
		x += right.x;
		y += right.y;
		x1 += right.x1;
		y1 += right.y1;
		return *this;
	}
	inline BearVector4 &operator-=(const BearVector4& right)
	{
		x -= right.x;
		y -= right.y;
		x1 -= right.x1;
		y1 -= right.y1;
		return *this;
	}
	inline BearVector4 &operator*=(const BearVector4& right)
	{
		x = x *right.x;
		y = y * right.y;
		x1 = x1* right.x1;
		y1 = y1* right.y1;
		return *this;
	}
	inline	BearVector4 &operator/=(const BearVector4& right)
	{
		x = x / right.x;
		y = y / right.y;
		x1 = x1 / right.x1;
		y1 = y1/ right.y1;
		return *this;
	}
	inline BearVector2<cl>& getCenter() const
	{
		return BearVector2<cl>((x-x1)/2,(y-y1)/2);
	}
	inline bool inZone(const BearVector2<cl>&position)const
	{
		if (x <= position.x&&y <= position.y&&x1+x >= position.x&&y1 + y >= position.y)
		{
			return true;
		}
		return false;
	}
	inline bool inZone(const BearVector4<cl>&rect)const
	{
		return x < rect.x + rect.x1 &&
			x + x1 > rect.x &&
			y < rect.y + rect.y1 &&
			y1 + y > rect.y;

	}
	inline cl getW() const
	{
		return x1 - x;
	}
	inline cl getH() const
	{
		return y1 - y;
	}
	cl &operator[](bsize id)
	{
		return Array[id];
	}
	const cl &operator[](bsize id) const
	{
		return Array[id];
	}
	union
	{
		struct
		{
			cl x;
			cl y;
			cl x1;
			cl y1;
		};
		struct
		{
			cl Array[4];
		};
	};
private:

};
