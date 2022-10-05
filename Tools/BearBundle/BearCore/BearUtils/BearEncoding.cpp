#include "BearCore.hpp"
#include <codecvt>
#include <string>
#include <locale>

template <typename I, typename E, typename S>
struct codecvt : public std::codecvt<I, E, S>
{
	~codecvt()
	{
	}
};
template <typename I, typename E, typename S>
struct codecvt_byname : public std::codecvt_byname<I, E, S>
{
	codecvt_byname(const char *l) : std::codecvt_byname<I, E, S>(l) {}
	~codecvt_byname()
	{
	}
};
#ifdef MSVC
#pragma warning(disable:4996)
#endif
#ifdef MSVC
std::wstring_convert<::codecvt_byname<bchar16, bchar8, std::mbstate_t>, bchar16> *ConverterUTF16;
#endif
std::wstring_convert<std::codecvt_utf8_utf16<bchar16>, bchar16> *ConverterUTF8;

#if defined(WINDOWS) || defined(UNIX)
static uint32 LAnsiCode = 1251; /*RUS*/
;
#endif

struct Initializer
{
	Initializer() {}
	inline void Initialize(const char *lg = "en_US.UTF-8")
	{
#ifdef MSVC
		if (ConverterUTF8)
		{
			return;
		}
#endif
		ReInitialize(lg);
	}
	inline void ReInitialize(const char *lg = "en_US.UTF-8")
	{

		if (ConverterUTF8)
		{
			#ifdef MSVC
			delete[] ConverterUTF16;
			#endif
			delete[] ConverterUTF8;
		}
	#ifdef MSVC
		ConverterUTF16 = new std::wstring_convert<::codecvt_byname<bchar16, bchar8, std::mbstate_t>, bchar16>(new ::codecvt_byname<bchar16, bchar8, std::mbstate_t>(lg));
		#endif
		ConverterUTF8 = new std::wstring_convert<std::codecvt_utf8_utf16<bchar16>, bchar16>;
	}
	~Initializer()
	{
		#ifdef MSVC
		delete[] ConverterUTF16;
		#endif
		delete[] ConverterUTF8;
	}
};
Initializer initializer;

inline bchar8 ToANSI(bchar16 c)
{

	if ((bchar16)(bchar8)c != c)
	{  
#ifndef MSVC
		if (LAnsiCode == 1251)
		{ 
			if (c >= L'А' && c <= L'Я'+32)
				return ((bchar8)(c - L'А')) + 0xC0;
			if (c >= L'а' && c <= L'я'+32)
				return ((bchar8)(c - L'а')) + 0xE0;
			if (c == L'ё')
				return 0xB8;
			if (c ==L'Ё' )
				return 0xA8;
		}
		return L'?';
#else
		initializer.Initialize();
		return ConverterUTF16->to_bytes(c)[0];
#endif
	}
	return (bchar8)c;
}

inline bchar16 ToUTF16(bchar8 c_)
{
	uint8 c=c_;
	if (c>=0x80)
	{
#ifdef WINDOWS
		bchar16 res[2]; 
		MultiByteToWideChar(LAnsiCode, 0, &c_, 2, res, 2);
		return res[0];
#else
		if (LAnsiCode == 1251)
		{
			if (c >= 0xC0 && c <= 0xDF)
				return ((bchar16)(c - 0xC0)) + L'А';
			if (c >= 0xE0 && c <= 0xFF)
				return ((bchar16)(c - 0xE0)) + L'а';
			if (c == 0xB8)
				return L'ё';
			if (c == 0xA8)
				return L'Ё';
		}
#endif
	}
	return (bchar16)c;
}
BearStringAnsi BearEncoding::ToAnsi(const bchar16* begin, const bchar16* end)
{
	BearStringAnsi out;
	out.resize(end - begin, ' ');
	bsize i = 0;
	while (begin != end)
	{
		out[i++] = ::ToANSI(*(begin++));
	}
	return out;
}



BearStringUnicode BearEncoding::ToUTF16(const bchar8 *begin, const bchar8 *end)
{
	BearStringUnicode out;
	out.resize(end - begin, L' ');
	bsize i = 0;
	while (begin != end)
	{
		out[i++] = ::ToUTF16(*(begin++));
	}
	return out;

}


BearStringUnicode BearEncoding::ConvertUTF8InUTF16(const bchar8*text, const bchar8*end)
{
	initializer.Initialize();
	BearStringUnicode str = ConverterUTF8->from_bytes(reinterpret_cast<const char*>(text), reinterpret_cast<const char*>(end)).c_str();
	return str;
}


BearStringAnsi BearEncoding::ConvertUTF16InUTF8(const bchar16 *text, const bchar16 *end)
{
	initializer.Initialize();
	BearStringAnsi str = ConverterUTF8->to_bytes(text, end).c_str();
	return str;
}

bchar8 BearEncoding::ToAnsi(bchar16 c)
{
	return ::ToANSI(c);
}

bchar16 BearEncoding::ToUTF16(bchar8 c)
{
	return ::ToUTF16(c);
}

void BearEncoding::SetLangure(Lang lang)
{
#if defined(WINDOWS) || defined(UNIX)
	switch (lang)
	{
	case BearEncoding::E_RUS:
		LAnsiCode = 1251;
		break;
	case BearEncoding::E_ENG:
		LAnsiCode = 0;
		break;
	default:
		LAnsiCode = 0;
		break;
	} 
#endif
	switch (lang)
	{
	case BearEncoding::E_RUS:
		setlocale(LC_CTYPE, "ru_RU.UTF-8");
#ifdef WINDOWS
		initializer.ReInitialize("Russian");
#else
		initializer.ReInitialize("ru_RU.UTF-8");
#endif

		break;
	case BearEncoding::E_ENG:
	default:
		setlocale(LC_CTYPE, "en_US.UTF-8");
		initializer.ReInitialize("en_US.UTF-8");
		break;
	}
	{
	}
	setlocale(LC_NUMERIC, "en_US.UTF-8");
}
