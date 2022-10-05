#include "BearCore.hpp"
#define READT(type) type t=0; ReadBuffer(&t,sizeof(t)); return t;
BearInputStream::BearInputStream():m_last_pos(bsize(-1))
{
}
BearInputStream::~BearInputStream()
{
}
uint8 BearInputStream::ReadUint8()const
{
	READT(uint8);
}

uint16 BearInputStream::ReadUint16()const
{
	READT(uint16);
}

uint32 BearInputStream::ReadUint32()const
{
	READT(uint32);
}

uint64 BearInputStream::ReadUint64()const
{
	READT(uint64);
}

int8 BearInputStream::ReadInt8()const
{
	READT(int8);
}

int16 BearInputStream::ReadInt16()const
{
	READT(int16);
}

int32 BearInputStream::ReadInt32()const
{
	READT(int32);
}

int64 BearInputStream::ReadInt64()const
{
	READT(int64);
}

float BearInputStream::ReadFloat() const
{
	READT(float);
}

double BearInputStream::ReadDouble() const
{
	READT(double);
}



BearRef<uint8> BearInputStream::Read() const
{
	return Read(Size() - Tell());
}

BearRef<uint8> BearInputStream::Read(bsize size) const
{
	uint8*data = bear_alloc<uint8>(static_cast<bsize>(size));
	ReadBuffer(data, static_cast<bsize>(size));
	return BearRef<uint8>(data);
}

void BearInputStream::ReadString(BearString & str) const
{
	int64 t = Tell();
	if (t < 0)return;
	uint8 flags = ReadUint8();
	if (!(flags & uint8(0b10000001)))
		return;
	bool ansi = (flags & 0b10);
	bool utf8 = (flags & 0b100);
	bool utf16 = (flags & 0b1000);
	if (ansi + utf8 + utf16 != 1)return;
	uint32 size = ReadUint32();
	BearEncoding::Encoding type= BearEncoding::Encoding::ANSI;
	if (utf8)type = BearEncoding::Encoding::UTF8;
	if (utf16)type = BearEncoding::Encoding::UTF16;
	ReadString(str, size, type);
}


void BearInputStream::ReadString(BearString & str, bsize size, BearEncoding::Encoding type) const
{
	BearRef<uint8> text(bear_alloc<uint8>(size));
	ReadBuffer(*text, size);

	if (type == BearEncoding::UTF8)
	{
		auto te = BearEncoding::ConvertUTF8InUTF16(reinterpret_cast<const bchar8*>(*text), reinterpret_cast<const bchar8*>(*text) + size);
#ifdef UNICODE
		str.assign(*te);
#else
		str = BearEncoding::ToAnsi(*te);
#endif
	}
	else if (type == BearEncoding::ANSI)
	{
#ifdef UNICODE
		auto te = BearEncoding::ToUTF16(reinterpret_cast<const bchar8*>(*text), reinterpret_cast<const bchar8*>(*text+size));
		str.append(*te);
#else 
		str.append((bchar8*)*text,size);
#endif
	
	}
	else if (type == BearEncoding::UTF16)
	{
#ifdef UNICODE
		str.append((bchar16*)*text, size/2);
#elif UTF_8
		auto te = BearEncoding::ToAnsi(reinterpret_cast<const bchar16*>(*text), reinterpret_cast<const bchar16*>(*text + size));
		str.append(*te);
#endif
	}
	return;
}

void BearInputStream::ToString(BearString & str, BearEncoding::Encoding type) const
{
	if (Tell() == 0)
	{
		uint8 utf_bod[3];
		ReadBuffer(utf_bod, 3);
		if (utf_bod[0] == 0xef && utf_bod[1] == 0xbb && utf_bod[2] == 0xbf)
		{
			type = BearEncoding::UTF8;
			goto jump_01;
		}
		Seek(0);
	}
jump_01:
	ReadString(str, static_cast<bsize>(Size() - Tell()), type);
}

bsize BearInputStream::GoToChunk(uint32 type_) const
{
	if (m_last_pos != bsize(-1))
	{
		Seek(m_last_pos);
		uint32 type = ReadUint32();
		uint32 size_ = ReadUint32();
		BEAR_ASSERT((size_ + Tell()) <= Size());
		if (type_ == type)
			return size_;
	}
	Seek(0);
	while (!Eof())
	{
		m_last_pos = Tell();
		uint32 type = ReadUint32();
		uint32 size_ = ReadUint32();
		if ((size_ + Tell()) > Size())
		{
			m_last_pos =bsize( -1);
			return 0;
		}
		if (type_ == type)
			return size_;
		Seek(static_cast<bsize>(Tell() + size_));
	}
	m_last_pos = bsize(-1);
	return 0;
}
