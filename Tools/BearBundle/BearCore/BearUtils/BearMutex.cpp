#include "BearCore.hpp"
#ifdef WINDOWS
#include "Windows/BearMutex_Windows.h"
#elif UNIX
#include "Unix/BearMutex_Unix.h"
#endif

