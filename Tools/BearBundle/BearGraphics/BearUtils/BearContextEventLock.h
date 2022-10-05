#pragma once
class BearContextEventLock
{
	BearRHI::BearRHIContext& m_Context;
public:
	BearContextEventLock(BearRHI::BearRHIContext& Context, const char* name, BearColor color = BearColor::White) :m_Context(Context)
	{
		m_Context.BeginEvent(name, color);
	};
	inline ~BearContextEventLock()
	{
		m_Context.EndEvent();
	}
};