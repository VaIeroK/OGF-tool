#pragma once

#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)  static bsize Count ## Name();
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)
#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 

class BEARGRAPHICS_API BearRenderStats
{
public:
#include "BearTemplate\BearGraphicsObjectsList.h"

	static void Dump();
	static void Cheak();
};