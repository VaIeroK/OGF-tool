#pragma once
class BEARTOOL_API BearLog
{
	BEAR_CLASS_WITHOUT_COPY(BearLog);
public:
	BearLog();
	virtual  ~BearLog();
	virtual void Printf(const bchar*text, ...);
	virtual void Flush();
	virtual void SetFile(const bchar*file);

	BEAR_FUNCTION_DEBUG(void, DebugPrintf, const bchar * text, ...);

	virtual const BearVector<BearString>& LockData();
	virtual void UnlockData();
public:
	inline static BearString GetBuild(int32 year, int32 month, int32 day)
	{
		return BearString().append_printf(TEXT("%d," BEAR_PRINT_STR), GetBuildImpl(__DATE__, year, month, day), __DATE__);
	}
protected:
	virtual void PushLine(const bchar* string);
private:

	void Push(bchar* string);
	static int32 GetBuildImpl(const char* date, int32 year, int32 month, int32 day);
	BearVector<BearString> LogData;
	BearMutex LogMutex;
	BearStringPath LogFileOut;
};
#define BEAR_PRINTF(...) BearCore::GetLog()->Printf(__VA_ARGS__)
#define BEAR_DEBUG_PRINTF(...) BearCore::GetLog()->DebugPrintf(__VA_ARGS__)