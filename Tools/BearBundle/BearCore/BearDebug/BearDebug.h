#pragma once
class BEARTOOL_API BearDebug
{
	BEAR_CLASS_WITHOUT_COPY(BearDebug);
public:
	BearDebug();
	virtual ~BearDebug();
	virtual void Break();
	virtual void Error(const bchar * chenk, const bchar * name, const char * function_name, uint32 line);
	virtual void Error(const bchar* text,...);
	virtual  void ErrorMessage(const bchar* text, ...);
};
