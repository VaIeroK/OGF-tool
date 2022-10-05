#pragma once

	
class BEARTOOL_API  BearEncoding
{
	BEAR_CLASS_STATIC(BearEncoding);
public:
	enum Encoding
	{
		ANSI,
		UTF8,
		UTF16
	};

	enum Lang
	{
		E_RUS,
		E_ENG,
	};
	static void SetLangure(Lang lang);

	static inline BearStringUnicode FastToUnicode(const bchar8*text)
	{
		return FastToUnicode(text, text + BearString::GetSize(text));
	}
	static inline BearStringUnicode FastToUnicode(const bchar8*begin, const bchar8*end)
	{
		BearStringUnicode out;
		out.reserve(end - begin);
		for (; begin != end; begin++)
		{
			bchar16 r = static_cast<bchar16>(*begin);
			if (static_cast<char>(r) != *begin)
			{
				out.append(1, L'?');
			}
			else
			{
				out.append(1, r);
			}
		}
		return out;
	}

	static inline BearStringAnsi FastToAnsi(const bchar16*text)
	{
		return FastToAnsi(text, text + BearString::GetSize(text));
	}
	static inline BearStringAnsi FastToAnsi(const bchar16*begin, const bchar16*end)
	{
		BearStringAnsi out;
		out.reserve(end - begin);
		for (; begin != end; begin++)
		{
			bchar8 r = static_cast<bchar8>(*begin);
			if (static_cast<char>(r) != *begin)
			{
				out.append(1, L'?');
			}
			else
			{
				out.append(1, r);
			}
		}
		return out;
	}

	static inline  BearStringAnsi ToAnsi(const bchar16*text)
	{
		return ToAnsi(text, text + BearString::GetSize(text));
	}
	static BearStringAnsi ToAnsi(const bchar16*begin, const bchar16*end);

	static inline  BearStringUnicode ToUTF16(const bchar8*text)
	{
		return ToUTF16(text, text + BearString::GetSize(text));
	}
	static BearStringUnicode ToUTF16(const bchar8*begin, const bchar8*end);



	static inline  BearStringUnicode ConvertUTF8InUTF16(const bchar8*text)
	{
		return ConvertUTF8InUTF16(text, text + BearString::GetSize(text));
	}
	static BearStringUnicode ConvertUTF8InUTF16(const bchar8*begin, const bchar8*end);
	static inline  BearStringAnsi ConvertUTF16InUTF8(const bchar16*text)
	{
		return ConvertUTF16InUTF8(text, text + BearString::GetSize(text));
	}
	static BearStringAnsi ConvertUTF16InUTF8(const bchar16*begin, const bchar16*end);

	static bchar8 ToAnsi(bchar16 c);
	static bchar16 ToUTF16(bchar8 c);


	inline static bchar8 ToAnsi(bchar8 c)
	{
		return c;
	}

	static bchar16 ToUTF16(bchar16 c)
	{
		return c;
	}



	template<typename C>
	static inline  bchar ToCurrent(C text)
	{
#ifdef UNICODE
		return ToUTF16(text);
#else
		return ToAnsi(text);
#endif
	}
};