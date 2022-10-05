#pragma once

class BEARTOOL_API BearCheckSum
{
public:
	BEAR_CLASS_STATIC(BearCheckSum)
	static void SHA256(void*data, bsize size, uint8 sha[]);
	static uint32 CRC32(const void*data, bsize size);
	static uint32 CRC32(uint32 send,const void*data, bsize size);
};