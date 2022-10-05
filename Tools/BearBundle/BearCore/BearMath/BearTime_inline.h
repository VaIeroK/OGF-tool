#pragma once

inline BearTime::BearTime(const BearTime & right)
{
	assign(right);
}
inline BearTime::BearTime( BearTime && right) :m_microseconds(0)
{
	swap(right);
}
inline BearTime::BearTime() :m_microseconds(0)
{
}

inline BearTime::BearTime(int64 microseconds)
{
	assign(microseconds);
}

inline BearTime::BearTime(int32 miliseconds)
{
	assign(miliseconds);
}

inline BearTime::BearTime(float seconds)
{
	assign(seconds);
}

inline void BearTime::swap(BearTime & right)
{
	bear_swap(m_microseconds, right.m_microseconds);
}
inline void BearTime::copy(const BearTime & right)
{
	assign(right);
}

inline void  BearTime::assign(int64 microseconds)
{
	m_microseconds = microseconds;
}

inline void  BearTime::assign(int32 miliseconds)
{
	m_microseconds = int64(miliseconds * 1000);
}

inline void  BearTime::assign(float seconds)
{
	m_microseconds = int64(seconds * 1000000);
}

inline void BearTime::assign(const BearTime &time)
{
	m_microseconds = time.m_microseconds;
}

inline void  BearTime::append(int64 microseconds)
{
	m_microseconds += microseconds;
}

inline void  BearTime::append(int32 miliseconds)
{
	append(int64(miliseconds * 1000));
}

inline void  BearTime::append(float seconds)
{
	append(int64(seconds * 1000000));
}

inline void BearTime::append(const BearTime& time)
{
	append(time.m_microseconds);
}

inline int64  BearTime::asmicroseconds() const
{
	return m_microseconds;
}

inline int32  BearTime::asmiliseconds() const
{
	return static_cast<int32>(m_microseconds / 1000ll);
}

inline float  BearTime::asseconds() const
{
	return m_microseconds / 1000000.f;
}
inline BearTime & BearTime::operator-=(const BearTime & right)
{
	m_microseconds -= right.m_microseconds;
	return *this;
}
inline BearTime & BearTime::operator+=(const BearTime & right)
{
	m_microseconds += right.m_microseconds;
	return *this;
}
inline BearTime & BearTime::operator-=(BearTime && right)
{
	m_microseconds -= right.m_microseconds;
	return *this;
}
inline BearTime & BearTime::operator+=(BearTime && right)
{
	m_microseconds += right.m_microseconds;
	return *this;
}

inline BearTime & BearTime::operator*=(const BearTime & right)
{
	m_microseconds *= right.m_microseconds;
	return *this;
}

inline BearTime & BearTime::operator/=(const BearTime & right)
{
	m_microseconds /= right.m_microseconds;
	return *this;
}

inline BearTime & BearTime::operator*=(BearTime && right)
{
	m_microseconds *= right.m_microseconds;
	return *this;
}

inline BearTime & BearTime::operator*(double right)
{
	 m_microseconds=int64(double(m_microseconds)*right);
	 return *this;
}

inline BearTime & BearTime::operator/=(BearTime && right)
{
	m_microseconds /= right.m_microseconds;
	return *this;
}

inline BearTime  & BearTime::operator=(const BearTime& right)
{
	m_microseconds = right.m_microseconds;
	return*this;
}
inline BearTime  & BearTime::operator=( BearTime&& right)
{
	m_microseconds = right.m_microseconds;
	return*this;
}
inline BearTime operator-(const BearTime & left, const BearTime & right)
{
	return BearTime(left) -= right;
}

inline BearTime operator+(const BearTime & left, const BearTime & right)
{
	return BearTime(left) += right;
}

inline BearTime operator-(const BearTime & left, BearTime && right)
{
	return BearTime(left) -= right;
}

inline BearTime operator+(const BearTime & left, BearTime && right)
{
	return BearTime(left) += right;
}

inline  BearTime operator*(const BearTime & left, const BearTime & right)
{
	return BearTime(left) *= right;
}

inline  BearTime operator/(const BearTime & left, const BearTime & right)
{
	return BearTime(left) /= right;
}

inline  BearTime operator*(const BearTime & left, BearTime && right)
{
	return BearTime(left) *= right;
}

inline BearTime operator/(const BearTime & left, BearTime && right)
{
	return BearTime(left) /= right;
}
