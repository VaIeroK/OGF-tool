#include <ctime>

BearGlobalTime BearGlobalTime::GetCurrentTime()
{
	BearGlobalTime time;
	std::time_t t = std::time(0);   
	std::tm now;
#ifdef MSVC
	localtime_s(&now,&t);
#else
	now = *localtime(&t);
#endif
	time.Year = now.tm_year + 1980;
	time.Month = now.tm_mon + 1;
	time.Day = now.tm_mday;
	time.Hour = now.tm_hour;
	time.Minute = now.tm_min;
	time.Second = now.tm_sec;
	return time;
}
