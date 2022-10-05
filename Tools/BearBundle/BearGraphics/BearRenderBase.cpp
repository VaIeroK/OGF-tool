#include "BearGraphics.hpp"
#include "BearRenderBase.h"
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)    BearRHI##Name::~BearRHI##Name(){}
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name)
namespace BearRHI
{
#include "BearTemplate/BearGraphicsObjectsList.h"
}
BearRHI::BearRHIObject::~BearRHIObject()
{

}
void*BearRHI::BearRHIObject::QueryInterface(int)
{
	return nullptr;
}
BearRHI::BearRHIFactory::~BearRHIFactory()
{

}
BearRHI::BearRHIStats::~BearRHIStats()
{

}