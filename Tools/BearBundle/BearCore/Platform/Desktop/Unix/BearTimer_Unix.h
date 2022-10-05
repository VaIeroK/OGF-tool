#include <time.h>
BearCore::BearTime BearCore::BearTimer::GetCurrentTime() const
{
    timespec time;
    clock_gettime(CLOCK_MONOTONIC, &time);
    return BearCore::BearTime(static_cast<int64>(time.tv_sec) * 1000000 + time.tv_nsec / 1000);
}
 
