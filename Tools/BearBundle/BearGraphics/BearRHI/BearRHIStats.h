#pragma once
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...) virtual bsize Count ## Name()=0;
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)

#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 


namespace BearRHI
{
	class BEARGRAPHICS_API  BearRHIStats
	{
	public:
#include "..\BearTemplate\BearGraphicsObjectsList.h"
		virtual ~BearRHIStats() = 0;
	};
}


