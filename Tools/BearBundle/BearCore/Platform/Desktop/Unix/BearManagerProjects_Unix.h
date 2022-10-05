#include <dlfcn.h>
struct lib
{
	void* Module;
	BearCore::BearMap<BearCore::BearStringConteniar,   void*> Procs;
};
BearCore::BearMap<BearCore::BearStringConteniar, lib> Libs;
bool BearCore::BearProjectTool::CheckProject(const bchar * name)
{
	auto Item = Libs.find(BearStringConteniar(name, false));
	if (Item == Libs.end())
	{
		const bchar*name_project = name;
		BearStringPath fullname,main_project,name_data;
		BearString::Copy(main_project, TEXT(BEAR_TO_STRING(MAIN_PROJECT_NAME)));
		if (BearString::ToChar(main_project, TEXT("_")[0]))
			BearString::ToChar(main_project, TEXT("_")[0])[0] = 0;

	    BearString::Copy(fullname, TEXT("lib"));
		BearString::Contact(fullname, TEXT(BEAR_TO_STRING(MAIN_PROJECT_NAME)));
		BearString::Contact(fullname, TEXT("_"));

		BearString::Copy(name_data, name);
		name = name_data;
		if (BearString::ExistPossition(name, 0, main_project))
		{
			name += BearString::GetSize(main_project);
		}
		if (name[0] == TEXT("_")[0])
			name++;

		BearString::Contact(fullname, name);
#ifdef _DEBUG
		BearString::Contact(fullname, TEXT("_debug"));
#elif MIXED
		BearString::Contact(fullname, TEXT("_mixed"));
#endif
		BearString::Contact(fullname, TEXT(".so"));
		lib obj;
		BearLog::DebugPrintf(TEXT("Проверка библиотеки " BEAR_PRINT_CURRENT),fullname);
		obj.Module = dlopen(*BearEncoding::ToANSI( fullname), RTLD_LAZY);
		if(obj.Module) Libs.insert(name_project, obj);
		return obj.Module;
	}
	return false;
}
void BearCore::BearProjectTool::Destory()
{
	auto begin = Libs.begin();
	auto end = Libs.end();
	while (begin != end)
	{
		if(begin->second.Module)
		dlclose(begin->second.Module);
		begin++;
	}
	Libs.clear();
}
void BearCore::BearProjectTool::UnLoad(const bchar * name)
{
	auto Item = Libs.find(BearStringConteniar(name, false));
	if (Item != Libs.end())
	{
		dlclose(Item->second.Module);
		Libs.erase(Item);
	}
}
void * BearCore::BearProjectTool::GetFunctionInProjectImpl(const bchar * name, const bchar * function)
{
	auto Item= Libs.find(BearStringConteniar( name,false));
	if (Item == Libs.end())
	{
		const bchar*name_project = name;
		BearStringPath fullname, main_project, name_data;
		BearString::Copy(main_project, TEXT(BEAR_TO_STRING(MAIN_PROJECT_NAME)));
		if (BearString::ToChar(main_project, TEXT("_")[0]))
			BearString::ToChar(main_project, TEXT("_")[0])[0] = 0;

		
	    BearString::Copy(fullname, TEXT("lib"));
		BearString::Contact(fullname, TEXT(BEAR_TO_STRING(MAIN_PROJECT_NAME)));
		BearString::Contact(fullname, TEXT("_"));

		BearString::Copy(name_data, name);
		name = name_data;
		if (BearString::ExistPossition(name, 0, main_project))
		{
			name += BearString::GetSize(main_project);
		}
		if (name[0] == TEXT("_")[0])
			name++;

		BearString::Contact(fullname, name);
#ifdef DEBUG
		BearString::Contact(fullname, TEXT("_debug"));
#elif MIXED
		BearString::Contact(fullname, TEXT("_mixed"));
#endif
		BearString::Contact(fullname, TEXT(".so"));
		lib obj;
		obj.Module = dlopen(*BearEncoding::ToANSI( fullname), RTLD_LAZY);
		BearLog::DebugPrintf(TEXT("Загрузка Библиотеки " BEAR_PRINT_CURRENT),fullname);
		BEAR_FATALERROR(obj.Module, TEXT("Бибилиотека " BEAR_PRINT_CURRENT " не сущесвтует или повреждена"), fullname);
		Libs.insert(name_project, obj);
		Item = Libs.find(BearStringConteniar(name_project, false));
	}
	auto ItemProc = Item->second.Procs.find(BearStringConteniar(function,false));
	if (ItemProc == Item->second.Procs.end())
	{
		void* proc = dlsym(Item->second.Module, *BearEncoding::ToANSI(function));
		BEAR_FATALERROR(proc, TEXT("Функция " BEAR_PRINT_CURRENT " не существует в библиотеке " BEAR_PRINT_CURRENT ""), function, name);
		Item->second.Procs.insert(function, proc);
		return proc;
	}
	return ItemProc->second;
}
