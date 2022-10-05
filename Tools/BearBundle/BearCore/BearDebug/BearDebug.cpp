#include "BearCore.hpp"
static void(*LCallBack)(void) = 0;
extern BearStringPath LogFileOut;
#if 0
#include "BugTrap.h"
#pragma warning(disable:4091)
#include <dbghelp.h> // MiniDump flags
void CALLBACK GErrorHandler(INT_PTR)
{
	if (LCallBack)LCallBack();
	BearLog::Flush();
	BT_AddLogFile(LogFileOut);
	BT_SaveSnapshot(0);
}

#elif UNIX
#include <signal.h>
#endif


BearDebug::BearDebug()
{
}

BearDebug::~BearDebug()
{
}

void BearDebug::Break()
{
#ifdef MSVC
	__debugbreak();
#else
   __builtin_trap();
#endif
}




void BearDebug::Error(const bchar* chenk, const bchar* name, const char* function_name, uint32 line)
{
#ifdef MSVC
	if (IsDebuggerPresent())
	{
		Break();
		return;
	}
#endif
	BearString str;

	str.append(TEXT("\r\nОшибка в файле:")).append(name);
	str.append(TEXT("\r\nМесто:")).append(chenk);
#ifdef UNICODE
	str.append(TEXT("\r\nФункция:")).append(*BearEncoding::FastToUnicode( function_name));
#else
	str.append(TEXT("\r\nФункция:")).append(function_name);
#endif
	str.append(TEXT("\r\nСтока:"));
	str.append_printf(TEXT("%u"),line);
	BEAR_PRINTF(TEXT(BEAR_PRINT_STR_CURRENT), *str);
	Break();
}

void BearDebug::Error(const bchar* text, ...)
{
	BearString str;
	va_list va;
	va_start(va, text);
	str.append_printf_va(text, va);
	va_end(va);
	BEAR_PRINTF(TEXT(BEAR_PRINT_STR_CURRENT), *str);
}

void BearDebug::ErrorMessage(const bchar* text, ...)
{
	BearString str;
	va_list va;
	va_start(va, text);
	str.append_printf_va(text, va);
	va_end(va);
	MessageBox(NULL, text, TEXT("Error"), MB_ICONERROR);
	exit(-1);
}
