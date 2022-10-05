#include <wchar.h>
#include <unistd.h>
#include <dirent.h>
#define BUFFER_SIZE 64 * 1024
bool BearCore::BearFileManager::FileCopy(const bchar *in, const bchar *out)
{
	BearFileStream in_file;
	BearFileStream out_file;
	if (!in_file.Open(in))
		return false;
	if (!out_file.Open(out, out_file.M_Write))
		return false;
	char buffer[BUFFER_SIZE];
	while (!in_file.Eof())
	{
		bsize size = in_file.Read(buffer, BUFFER_SIZE);
		out_file.Write(buffer, size);
	}
	return true;
};
#include <sys/types.h>
#include <sys/stat.h>
bool BearCore::BearFileManager::DirectoryCreate(const bchar *path)
{
	return mkdir((const char*)*BearEncoding::ToUTF8(path), S_IRWXU | S_IRGRP | S_IXGRP | S_IROTH | S_IXOTH) == 0;
}
bool BearCore::BearFileManager::DirectoryExists(const bchar *name)
{
	struct stat buffer;
	int exist = stat((const char*)*BearEncoding::ToUTF8(name), &buffer);
	if (exist == 0)
		return S_ISDIR(buffer.st_mode);
	else // -1
		return false;
}
void BearCore::BearFileManager::GetWorkPath(BearStringPath &path)
{
#ifdef UNICODE
	BearStringUTF8Path path_ansi;
	getcwd((char*)path_ansi, MAX_PATH);
	BearString::Copy(path, *BearEncoding::ToUTF16(path_ansi));
#else
	getcwd((char*)path, MAX_PATH);
#endif
	PathOptimization(path);
}

void BearCore::BearFileManager::GetApplicationPath(BearStringPath &path)
{

	BearStringAnsiPath path_ansi;
#ifdef UNICODE
	auto length = readlink("/proc/self/exe", path_ansi, sizeof(path_ansi));
	if (length < 0)
		length = 0;
	path_ansi[length] = 0;
	BearString::Copy(path, *BearEncoding::ToUTF16(path_ansi));
#else
	auto length = readlink("/proc/self/exe", (char*)path, sizeof(path));
	if (length < 0)
		length = 0;
	path[length] = 0;
#endif
	BearString::ToCharWithEnd(path, BEAR_PATH[0])[0] = 0;
	PathOptimization(path);
}
bool BearCore::BearFileManager::FileExists(const bchar *name)
{
	struct stat buffer;
	int exist = stat((const char*)*BearEncoding::ToUTF8(name), &buffer);
	if (exist == 0)
		return !S_ISDIR(buffer.st_mode);
	else // -1
		return false;
}
bool BearCore::BearFileManager::FileDelete(const bchar *name)
{
	return remove((const char*)*BearEncoding::ToUTF8(name)) != -1;
}
bsize BearCore::BearFileManager::GetFileSize(const bchar *name)
{
	FILE *file = fopen((const char*)*BearEncoding::ToUTF8(name), "rb");
	if (!file)
		return 0;
	fseek(file, 0, SEEK_END);
	int64 size = ftell(file);
	fclose(file);
	return static_cast<bsize>(size);
}

BearCore::BearFileManager::FileTime BearCore::BearFileManager::GetFileCreateTime(const bchar *file)
{
	BearCore::BearFileManager::FileTime ft;
	struct stat buffer;
	int exist = stat((const char*)*BearEncoding::ToUTF8(file), &buffer);
	if (exist == 0)
	{
		struct tm *tm = localtime(&(buffer.st_ctime));
		ft.Year = tm->tm_year;
		ft.Month = tm->tm_mon;
		ft.Day = tm->tm_mday;
		ft.Hour = tm->tm_hour;
		ft.Minute = tm->tm_min;
		ft.Second = tm->tm_sec;
	}
	return ft;
}

BearCore::BearFileManager::FileTime BearCore::BearFileManager::GetFileLastWriteTime(const bchar *file)
{
	BearCore::BearFileManager::FileTime ft;
	struct stat buffer;
	int exist = stat((const char*)*BearEncoding::ToUTF8(file), &buffer);
	if (exist == 0)
	{
		struct tm *tm = localtime(&(buffer.st_mtime));
		ft.Year = tm->tm_year;
		ft.Month = tm->tm_mon;
		ft.Day = tm->tm_mday;
		ft.Hour = tm->tm_hour;
		ft.Minute = tm->tm_min;
		ft.Second = tm->tm_sec;
	}
	return ft;
}
bool BearCore::BearFileManager::FileMove(const bchar *name, const bchar *newname)
{
	return rename((const char*)*BearEncoding::ToUTF8(name), (const char*)*BearEncoding::ToUTF8(newname)) != -1;
}

static void Find(BearCore::BearVector<BearCore::BearString> &list, const bchar *path, const char *ext, bool full_path, bool findFile = true)
{
	DIR *dir;
	struct dirent *ent;
	BearCore::BearStringAnsiPath ext1;
	ext = BearCore::BearString::ReadTo(ext, '*', ext1);
	bsize es = BearCore::BearString::GetSize(ext);

	if ((dir = opendir((const char*)*BearCore::BearEncoding::ToUTF8(path))) != NULL)
	{
		while ((ent = readdir(dir)) != NULL)
		{
			bool is_dir=DT_DIR&ent->d_type;
			if (findFile!=is_dir)
			{
				bsize name_size = BearCore::BearString::GetSize(ent->d_name);
				if (ext)
				{
					if (BearCore::BearString::ExistPossition(ent->d_name, 0, ext1))
					{
						if (BearCore::BearString::Compare(ent->d_name + name_size - es, ext) == 0)
						{
							if (full_path)
							{
								BearCore::BearString str;
								str.append(path);
								str.append(BEAR_PATH);
								str.append(*BearCore::BearEncoding::ToCurrent((const char *)ent->d_name));
								BearCore::BearFileManager::PathOptimization(*str);
								list.push_back(str);
							}
							else
							{
								list.push_back(*BearCore::BearEncoding::ToCurrent((const char *)ent->d_name));
							}
						}
					}
				}
				else if (BearCore::BearString::Compare((ent->d_name) + name_size - es, ext1) == 0)
				{
					if (full_path)
					{
						BearCore::BearString str;
						str.append(path);
						str.append(BEAR_PATH);
						str.append(*BearCore::BearEncoding::ToCurrent((const char *)ent->d_name));
						BearCore::BearFileManager::PathOptimization(*str);
						list.push_back(str);
					}
					else
					{
						list.push_back(*BearCore::BearEncoding::ToCurrent((const char *)ent->d_name));
					}
				}
			}
		}
		closedir(dir);
	}
}
static void Find(BearCore::BearVector<BearCore::BearString> &list, const bchar *path, const bchar *ext, bool full_path, bool findFile = true)
{
	Find(list,path,(const char*)*BearCore::BearEncoding::ToUTF8(path),full_path,findFile);
}