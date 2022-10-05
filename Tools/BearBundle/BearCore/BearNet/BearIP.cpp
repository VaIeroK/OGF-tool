#include "BearCore.hpp"
BearIP::BearIP(uint8 byte0, uint8 byte1, uint8 byte2, uint8 byte3)
{
	m_IP = htonl((byte0 << 24) | (byte1 << 16) | (byte2 << 8) | byte3);
}
BearIP::BearIP(uint32 adress)
{
	m_IP = htonl(adress);;
}
BearIP::BearIP(const bchar* str)
{
	BEAR_VERIFY(Assign(str));
}
bool BearIP::Assign(const bchar* str)
{
	if (BearString::Compare(str, TEXT("localhost")) == 0)
	{
		m_IP = htonl((127 << 24) | (0 << 16) | (0 << 8) | 1);
		return true;
	}
	uint8 byte0 = 127; uint8 byte1 = 0; uint8 byte2 = 0; uint8 byte3 = 1;
	if(BearString::Scanf(str, TEXT("%d,%d,%d,%d"), byte0, byte1, byte2, byte3) == 4)
	{
		m_IP = htonl((byte0 << 24) | (byte1 << 16) | (byte2 << 8) | byte3);
		return true;
	}
	return false;
}
uint32 BearIP::ToUint() const
{
	return ntohl(m_IP);
}
void BearIP::ToString(BearStringConteniar& str) const
{
	in_addr Addr;
	Addr.s_addr = m_IP;
	char* OutStr=inet_ntoa(Addr);
#ifdef UNICODE
	str = *BearEncoding::FastToUnicode(OutStr);
#else

	str = OutStr;
#endif
}
