#pragma once
#ifndef MAIN_PROJECT_NAME
#define MAIN_PROJECT_NAME ""
#endif
class BEARTOOL_API BearManagerDynamicLibraries
{
	BEAR_CLASS_STATIC(BearManagerDynamicLibraries);
public:
	template<typename C> 
	static C GetFunctionInProject(BearStringConteniar name, BearStringConteniar function)
	{
		return reinterpret_cast<C>(GetFunctionInProjectImpl(name, function));
	}
	static bool CheckProject(BearStringConteniar name);
	static void Destory();
	static void UnLoad(BearStringConteniar name);
private:
	static void * GetFunctionInProjectImpl(BearStringConteniar name, BearStringConteniar function);
};
