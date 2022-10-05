#include "BearGraphics.hpp"
#include "BearRHI\BearRHIStats.h"
extern BEARGRAPHICS_API BearRHI::BearRHIStats* GStats;

#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)  bsize BearRenderStats::Count ## Name(){ if(GStats) return GStats->Count ## Name();return 0;}
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)
#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 

#include "BearTemplate\BearGraphicsObjectsList.h"


void BearRenderStats::Dump()
{
	BEAR_PRINTF(TEXT("-------------------------------DUMP Render resources--------------------------------"));
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)  BEAR_PRINTF(TEXT("" # Name ## ":%d"),Count ## Name());
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)
#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 

#include "BearTemplate\BearGraphicsObjectsList.h"
	BEAR_PRINTF(TEXT("------------------------------------------------------------------------------------"));
}

void BearRenderStats::Cheak()
{
	bsize Counter = 0;
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)  Counter += Count ## Name();
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)
#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 

#include "BearTemplate\BearGraphicsObjectsList.h"
	if (Counter == 0)return;
	Dump();
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)  BEAR_ASSERT(Count ## Name()==0);
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)
#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 

#include "BearTemplate\BearGraphicsObjectsList.h"
}
