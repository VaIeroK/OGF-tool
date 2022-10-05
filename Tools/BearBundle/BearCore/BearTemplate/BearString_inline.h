
#ifdef GCC
static inline bsize GetSize(const bchar8 *text)
{
	return strlen(text);
}
static inline bsize GetSize(const bchar16 *text)
{
	return wcslen(text);
}
template <bsize sizeBuffer>
static inline void Contact(bchar8(&dst)[sizeBuffer], const bchar8 *src)
{
	strcat(dst, src);
}

static inline void Contact(bchar8 *dst, bsize sizeBuffer, const bchar8 *src)
{
	strcat(dst, src);
}
template <bsize sizeBuffer>
static inline void Contact(bchar16(&dst)[sizeBuffer], const bchar16 *src)
{
	wcscat(dst, src);
}
static inline void Contact(bchar16 *dst, bsize sizeBuffer, const bchar16 *src)
{
	wcscat(dst, src);
}

template <bsize sizeBuffer>
static inline void Copy(bchar16(&dst)[sizeBuffer], const bchar16 *src)
{
	wcscpy(dst, src);
}
static inline void Copy(bchar16 *dst, bsize sizeBuffer, const bchar16 *src)
{
	wcscpy(dst, src);
}
template <bsize sizeBuffer>
static inline void Copy(bchar8(&dst)[sizeBuffer], const bchar8 *src)
{
	strcpy(dst, src);
}
static inline void Copy(bchar8 *dst, bsize sizeBuffer, const bchar8 *src)
{
	strcpy(dst, src);
}

static inline const bchar8 *ToChar(const bchar8 *str, bchar8 ch)
{
	return strchr(str, ch);
}
static inline bchar8 *ToChar(bchar8 *str, bchar8 ch)
{
	return strchr(str, ch);
}
static inline const bchar8 *ToCharWithEnd(const bchar8 *str, bchar8 ch)
{
	const bchar8 *end = str + GetSize(str);
	const bchar8 *find = bear_find_if_of_end(str, end, [ch](bchar8 item) { return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}
static inline bchar8 *ToCharWithEnd(bchar8 *str, bchar8 ch)
{
	bchar8 *end = str + GetSize(str);
	bchar8 *find = bear_find_if_of_end(str, end, [ch](bchar8 item) { return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}

static inline const bchar16 *ToChar(const bchar16 *str, bchar16 ch)
{
	return wcschr(str, ch);
}
static inline bchar16 *ToChar(bchar16 *str, bchar16 ch)
{
	return wcschr(str, ch);
}
static inline const bchar16 *ToCharWithEnd(const bchar16 *str, bchar16 ch)
{
	const bchar16 *end = str + GetSize(str);
	const bchar16 *find = bear_find_if_of_end(str, end, [ch](bchar16 item) { return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}
static inline bchar16 *ToCharWithEnd(bchar16 *str, bchar16 ch)
{
	bchar16 *end = str + GetSize(str);
	bchar16 *find = bear_find_if_of_end(str, end, [ch](bchar16 item) { return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}

static inline const bchar8 *Find(const bchar8 *str, const bchar8 *subStr)
{
	return strstr(str, subStr);
}
static inline bchar8 *Find(bchar8 *str, const bchar8 *subStr)
{
	return strstr(str, subStr);
}

static inline const bchar16 *Find(const bchar16 *str, const bchar16 *subStr)
{
	return wcsstr(str, subStr);
}
static inline bchar16 *Find(bchar16 *str, const bchar16 *subStr)
{
	return wcsstr(str, subStr);
}

template <bsize sizeBuffer>
static inline bsize Printf(bchar8(&dst)[sizeBuffer], const bchar8 *str, ...)
{
	va_list va;
	va_start(va, str);
	int result = vsnprintf(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
template <bsize sizeBuffer>
static inline bsize PrintfVa(bchar8(&dst)[sizeBuffer], const bchar8 *str, va_list va)
{
	int result = vsnprintf(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}

static inline bsize Printf(bchar8 *dst, bsize sizeBuffer, const bchar8 *str, ...)
{

	va_list va;
	va_start(va, str);
	int result = vsprintf(dst, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
static inline bsize PrintfVa(bchar8 *dst, bsize sizeBuffer, const bchar8 *str, va_list va)
{
	int result = vsprintf(dst, str, va);
	return static_cast<bsize>(result);
}

template <bsize sizeBuffer>
static inline bsize Printf(bchar16(&dst)[sizeBuffer], const bchar16 *str, ...)
{
	va_list va;
	va_start(va, str);
	int result = vsnwprintf(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
template <bsize sizeBuffer>
static inline bsize PrintfVa(bchar16(&dst)[sizeBuffer], const bchar16 *str, va_list va)
{
	int result = vsnwprintf(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}
static inline bsize Printf(bchar16 *dst, bsize sizeBuffer, const bchar16 *str, ...)
{

	va_list va;
	va_start(va, str);
	int result = vsnwprintf(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
static inline bsize PrintfVa(bchar16 *dst, bsize sizeBuffer, const bchar16 *str, va_list va)
{
	int result = vsnwprintf(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}

static inline int32 Scanf(const bchar8 *text, const bchar8 *str, ...)
{
	va_list va;
	va_start(va, str);
	int32 result = vsscanf(text, str, va);
	va_end(va);
	return result;
}
static inline int32 Scanf(const bchar16 *text, const bchar16 *str, ...)
{
	va_list va;
	va_start(va, str);
	int32 result = vswscanf(text, str, va);
	va_end(va);
	return result;
}
static inline void ToLower(bchar8 *p)
{
	while (*p)
	{
		*p = tolower((unsigned char)*p);
		p++;
	}
}
static inline void ToUpper(bchar8 *p)
{
	while (*p)
	{
		*p = toupper((unsigned char)*p);
		p++;
	}
}
static inline void ToLower(bchar16 *p)
{
	while (*p)
	{
		*p = towlower(*p);
		p++;
	}
}
static inline void ToUpper(bchar16 *p)
{
	while (*p)
	{
		*p = towupper(*p);
		p++;
	}
}
static inline int32 Compare(const bchar8 *str1, const bchar8 *str2)
{
	return strcmp(str1, str2);
}
static inline int32 Compare(const bchar16 *str1, const bchar16 *str2)
{
	return wcscmp(str1, str2);
}
static inline int32 CompareWithoutCase(const bchar8 *a, const bchar8 *b)
{
	int ca, cb;
	do
	{
		ca = (unsigned char)*a++;
		cb = (unsigned char)*b++;
		ca = tolower(toupper(ca));
		cb = tolower(toupper(cb));
	} while (ca == cb && ca != '\0');
	return ca - cb;
}
static inline int32 CompareWithoutCase(const bchar16 *a, const bchar16 *b)
{
	wint_t ca, cb;
	do
	{
		ca = (wint_t)*a++;
		cb = (wint_t)*b++;
		ca = towlower(towupper(ca));
		cb = towlower(towupper(cb));
	} while (ca == cb && ca != '\0');
	return ca - cb;
}

template <bsize sizeBuffer>
static inline void CopyWithSizeLimit(bchar16(&dst)[sizeBuffer], const bchar16 *src, bsize size)
{
	wcsncpy(dst, src, size);
}
static inline void CopyWithSizeLimit(bchar16 *dst, bsize sizeBuffer, const bchar16 *src, bsize size)
{
	wcsncpy(dst, src, size);
}
template <bsize sizeBuffer>
static inline void CopyWithSizeLimit(bchar8(&dst)[sizeBuffer], const bchar8 *src, bsize size)
{
	strncpy(dst, src, size);
}
static inline void CopyWithSizeLimit(bchar8 *dst, bsize sizeBuffer, const bchar8 *src, bsize size)
{
	strncpy(dst, src, size);
}

#else
static inline bsize GetSize(const bchar8*text)
{
	return strlen(text);
}
static inline bsize GetSize(const bchar16*text)
{
	return wcslen(text);
}
template<bsize sizeBuffer>
static inline void Contact(bchar8(&dst)[sizeBuffer], const  bchar8 *src)
{
	strcat_s(dst, sizeBuffer, src);
}

static inline void Contact(bchar8*dst, bsize sizeBuffer, const  bchar8 *src)
{
	strcat_s(dst, sizeBuffer, src);

}
template<bsize sizeBuffer>
static inline void Contact(bchar16(&dst)[sizeBuffer], const  bchar16 *src)
{
	wcscat_s(dst, sizeBuffer, src);
}
static inline void Contact(bchar16*dst, bsize sizeBuffer, const  bchar16 *src)
{
	wcscat_s(dst, sizeBuffer, src);

}

template<bsize sizeBuffer>
static inline void Copy(bchar16(&dst)[sizeBuffer], const  bchar16 *src)
{
	wcscpy_s(dst, sizeBuffer, src);
}
static inline void Copy(bchar16*dst, bsize sizeBuffer, const  bchar16 *src)
{
	wcscpy_s(dst, sizeBuffer, src);

}
template<bsize sizeBuffer>
static inline void Copy(bchar8(&dst)[sizeBuffer], const  bchar8 *src)
{
	strcpy_s(dst, sizeBuffer, src);
}
static inline void Copy(bchar8*dst, bsize sizeBuffer, const  bchar8 *src)
{
	strcpy_s(dst, sizeBuffer, src);
}

static inline const bchar8* ToChar(const bchar8* str, bchar8 ch)
{
	return strchr(str, ch);
}
static inline  bchar8* ToChar(bchar8* str, bchar8 ch)
{
	return strchr(str, ch);
}
static inline const bchar8* ToCharWithEnd(const bchar8* str, bchar8 ch)
{
	const bchar8*end = str + GetSize(str);
	const bchar8*find = bear_find_if_of_end(str, end, [ch](bchar8 item) {return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}
static inline  bchar8* ToCharWithEnd(bchar8* str, bchar8 ch)
{
	bchar8*end = str + GetSize(str);
	bchar8*find = bear_find_if_of_end(str, end, [ch](bchar8 item) {return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}


static inline const bchar16* ToChar(const bchar16* str, bchar16 ch)
{
	return wcschr(str, ch);
}
static inline  bchar16* ToChar(bchar16* str, bchar16 ch)
{
	return wcschr(str, ch);
}
static inline const bchar16* ToCharWithEnd(const bchar16* str, bchar16 ch)
{
	const bchar16*end = str + GetSize(str);
	const bchar16*find = bear_find_if_of_end(str, end, [ch](bchar16 item) {return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}
static inline  bchar16* ToCharWithEnd(bchar16* str, bchar16 ch)
{
	bchar16*end = str + GetSize(str);
	bchar16*find = bear_find_if_of_end(str, end, [ch](bchar16 item) {return item == ch; });
	if (find == end)
	{
		return 0;
	}
	return find;
}


static inline const bchar8* Find(const bchar8* str, const  bchar8 *subStr)
{
	return strstr(str, subStr);
}
static inline bchar8* Find(bchar8* str, const bchar8 *subStr)
{
	return strstr(str, subStr);
}

static inline const bchar16* Find(const bchar16* str, const  bchar16 *subStr)
{
	return wcsstr(str, subStr);
}
static inline bchar16* Find(bchar16* str, const bchar16 *subStr)
{
	return wcsstr(str, subStr);
}

template<bsize sizeBuffer>
static inline bsize Printf(bchar8(&dst)[sizeBuffer], const bchar8*str, ...)
{
	va_list va;
	va_start(va, str);
	int result = vsprintf_s(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
template<bsize sizeBuffer>
static inline bsize PrintfVa(bchar8(&dst)[sizeBuffer], const bchar8*str, va_list va)
{
	int result = vsprintf_s(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}

static inline bsize  Printf(bchar8*dst, bsize sizeBuffer, const bchar8*str, ...)
{

	va_list va;
	va_start(va, str);
	int result = vsprintf_s(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
static inline bsize  PrintfVa(bchar8*dst, bsize sizeBuffer, const bchar8*str, va_list va)
{
	int result = vsprintf_s(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}

template<bsize sizeBuffer>
static inline bsize Printf(bchar16(&dst)[sizeBuffer], const bchar16*str, ...)
{
	va_list va;
	va_start(va, str);
	int result = vswprintf_s(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
template<bsize sizeBuffer>
static inline bsize PrintfVa(bchar16(&dst)[sizeBuffer], const bchar16*str, va_list va)
{
	int result = vswprintf_s(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}
static inline bsize  Printf(bchar16*dst, bsize sizeBuffer, const bchar16*str, ...)
{

	va_list va;
	va_start(va, str);
	int result = vswprintf_s(dst, sizeBuffer, str, va);
	va_end(va);
	return static_cast<bsize>(result);
}
static inline bsize  PrintfVa(bchar16*dst, bsize sizeBuffer, const bchar16*str, va_list va)
{
	int result = vswprintf_s(dst, sizeBuffer, str, va);
	return static_cast<bsize>(result);
}


static inline int32 Scanf(const bchar8*text, const bchar8*str, ...)
{
	va_list va;
	va_start(va, str);
	int32 result = vsscanf(text, str, va);
	va_end(va);
	return result;
}
static inline int32 Scanf(const bchar16*text, const bchar16*str, ...)
{
	va_list va;
	va_start(va, str);
	int32 result = vswscanf(text, str, va);
	va_end(va);
	return result;
}
static inline void  ToLower(bchar8* str)
{
	_strlwr_s(str, strlen(str) + 1);
}
static inline void  ToUpper(bchar8* str)
{
	_strupr_s(str, strlen(str) + 1);
}
static inline void  ToLower(bchar16* str)
{
	_wcslwr_s(str, wcslen(str) + 1);
}
static inline void  ToUpper(bchar16* str)
{
	_wcsupr_s(str, wcslen(str) + 1);
}
static inline int32   Compare(const bchar8*str1, const bchar8*str2)
{
	return strcmp(str1, str2);
}
static inline int32   Compare(const bchar16*str1, const bchar16*str2)
{
	return wcscmp(str1, str2);
}
static inline int32  CompareWithoutCase(const bchar8*str1, const bchar8*str2)
{
	return _stricmp(str1, str2);
}
static inline int32   CompareWithoutCase(const bchar16*str1, const bchar16*str2)
{
	return _wcsicmp(str1, str2);
}





template<bsize sizeBuffer>
static inline void CopyWithSizeLimit(bchar16(&dst)[sizeBuffer], const  bchar16 *src, bsize size)
{
	wcsncpy_s(dst, sizeBuffer, src, size);
}
static inline void CopyWithSizeLimit(bchar16*dst, bsize sizeBuffer, const  bchar16 *src, bsize size)
{
	wcsncpy_s(dst, sizeBuffer, src, size);

}
template<bsize sizeBuffer>
static inline void CopyWithSizeLimit(bchar8(&dst)[sizeBuffer], const  bchar8 *src, bsize size)
{
	strncpy_s(dst, sizeBuffer, src, size);
}
static inline void CopyWithSizeLimit(bchar8*dst, bsize sizeBuffer, const  bchar8 *src, bsize size)
{
	strncpy_s(dst, sizeBuffer, src, size);
}
#endif
#define BEAR_PATH TEXT("\\")