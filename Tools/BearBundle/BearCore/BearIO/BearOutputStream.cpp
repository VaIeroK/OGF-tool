#include "BearCore.hpp"
#define WRITENUMBER WriteBuffer(&data,sizeof(data)); 
BearOutputStream::BearOutputStream():m_tell_Chunk(-1)
{
}
BearOutputStream::~BearOutputStream()
{
}
void BearOutputStream::WriteUint8(uint8 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteUint16(uint16 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteUint32(uint32 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteUint64(uint64 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteInt8(int8 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteInt16(int16 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteInt32(int32 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteInt64(int64 data)
{
	WRITENUMBER
}

void BearOutputStream::WriteFloat(float data)
{
	WRITENUMBER
}

void BearOutputStream::WriteDouble(double data)
{
	WRITENUMBER
}



void BearOutputStream::BeginChunk(int32 id)
{
	if (Tell() < 0)return;
	WriteInt32(id);
	m_tell_Chunk = Tell();
	if (m_tell_Chunk < -1)return;
	WriteInt32(0);
}

void BearOutputStream::EndChunk()
{
	BEAR_CHECK(m_tell_Chunk >= 0);
	if (Tell() < 0)return;
	int64 size = Tell() - m_tell_Chunk;
	Seek(static_cast<bsize>(m_tell_Chunk));
	WriteInt32(static_cast<int32>(size));
	Seek(static_cast<bsize>(Tell() + m_tell_Chunk - 4));
	m_tell_Chunk = -1;
}

void BearOutputStream::WriteString(const BearString & str, BearEncoding::Encoding encoding, bool info)
{
	if (info)
	{
		uint8 type_info = 0b10000001;
		switch (encoding)
		{
		case BearEncoding::ANSI:
			type_info |= 0b10;
			WriteUint8(type_info);
			WriteUint32(static_cast<uint32>(str.size()));
			break;
		case BearEncoding::UTF8:
			type_info |= 0b100;
			WriteUint8(type_info);
			break;
		case BearEncoding::UTF16:
			type_info |= 0b1000;
			WriteUint8(type_info);
			WriteUint32(static_cast<uint32>(str.size())*2);
			break;
		default:
			return;
		}

	}

	switch (encoding)
	{
	case BearEncoding::ANSI:
	{
#ifdef UNICODE
		WriteBuffer((void*)*BearEncoding::ToAnsi(*str), str.size());
#else
		WriteBuffer((void*)*str, str.size());
#endif
	}
	break;
	case BearEncoding::UTF8:
	{

#ifndef UNICODE
		BearStringUnicode str_;
		str_ = BearEncoding::ToUTF16(*str, *str+str.size());
#endif
		BearStringAnsi utf8 = BearEncoding::ConvertUTF16InUTF8(*
#ifdef UNICODE
			str
#else
			str_
#endif
		);
		WriteUint32(static_cast<uint32>(utf8.size()));
		WriteBuffer((void*)*utf8, utf8.size());

	}
	break;
	case BearEncoding::UTF16:
	{
#ifndef UNICODE
		WriteBuffer((void*)*BearEncoding::ToUTF16(*str), str.size()*2);
#else
		WriteBuffer((void*)(*str), str.size()*2);
#endif
	}
	break;
	default:
		break;
	};
}

void BearOutputStream::WriteStringWithZero(const BearString & str, BearEncoding::Encoding encoding)
{
	WriteString(str, encoding, false);
	switch (encoding)
	{
	case BearEncoding::ANSI:
	case BearEncoding::UTF8:
		WriteInt8((int8)0);	
		break;
	case BearEncoding::UTF16:
		WriteInt16((int16)0);
		break;
	default:
		break;
	}
}

void BearOutputStream::WriteStringAndNewLine(const BearString & str, BearEncoding::Encoding encoding)
{
	WriteString(str, encoding, false);
	switch (encoding)
	{
	case BearEncoding::ANSI:
	case BearEncoding::UTF8:
		WriteInt8((int8)'\r');		WriteInt8((int8)'\n');
		break;
	case BearEncoding::UTF16:
		WriteInt16((int16)L'\r');		WriteInt16((int16)L'\n');
		break;
	default:
		break;
	}
}



