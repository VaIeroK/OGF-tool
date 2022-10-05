
bool BearFileManager::FileCopy(const bchar * in, const bchar * out)
{
	return CopyFile(in, out, false);
}


bool BearFileManager::DirectoryCreate(const bchar * path)
{
#ifdef UNICODE
	return CreateDirectoryW(path, NULL) || GetLastError() == ERROR_ALREADY_EXISTS;
#else
	return CreateDirectoryA(path, NULL) || GetLastError() == ERROR_ALREADY_EXISTS;
#endif
}

bool BearFileManager::DirectoryExists(const bchar * name)
{

	DWORD dwAttrs = GetFileAttributes(name);

	if (dwAttrs == 16 || dwAttrs == 48 || dwAttrs == 22)
		return true;
	return false;
}
void BearFileManager::GetWorkPath(BearStringPath & path)
{
	GetCurrentDirectory(MAX_PATH , path);
	PathOptimization(path);
}

void BearFileManager::GetApplicationPath(BearStringPath & path)
{

	GetModuleFileName(GetModuleHandle(NULL), path, MAX_PATH);
	BearString::ToCharWithEnd(path, BEAR_PATH[0])[0] = 0;
	PathOptimization(path);
}


static void Find(BearVector<BearString>& list, const bchar* path, const bchar* ext, bool full_path, bool findFile = true)
{
	WIN32_FIND_DATA file;
	BearStringPath full;
	BearString::Copy(full, path);
	BearString::Contact(full, BEAR_PATH);
	BearString::Contact(full, ext);
	HANDLE search_handle = FindFirstFile(full, &file);
	if (reinterpret_cast<bint>(search_handle) != -1)
	{
		do
		{
			if (file.dwFileAttributes & 32)
			{
				if (!findFile)
					continue;
			}
			else
			{
				if (findFile)
					continue;
			}
			if (file.cFileName[0] == TEXT('.'))
			{
				if (file.cFileName[1] == 0 || ((file.cFileName[1] == TEXT('.') && file.cFileName[2] == 0)))
				{
						continue;
				}
			}
			if (full_path)
			{
				BearStringPath item;
				BearString::Copy(item, path);
				BearString::Contact(item, BEAR_PATH);
				BearString::Contact(item, file.cFileName);
				list.push_back(item);
			}
			else
			{
				BearStringPath item;
				BearString::Copy(item, file.cFileName);
				list.push_back(item);
			}
		} while (FindNextFile(search_handle, &file));

	}
	FindClose(search_handle);
}

bool BearFileManager::FileExists(const bchar * name)
{

#ifdef UNICODE
	struct _stat buffer;
	int exist = _wstat(name, &buffer);
#else
#ifdef MSVC
	struct _stat buffer;
	int exist = _stat(name, &buffer);
#else
	struct _stat buffer;
	int exist = _stat((const char*)name, &buffer);
#endif
#endif
	if (exist == 0)
		return true;
	else // -1
		return false;
}



bool BearFileManager::FileDelete(const bchar * name)
{
#ifdef UNICODE
	return _wremove(name) != -1;
#else
	return remove(name) != -1;
#endif
}

bsize BearFileManager::GetFileSize(const bchar * name)
{

	FILE*file = 0;
#ifdef MSVC
#ifdef UNICODE
	_wfopen_s(&file, name, TEXT("rb"));
#else
	fopen_s(&file, name, "rb");
#endif
#else
#ifdef UNICODE
	file = fopen (*BearEncoding::FastToAnsi(name), "rb");
#else
	 file = fopen(name, "rb");
#endif
#endif
	if (!file)
		return 0;
	fseek(file, 0, SEEK_END);
	int64 size = ftell(file);
	fclose(file);
	return static_cast<bsize>(size);
}


BearFileManager::FileTime BearFileManager::GetFileCreateTime(const bchar * file)
{
	BearFileManager::FileTime ft;
	HANDLE fH;
	FILETIME creationTime;
	SYSTEMTIME sysTime;
	fH = CreateFile(file, GENERIC_READ, 0, 0, OPEN_EXISTING, 0, 0);
	if (fH != INVALID_HANDLE_VALUE)
	{
		GetFileTime(fH, &creationTime, 0, 0);
		FileTimeToSystemTime(&creationTime, &sysTime);
		ft.Year = sysTime.wYear;
		ft.Month = sysTime.wMonth;
		ft.Day = sysTime.wDay;
		ft.Hour = sysTime.wHour;
		ft.Minute = sysTime.wMinute;
		ft.Second = sysTime.wSecond;
		CloseHandle(fH);
	}
	return ft;
}

BearFileManager::FileTime BearFileManager::GetFileLastWriteTime(const bchar * file)
{
	BearFileManager::FileTime ft;
	HANDLE fH;
	FILETIME creationTime;
	SYSTEMTIME sysTime;
	fH = CreateFile(file, GENERIC_READ, 0, 0, OPEN_EXISTING, 0, 0);
	if (fH != INVALID_HANDLE_VALUE)
	{
		GetFileTime(fH, 0, 0, &creationTime);
		FileTimeToSystemTime(&creationTime, &sysTime);
		ft.Year = sysTime.wYear;
		ft.Month = sysTime.wMonth ;
		ft.Day = sysTime.wDay;
		ft.Hour = sysTime.wHour;
		ft.Minute = sysTime.wMinute;
		ft.Second = sysTime.wSecond;
		CloseHandle(fH);
	}
	return ft;
}
bool BearFileManager::FileMove(const bchar * name, const bchar * newname)
{
#ifdef UNICODE
	return _wrename(name, newname) != -1;
#else
	return rename(name, newname) != -1;
#endif
}