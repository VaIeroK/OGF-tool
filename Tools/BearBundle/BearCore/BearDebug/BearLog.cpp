#include "BearCore.hpp"
#include <iostream>
#if 0
#include "BugTrap.h"
#endif


#ifdef DEVELOPER_VERSION
void BearLog::DebugPrintf(const bchar * text, ...)
{
	
	BearMutexLock lock(LogMutex);
	BearString8192 var1;
	va_list va;
	va_start(va, text);
	BearString::PrintfVa(var1, text, va);
	va_end(va);
#if  defined(WINDOWS)&&defined(DEVELOPER_VERSION)
	OutputDebugString(var1);
	OutputDebugString(TEXT("\r\n"));
#endif
#ifdef WINDOWS
	char text866[8192];
	CharToOem(var1, text866);
	printf(text866);
	printf("\r\n");
#elif UNIX
#ifdef UNICODE
	wprintf(var1);
	wprintf(TEXT("\n"));
#else
	puts((const char*)var1);
	puts((const char*)TEXT("\n"));
#endif
#endif
	Push(var1);
}
#endif
BearLog::BearLog()
{
	LogFileOut[0] = 0;
}
BearLog::~BearLog()
{
}
void BearLog::Printf(const bchar * text, ...)
{

	BearMutexLock lock(LogMutex);
	BearString8192 var1;
	va_list va;
	va_start(va, text);
	BearString::PrintfVa(var1, text, va);
	va_end(va);
#if  defined(WINDOWS)&&defined(DEVELOPER_VERSION)
	OutputDebugString(var1);
	OutputDebugString(TEXT("\r\n"));
#endif
#ifdef WINDOWS
	char text866[8192];
	CharToOem(var1, text866);
	printf(text866);
	printf("\r\n");
#elif UNIX
#ifdef UNICODE 
	wprintf(var1);
	wprintf(TEXT("\n"));
#else
	puts((const char*)var1);
	puts((const char*)TEXT("\n"));
#endif
#endif
	Push(var1);
}

void BearLog::Flush()
{
	if (!LogFileOut[0])
	{
		LogData.clear();
		return;
		
	}
	BearMutexLock lock(LogMutex);
	auto b = LogData.begin();
	auto e = LogData.end();
	BearFileStream LogFile ;
	if (!LogFile.Open(LogFileOut, LogFile.M_Write))return;
	while (b != e)
	{
		LogFile.WriteBuffer((void*)**b, sizeof(bchar)*(b->size()));
#ifdef WINDOWS
		LogFile.WriteBuffer(TEXT("\r\n"), sizeof(bchar) * 2);
#else
		LogFile.Write((void*)TEXT("\n"), sizeof(bchar) * 2);
#endif
		b++;
	}
	LogData.clear();
}


const BearVector<BearString>& BearLog::LockData()
{
	LogMutex.Lock();
	return LogData;
}

void BearLog::UnlockData()
{
	LogMutex.Unlock();
}

void BearLog::SetFile(const bchar * file)
{
	BearString::Copy(LogFileOut, file);
}


void BearLog::PushLine(const bchar* string)
{
}

void BearLog::Push( bchar * string)
{

	bsize cur = 0;
	for (bsize i = 0; string[i]; i++)
	{
		if (string[i] == TEXT("\r")[0])
		{
			string[i] = 0;
			if (string[i + 1] == TEXT("\n")[0])
				i++;
			i++;	
			if (string[cur]) {
				PushLine(&string[cur]);
				LogData.push_back(&string[cur]);
			}
			cur = i;
		}
		else if (string[i] == TEXT("\n")[0])
		{
			string[i] = 0;
			i++;	
			if (string[cur]) {

				PushLine(&string[cur]);
				LogData.push_back(&string[cur]);
			}
			cur = i;
		}
	}
	if (string[cur]) {

		PushLine(&string[cur]);
		LogData.push_back(&string[cur]);
	}
}

int32 BearLog::GetBuildImpl(const char * date, int32 start_year, int32 start_month, int32 start_day)
{
	static const bchar* month_id[12] =
	{
		TEXT("Jan"), TEXT("Feb"), TEXT("Mar"),TEXT("Apr"),TEXT("May"), TEXT("Jun"), TEXT("Jul"), TEXT("Aug"),TEXT("Sep"), TEXT("Oct"), TEXT("Nov"), TEXT("Dec")
	};
	int32 build_id = 0;
	static int days_in_month[12] =
	{
		31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
	};
	BearString data;

#ifdef UNICODE
	data.append(*BearEncoding::FastToUnicode(date));
#else
	data.append(date);
#endif

	BearString16 month;
	uint32 days = 0, year = 0;
#ifdef WINDOWS
	data.scanf(TEXT("%s %u %u"), month, &days, &year);
#else 
	data.scanf(TEXT("" BEAR_PRINT_UTF8 " %d %d"), month, &days, &year);
#endif
	int32 months = 0;
	for (int i = 0; i < 12; i++)
	{
		if (BearString::Compare(month, month_id[i]) != 0)
			continue;

		months = i;
		break;
	}

	build_id = (year - start_year) * 365 + days - start_day;

	for (int i = 0; i < months; ++i)
		build_id += days_in_month[i];

	for (int i = 0; i < start_month - 1; ++i)
		build_id -= days_in_month[i];
	data.clear();
	return build_id;
}
