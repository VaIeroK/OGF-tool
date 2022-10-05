#pragma once
#ifdef WINDOWS
#undef GetCurrentTime
#endif
class  BearTime
{
public:
	inline BearTime(const BearTime&right);
	inline BearTime( BearTime&&right);

	inline BearTime();
	inline BearTime(int64 microseconds);
	inline BearTime(int32 miliseconds);
	inline BearTime(float seconds);

	inline 		void swap(BearTime&right);
	inline 		void copy(const BearTime&right);

	inline 	void assign(int64 microseconds);
	inline 	void assign(int32 miliseconds);
	inline 	void assign(float seconds);
	inline 	void assign(const BearTime &time);
	inline 	void append(int64 microseconds);
	inline 	void append(int32 miliseconds);
	inline 	void append(float seconds);
	inline 	void append(const BearTime& time);
	inline 	int64 asmicroseconds() const;
	inline 	int32 asmiliseconds() const;
	inline 	float asseconds() const;
	inline 	BearTime& operator-=(const BearTime &right);
	inline 	BearTime& operator+=(const BearTime &right);
	inline 	BearTime& operator-=(BearTime &&right);
	inline 	BearTime& operator+=(BearTime &&right);
	inline 	BearTime& operator*=(const BearTime &right);
	inline 	BearTime& operator/=(const BearTime &right);
	inline 	BearTime& operator*=(BearTime &&right);
	inline 	BearTime& operator*(double right);
	inline 	BearTime& operator/=(BearTime &&right);

	inline 		BearTime &operator=(const BearTime &right);
	inline 		BearTime &operator=( BearTime &&right);
private:
	int64 m_microseconds;
};
inline  BearTime operator-(const BearTime &left, const BearTime &right);
inline 	 BearTime operator+(const BearTime &left, const BearTime &right);
inline 	 BearTime operator-(const BearTime &left, BearTime &&right);
inline 	 BearTime operator + (const BearTime &left, BearTime &&right);
inline  BearTime operator*(const BearTime &left, const BearTime &right);
inline 	 BearTime operator/(const BearTime &left, const BearTime &right);
inline 	 BearTime operator*(const BearTime &left, BearTime &&right);
inline 	 BearTime operator / (const BearTime &left, BearTime &&right);

#include "BearTime_inline.h"
