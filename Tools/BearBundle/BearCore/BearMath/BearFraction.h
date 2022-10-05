#pragma once

class BEARTOOL_API BearFraction
{
public:
	BearFraction();
	BearFraction( BearFraction&&right);
	BearFraction(const BearFraction&right);
	void Copy(const BearFraction&right);
	void Swap(BearFraction&right);
	BearFraction &operator=(const BearFraction&right); 
	BearFraction &operator=(BearFraction&&right);

	BearFraction(uint64 c, uint64 b);
	uint64 Div();
	bool IsRest();
	
	BearFraction &operator*=(const BearFraction& a);
	BearFraction& operator/=(const BearFraction& a);
	BearFraction& operator+=(const BearFraction& a);
	BearFraction& operator-=(const BearFraction& a);
	BearFraction& operator+=(uint64 a);
	BearFraction operator*(uint64 a);
private:
	mutable uint64 m_c, m_d;
};
BEARTOOL_API BearFraction operator*(const BearFraction& a,const BearFraction& b);
BEARTOOL_API BearFraction operator/(const BearFraction& a, const BearFraction& b);
BEARTOOL_API BearFraction operator+(const BearFraction& a, const BearFraction& b);
BEARTOOL_API BearFraction operator-(const BearFraction& a, const BearFraction& b);
