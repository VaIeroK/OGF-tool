#pragma once
class BEARTOOL_API BearIP
{
public:
	BearIP() { m_IP = 0; }
	BearIP(const BearIP& right) { Copy(right); }
	BearIP(BearIP&& right) { Swap(right); }
	inline BearIP& operator = (const BearIP& right) { Copy(right); return*this; }
	inline BearIP& operator = (BearIP&& right) { Swap(right); return*this; }
	inline void Swap(BearIP& right) { bear_swap(m_IP, right.m_IP); }
	inline void Copy(const BearIP& right) { m_IP = right.m_IP; }

	BearIP(uint8 byte0, uint8 byte1, uint8 byte2, uint8 byte3);
	BearIP(uint32 adress);
	BearIP(const bchar* str);
	bool Assign(const bchar* str);
	uint32 ToUint() const;
	void ToString(BearStringConteniar& str) const;
private:
	uint32 m_IP;
};
