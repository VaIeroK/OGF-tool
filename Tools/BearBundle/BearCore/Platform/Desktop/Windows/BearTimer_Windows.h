#include <time.h>
#include <ctime>
#include <assert.h>
static double CyclesPerMicrosecond;
struct Initializer
{
	
	inline void Initialize()
	{
		static bool ok = false;
		if (ok)return;
		ok = true;
		LARGE_INTEGER Frequency;
		QueryPerformanceFrequency(&Frequency);
		CyclesPerMicrosecond = (1.0 / Frequency.QuadPart)*(1000.0)*(1000.0);
	}

};
static Initializer initializer;
BearTime BearTimer::GetCurrentTime() 
{
	initializer.Initialize();
	LARGE_INTEGER Cycles;
	QueryPerformanceCounter(&Cycles);
	return BearTime(static_cast<int64>((Cycles.QuadPart * CyclesPerMicrosecond)));
}