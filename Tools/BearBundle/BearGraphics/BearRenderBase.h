#pragma once



#include "BearGraphics.hpp"


#include "BearRHI/BearRHIFactory.h"
#include "BearRHI/BearRHIStats.h"

extern BEARGRAPHICS_API BearRHI::BearRHIFactory*GFactory ;
extern BEARGRAPHICS_API BearRHI::BearRHIStats* GStats;
#ifndef BEAR_GRAPHICS_EXPORTS
extern "C"
{
	__declspec(dllexport) bool RHIInitialize();
}
#endif
#include "BearRHI/BearTextureUtils.h"