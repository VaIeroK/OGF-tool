#include "BearCore.hpp"


#include <locale.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include <lzo/lzoconf.h>
#include <lzo/lzo1x.h>
#include <zlib/zlib.h>

extern void GKeyWindowsInitialize();
extern void GKeyWindowsDestroy();
static bool bInitialize=false;
extern lzo_voidp GLZOWrkmem;
extern lzo_voidp  GLZOWrkmem999;
#ifndef GCC
extern void CALLBACK GErrorHandler(INT_PTR);
#endif

z_stream GzlibStream;

static BearMemory* LMemory = nullptr;
BearMemory* BearCore::GetMemory()
{
	return LMemory;
}
static BearDebug* LDebug = nullptr;
BearDebug* BearCore::GetDebug()
{
	return LDebug;
}
static BearLog* LLog = nullptr;
BearLog* BearCore::GetLog()
{
	return LLog;
}

void BearCore::Destroy()
{
	BEAR_ASSERT(bInitialize);
	Impl::BearStringConteniarManager::Destroy();
	GKeyWindowsDestroy();
	if (GLZOWrkmem)bear_free(GLZOWrkmem);
	if (GLZOWrkmem999)bear_free(GLZOWrkmem999);
	BearManagerDynamicLibraries::Destory();
	LLog->Flush();
	delete LDebug;
	LDebug = nullptr;
	delete LLog;
	LLog = nullptr;
	delete LMemory;
	LMemory = nullptr;
	bInitialize = false;
}

void BearCore::SetMemory(BearMemory* memory)
{
	LMemory = memory;
}

void BearCore::Initialize(BearLog* Log, BearDebug* Debug)
{

	BEAR_CHECK(!LDebug);
	LLog = Log;
	LDebug = Debug;
	BEAR_CHECK(!bInitialize);
	bear_fill(GzlibStream);
	Impl::BearStringConteniarManager::Initialize();
	//BEAR_PRINTF(TEXT("BearCore build " BEAR_PRINT_STR_CURRENT), *BearLog::GetBuild(2015, 07, 27));

#if 0
	BT_InstallSehFilter();
	BT_SetActivityType(BTA_SHOWUI);
	BT_SetDialogMessage(
		BTDM_INTRO2,
		TEXT("Unknown error! ")
	);
	BT_SetDialogMessage(
		BTDM_INTRO1,
		TEXT("Fatal error!")
	);
	BT_SetReportFormat(BTRF_TEXT);
	BT_SetFlags(/**/BTF_DETAILEDMODE | /**BTF_EDIETMAIL |*/BTF_ATTACHREPORT /**| BTF_LISTPROCESSES *| BTF_SHOWADVANCEDUI *| BTF_SCREENCAPTURE*/);

	/*if(email)
	BT_SetSupportEMail(email);*/
	BT_SetDumpType(0);
	BT_SetAppName(App);
	BT_SetPreErrHandler(GErrorHandler, 0);
#endif
	bInitialize = true;
	BEAR_CHECK(lzo_init() == LZO_E_OK);

	BEAR_CHECK(inflateInit(&GzlibStream) == Z_OK);	inflateEnd(&GzlibStream);
	GKeyWindowsInitialize();
}
