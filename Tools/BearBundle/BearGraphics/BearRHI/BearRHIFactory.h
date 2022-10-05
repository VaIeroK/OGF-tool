#pragma once
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...) virtual BearRHI ## Name*Create ## Name(__VA_ARGS__)=0;
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name,__VA_ARGS__)

#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...)
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) 
#define RENDER_CONSTRUCTOR_REGISTRATION(Name,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name,__VA_ARGS__)

namespace BearRHI
{
	class BEARGRAPHICS_API  BearRHIFactory
	{
	public:
#include "..\BearTemplate\BearGraphicsObjectsList.h"
		virtual ~BearRHIFactory() = 0;
		virtual bool SupportRayTracing() = 0;
		virtual bool SupportMeshShader() = 0;
	};
}


