
struct SLibraries
{
	HMODULE Module;
	BearMap<BearStringConteniar, FARPROC> Procs;
};
BearMap<BearStringConteniar, SLibraries> *GLibraries=nullptr;
bool BearManagerDynamicLibraries::CheckProject(BearStringConteniar  name_)
{
	if (GLibraries == nullptr)
	{
		BearStringPath fullname;
		BearString::Copy(fullname, *name_);
		BearString::Contact(fullname, TEXT(".dll"));

		SLibraries obj;
		obj.Module = LoadLibrary(fullname);
		if (obj.Module)
		{
			GLibraries = bear_new<BearMap<BearStringConteniar, SLibraries>>();
			GLibraries->insert(name_, obj);
			return true;
		}
		return false;
	}
	auto Item = GLibraries->find(name_);
	const bchar* name = *name_;
	if (Item == GLibraries->end())
	{
		BearStringPath fullname;
		BearString::Copy(fullname, name);
		BearString::Contact(fullname, TEXT(".dll"));

		SLibraries obj;
		obj.Module = LoadLibrary(fullname);
		if(obj.Module) GLibraries->insert(name, obj);
		return obj.Module;
	}
	return true;
}
inline void BearManagerDynamicLibraries::Destory()
{
	if (GLibraries == nullptr)return;
	auto begin = GLibraries->begin();
	auto end = GLibraries->end();
	while (begin != end)
	{
		if(begin->second.Module)
		FreeLibrary(begin->second.Module);
		begin++;
	}
	GLibraries->clear();
	bear_delete(GLibraries);
}
inline void BearManagerDynamicLibraries::UnLoad(BearStringConteniar name)
{
	if (GLibraries == nullptr)return;
	auto Item = GLibraries->find(name);
	if (Item != GLibraries->end())
	{
		FreeLibrary(Item->second.Module);
		GLibraries->erase(Item);
	}
}
void * BearManagerDynamicLibraries::GetFunctionInProjectImpl(BearStringConteniar name_, BearStringConteniar function)
{
	if (GLibraries == nullptr)
	{
		BearStringPath fullname;
		BearString::Copy(fullname, *name_);
		BearString::Contact(fullname, TEXT(".dll"));

		SLibraries obj;
		obj.Module = LoadLibrary(fullname);
		if (obj.Module)
		{
			GLibraries = bear_new<BearMap<BearStringConteniar, SLibraries>>();
			GLibraries->insert(name_, obj);
		}
		else
		{
			return 0;
		}
	}
	auto Item= GLibraries->find(name_);
	const bchar* name=*name_;
	if (Item == GLibraries->end())
	{
		BearStringPath fullname;
		BearString::Copy(fullname, name);
		BearString::Contact(fullname, TEXT(".dll"));

		SLibraries obj;
		obj.Module = LoadLibrary(fullname);
		BEAR_CHECK(obj.Module);;
		GLibraries->insert(name, obj);
		Item = GLibraries->find(name);
	}
	auto ItemProc = Item->second.Procs.find(function);
	if (ItemProc == Item->second.Procs.end())
	{
#ifdef UNICODE
		FARPROC DLLFunction = GetProcAddress(Item->second.Module, *BearEncoding::FastToAnsi(*function));
#else
		FARPROC DLLFunction = GetProcAddress(Item->second.Module, *function);
#endif
		if (!DLLFunction) return nullptr;
		Item->second.Procs.insert(function, DLLFunction);
		return reinterpret_cast<void*>(DLLFunction);
	}
	return reinterpret_cast<void*>( ItemProc->second);
}
