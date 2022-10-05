#pragma once
namespace Impl
{

	namespace BearCasterObject
	{
		template<typename C>
		 inline void*CastParent( C*, BearGraphicsObjectType OutType) 
		{
			return 0;
		}
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) \
		template<>\
		 inline void*CastParent<BearRHI::BearRHI##Name>(BearRHI::BearRHI##Name *C,BearGraphicsObjectType OutType)  \
		{ \
			BearRHI::BearRHI##Parent*P= static_cast<BearRHI::BearRHI##Parent*>(C);\
			if(OutType==BearTypeManager::GetType<BearRHI::BearRHI##Parent>())\
				return reinterpret_cast<void*>(P);\
			return CastParent<BearRHI::BearRHI##Parent>(P,OutType); \
		}
#include "BearTemplate/BearGraphicsObjectsList.h"

		 inline void*Cast(void*in, BearGraphicsObjectType InType, BearGraphicsObjectType OutType)
		{
			if (OutType == GOT_None)return 0;
			if (OutType == InType)return in;
			switch (InType)
			{
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) \
case GOT_##Name: return CastParent<BearRHI::BearRHI##Name>(reinterpret_cast<BearRHI::BearRHI##Name*>(in),OutType);
#include "BearTemplate/BearGraphicsObjectsList.h"
			default:
				return 0;
				break;
			}
		//	return 0;
		}
	};
}