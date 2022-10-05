#pragma once
namespace Impl
{

	class BearRemoverObject
	{
	public:
		template<typename T>
		static void Remove(void*object, BearGraphicsObjectType InType)
		{
			if (object == 0)return;
			switch (InType)
			{
			case GOT_None:
				bear_delete (reinterpret_cast<T*>(object));
				break;

#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...) case GOT_##Name: bear_delete(reinterpret_cast<BearRHI::BearRHI##Name*>(object));  break;
#include "BearTemplate/BearGraphicsObjectsList.h"
			default:
				bear_delete(object);
				break;
			};
		}
	};
}