#pragma once
#ifdef WINDOWS
#undef GetCurrentTime
#endif
class BEARTOOL_API BearGlobalTime
{
public:
	BearGlobalTime() :Year(0), Month(0), Day(0), Hour(0), Minute(0), Second(0) {}

	inline void copy(const BearGlobalTime&right)
	{
			Year= right.Year;
			Month= right.Month;
			Day= right.Day;
			Hour= right.Hour;
			Minute= right.Minute;
			Second= right.Second;
	}
	inline void swap(BearGlobalTime&right)
	{
		bear_swap(Year, right.Year);
		bear_swap(Month, right.Month);
		bear_swap(Day, right.Day);
		bear_swap(Hour, right.Hour);
		bear_swap(Minute, right.Minute);
		bear_swap(Second, right.Second);
	}
	BearGlobalTime(const BearGlobalTime&right)
	{
		copy(right);
	}
	BearGlobalTime(BearGlobalTime&&right) :Year(0), Month(0), Day(0), Hour(0), Minute(0), Second(0)
	{
		swap(right);
	}
	uint32 Year;
	uint32 Month;
	uint32 Day;
	uint32 Hour;
	uint32 Minute;
	uint32 Second;
	inline 		BearGlobalTime &operator=(const BearGlobalTime &right)
	{
		copy(right);
		return*this;
	}
	inline 		BearGlobalTime &operator=(BearGlobalTime &&right)
	{
		swap(right);
		return*this;
	}

	inline bool operator<(const BearGlobalTime&time)const
	{
		if (Year != time.Year)
			return Year < time.Year;
		else if (Month != time.Month)
			return Month < time.Month;
		else if (Day != time.Day)
			return Day < time.Day;
		else if (Hour != time.Hour)
			return Hour < time.Hour;
		else if (Minute != time.Minute)
			return Minute < time.Minute;
		return Second < time.Second;
	}
	inline bool operator>(const BearGlobalTime&time)const
	{
		if (Year != time.Year)
			return Year > time.Year;
		else if (Month != time.Month)
			return Month > time.Month;
		else if (Day != time.Day)
			return Day > time.Day;
		else if (Hour != time.Hour)
			return Hour > time.Hour;
		else if (Minute != time.Minute)
			return Minute > time.Minute;
		return Second > time.Second;
	}

	inline bool operator==(const BearGlobalTime&time)const
	{
		if (Year == time.Year&&Month == time.Month&&Day == time.Day&&Hour == time.Hour&&Minute == time.Minute&&Second == time.Second)
			return true;
		return false;
	}
	inline bool operator>=(const BearGlobalTime&time)const
	{
		return !this->operator<(time);
	}
	inline bool operator<=(const BearGlobalTime&time)const
	{
		return !this->operator>(time);
	}
	inline bool operator!=(const BearGlobalTime&time)const
	{
		return !this->operator==(time);;
	}
	static BearGlobalTime GetCurrentTime();
};