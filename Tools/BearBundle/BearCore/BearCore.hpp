#pragma once
#include <wchar.h>
#include <stdio.h>
#include <string.h>
#include <vector>
#include <map>
#include <typeinfo>
#include <math.h>
#include <cmath>
#include <string>
#include <limits>

#include "BearTypes.h"
 

#ifdef BEARCORE_EXPORTS
#define BEARTOOL_API  BEARDLL_EXPORT
#else
#define BEARTOOL_API  BEARDLL_IMPORT
#endif

#include "BearTemplate/BearUtilsForClass.h"
#include "BearTemplate/BearDefines.h"
class BearMemory;
class BearDebug;
class BearLog;
class BEARTOOL_API BearCore
{
	BEAR_CLASS_STATIC(BearCore);
public:
	static BearMemory* GetMemory();
	static BearDebug* GetDebug();
	static BearLog* GetLog();
	template<class Memory = BearMemory, class Log = BearLog, class Debug = BearDebug>
	static void Initialize()
	{
		SetMemory(new Memory);
		Initialize( new Log, new Debug);
	}
	static void Destroy();
private:
	static void SetMemory(BearMemory* memory);
	static void Initialize(BearLog* Log, BearDebug* Debug);
};


#include "BearTemplate/BearFunctional.h"
#include "BearTemplate/BearAlgorithm.h"

#include "BearMemory/BearMemory.h"
#include "BearMemory/BearMemoryTemplate.h"
#include "BearMemory/BearMemoryAllocator.h"
#include "BearMemory/BearMemoryMultiRef.h"
#include "BearMemory/BearMemoryRef.h"


#include "BearDebug/BearDebug.h"
#include "BearDebug/BearDebugDefines.h"

#include "BearTemplate/BearVector.h"
#include "BearTemplate/BearMap.h"

#include "BearTemplate/BearString.h"
#include "BearTemplate/BearStringConteniar.h"


#include "BearUtils/BearMutex.h"
#include "BearDebug/BearLog.h"

#include "BearUtils/BearEncoding.h"

#include "BearIO/BearInputStream.h"
#include "BearIO/BearBufferedReader.h"
#include "BearIO/BearOutputStream.h"

#include "BearUtils/BearFlags.h"

#include "BearIO/BearFileStream.h"
#include "BearIO/BearMemoryStream.h"

#include "BearIO/BearFileManager.h"

#include "BearIO/BearFilePackageStream.h"
#include "BearIO/BearPackage.h"

#include "BearUtils/BearIncluder.h"

#include "BearParser/BearINI.h"
#include "BearParser/BearXML.h"

#include "BearIO/BearFileSystem.h"

#include "BearMath/BearMath.h"

#include "BearMath/BearVector2.h"
#include "BearMath/BearVector3.h"
#include "BearMath/BearVector4.h"

#include "BearIO/BearInput.h"

#include "BearMath/BearColor.h"

#include "BearMath/BearFraction.h"

#include "BearMath/BearMatrix.h"
#include "BearMath/BearCamera.h"

#include "BearMath/BearTime.h"


#include "BearTemplate/BearMultiplicatorDefine.h"

#include "BearTemplate/BearFunctionReflector.h"

#include "BearUtils/BearCheakSum.h"

#include "BearUtils/BearCompressor.h"

#include "BearUtils/BearGlobalTime.h"
#include "BearUtils/BearTimer.h"


#include "BearUtils/BearThread.h"

#include "BearUtils/BearManagerDynamicLibraries.h"

#include "BearUtils/BearSystem.h"

#include "BearTemplate/BearFunction.h"
#include "BearTemplate/BearFastDelegate.h"


#include "BearNet/BearIP.h"
#include "BearNet/BearNetTcpClient.h"
#include "BearNet/BearNetTcpServer.h"