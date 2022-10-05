#pragma once
namespace BearRHI
{
	class BEARGRAPHICS_API BearRHIObject
	{
	public:

		virtual void* QueryInterface(int Type);
		virtual ~BearRHIObject() = 0;
	};
}