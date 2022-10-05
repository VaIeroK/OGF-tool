#include "BearCore.hpp"
void BearBufferedReader::ReadStringToLine(BearString &str, BearEncoding::Encoding encoding) const
{
	if (Tell() == 0 && Size() > 3)
		if (((char *)Begin())[0] == 0xef && ((char *)Begin())[1] == 0xbb && ((char *)Begin())[2] == 0xbf)
			Seek(3);
	str.seek(0);
	str.clear_no_free();
	switch (encoding)
	{
	case BearEncoding::ANSI:
	{
		const bchar8 *Begin1 = reinterpret_cast<const bchar8 *>(Begin());
		const bchar8 *End1 = reinterpret_cast<const bchar8 *>(End());
		const bchar8 *Begin2 = strchr(Begin1, '\r');
		if (!Begin2)
			Begin2 = strchr(Begin1, '\n');

		if (!Begin2 || Begin2 > End1)
		{
			Begin2 = End1;
		}
#ifdef UNICODE
		str.append(*BearEncoding::ToUTF16(Begin1, Begin2), Begin2 - Begin1);
#else
		str.append(Begin1, Begin2 - Begin1);
#endif

		if (Begin2[0] == '\r' && Begin2[1] == '\n')
			Seek(Begin2 - Begin1 + Tell() + 2);
		else
			Seek(Begin2 - Begin1 + Tell() + 1);
	}
	break;
	case BearEncoding::UTF8:
	{
		const bchar8 *Begin1 = reinterpret_cast<const bchar8 *>(Begin());
		const bchar8*End1 = reinterpret_cast<const bchar8*>(End());
		const bchar8*Begin2 = reinterpret_cast<const bchar8*>(strchr(reinterpret_cast<const bchar8 *>(Begin1), '\r'));
		if (!Begin2)
			Begin2 = reinterpret_cast<const bchar8*>(strchr(reinterpret_cast<const bchar8 *>(Begin1), '\n'));
		if (!Begin2 || Begin2 > End1)
		{
			Begin2 = End1;
		}
#ifdef UNICODE
		str = BearEncoding::ConvertUTF8InUTF16(Begin1, Begin2);
#else
		auto str_ = BearEncoding::ConvertUTF8InUTF16(Begin1, Begin2);
		str.append(*BearEncoding::ToAnsi(*str_));
#endif

		if (Begin2[0] == '\r' && Begin2[1] == '\n')
			Seek(Begin2 - Begin1 + Tell() + 2);
		else
			Seek(Begin2 - Begin1 + Tell() + 1);

		break;
	}
	case BearEncoding::UTF16:
	{
		const bchar16 *Begin1 = reinterpret_cast<const bchar16 *>(Begin());
		const bchar16 *End1 = reinterpret_cast<const bchar16 *>(End());
		const bchar16 *Begin2 = wcschr(Begin1, L'\r');
		if (!Begin2)
			Begin2 = wcschr(Begin1, L'\n');
		if (!Begin2 || Begin2 > End1)
		{
			Begin2 = End1;
		}

#ifdef UNICODE
		str.append(Begin1, Begin2 - Begin1);
#else
		str.append(*BearEncoding::ToAnsi(Begin1, Begin2), (Begin2 - Begin1));
#endif

		if (Begin2[0] == L'\r' && Begin2[1] == L'\n')
			Seek(Begin2 - Begin1 + Tell() + 2);
		else
			Seek(Begin2 - Begin1 + Tell() + 1);

		break;
	}
	default:
		break;
	}
}

void BearBufferedReader::ReadStringToZero(BearString &str, BearEncoding::Encoding encoding) const
{
	switch (encoding)
	{
	case BearEncoding::ANSI:
	{
		const bchar8 *Begin1 = reinterpret_cast<const bchar8 *>(Begin());
		const bchar8 *End1 = reinterpret_cast<const bchar8 *>(End());
		const bchar8 *Begin2 = strchr(Begin1, '\0');
		if (!Begin2)
		{
			Begin2 = End1;
		}
#ifdef UNICODE
		str.append(*BearEncoding::ToUTF16(Begin1, Begin2), Begin2 - Begin1);
#else
		str.append(Begin1, Begin2 - Begin1);
#endif

		Seek(Begin2 - Begin1 + Tell() + 1);
	}
	break;
	case BearEncoding::UTF8:
	{
		const bchar8*Begin1 = reinterpret_cast<const bchar8*>(Begin());
		const bchar8*End1 = reinterpret_cast<const bchar8*>(End());
		const bchar8*Begin2 = reinterpret_cast<const bchar8 *>(strchr(reinterpret_cast<const bchar8 *>(Begin1), '\0'));
		if (!Begin2)
		{
			Begin2 = End1;
		}
#ifdef UNICODE
		str = BearEncoding::ConvertUTF8InUTF16(Begin1, Begin2);
#else
		auto str_ = BearEncoding::ConvertUTF8InUTF16(Begin1, Begin2);
		str.append(*BearEncoding::ToAnsi(*str_));
#endif

		Seek(Begin2 - Begin1 + Tell() + 1);

		break;
	}
	case BearEncoding::UTF16:
	{
		const bchar16 *Begin1 = reinterpret_cast<const bchar16 *>(Begin());

		const bchar16 *Begin2 = wcschr(Begin1, L'\0');

#ifdef UNICODE
		str.append(Begin1, Begin2 - Begin1);
#else
		str.append(*BearEncoding::ToAnsi(Begin1, Begin2), (Begin2 - Begin1));
#endif

		Seek(Begin2 - Begin1 + Tell() + 1);
		break;
	}
	default:
		break;
	}
}

BearRef<BearInputStream> BearBufferedReader::ReadChunkAsInputStream(uint32 type) const
{
	auto ptr = ReadChunkAsBufferedReader(type);
	auto strem = *ptr;
	ptr.clear_no_free();
	return BearRef<BearInputStream>(strem);
}
