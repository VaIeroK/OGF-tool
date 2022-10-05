#include "BearCore.hpp"
#ifdef WINDOWS
#include "Windows/BearGlobalTime_Windows.h"
#elif UNIX
#include "Unix/BearGlobalTime_Unix.h"
#endif 
