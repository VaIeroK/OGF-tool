#pragma once
namespace Impl
{
	namespace BearTypeManager
	{
		
		template<typename C>
		inline	BearGraphicsObjectType GetType()
		{
			return GOT_None;
		}

#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...) template<> inline	BearGraphicsObjectType GetType<BearRHI::BearRHI ## Name>(){return GOT_ ## Name;}
#include "BearTemplate/BearGraphicsObjectsList.h"



	};
}