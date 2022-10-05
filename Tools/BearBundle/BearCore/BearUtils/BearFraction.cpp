#include "BearCore.hpp"
BearFraction::BearFraction():m_c(0),m_d(0)
{
}

BearFraction::BearFraction(BearFraction && right) : m_c(0), m_d(0)
{
	Swap(right);
}

BearFraction::BearFraction(const BearFraction & right) : m_c(0), m_d(0)
{
	Copy(right);
}

void BearFraction::Copy(const BearFraction & right)
{
	m_c = right.m_c;
	m_d = right.m_d;
}

void BearFraction::Swap(BearFraction & right)
{
	bear_swap(m_c, right.m_c);
	bear_swap(m_d, right.m_d);
}

BearFraction::BearFraction(uint64 c, uint64 b):m_c(c),m_d(b)
{
}

uint64 BearFraction::Div()
{
	if (m_d == 0)return 0;
	uint64 a = m_c / m_d;
    return a;
}

bool BearFraction::IsRest()
{
	return m_c%m_d;
}

BearFraction & BearFraction::operator=(const BearFraction & right)
{
	Copy(right);
	return*this;
}

BearFraction & BearFraction::operator=(BearFraction && right)
{
	Swap(right);
	return*this;
}

BearFraction& BearFraction::operator*=(const BearFraction& a)
{
	if (!m_d || !a.m_d || !m_c || !a.m_c) return*this;
	m_c *= a.m_c;
	m_d *= a.m_d;
	return *this;
}

BearFraction& BearFraction::operator/=(const BearFraction& a)
{
	if (!m_d||!a.m_d||!m_c||!a.m_c) return*this;
	m_c *= a.m_d;
	m_d *= a.m_c;
	return *this;
}

BearFraction& BearFraction::operator+=(const BearFraction& a)
{
	if (m_d == 0 && a.m_d == 0)
		return*this;
	if (m_d == 0)
		m_d = a.m_d;
	if (m_d != a.m_d) {
		m_c *= a.m_d;
		a.m_c *= m_d;
		m_d *= a.m_d;
		a.m_d = m_d;

	}
	m_c += a.m_c;
	return *this;
}

BearFraction &BearFraction::operator-=(const BearFraction& a)
{
	if (m_d == 0 && a.m_d == 0)
		return*this;
	if (m_d == 0)
		m_d = a.m_d;
	if (m_d != a.m_d) {
		m_c *= a.m_d;
		a.m_c *= m_d;
		m_d *= a.m_d;
		a.m_d = m_d;

	}
	m_c -= a.m_c;
	return *this;
}

BearFraction& BearFraction::operator+=(uint64 a)
{
	m_c += a;
	return*this;
}

BearFraction BearFraction::operator*(uint64 a)
{
	BearFraction b(*this);
	b.m_c *= a;
	return b;
}

BEARTOOL_API BearFraction operator*(const BearFraction & c, const BearFraction & a)
{
	BearFraction b(c);
	b *= a;
	return b;
}

BEARTOOL_API BearFraction operator/(const BearFraction & c, const BearFraction & a)
{
	BearFraction b(c);
	b /= a;
	return b;
}

BEARTOOL_API BearFraction operator+(const BearFraction & c, const BearFraction & a)
{
	BearFraction b(c);
	b += a;
	return b;
}

BEARTOOL_API BearFraction operator-(const BearFraction & c, const BearFraction & a)
{
	BearFraction b(c);
	b -= a;
	return b;
}
