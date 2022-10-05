#include "BearCore.hpp"
#ifdef WINDOWS
#include <sys/stat.h>
#include <io.h>
#include "Windows/BearFileManager_Windows.h"
#elif UNIX
#include "Unix/BearFileManager_Unix.h"
#endif


bool BearFileManager::DirectoryDelete(const bchar * name, bool deleteall)
{
	return false;
}

bool BearFileManager::DirectoryRename(const bchar * path, const bchar * newpath)
{
	return false;
}

bool BearFileManager::DirectoryMove(const bchar * path, const bchar * newpath)
{
	return false;
}

BearString BearFileManager::GetFileNameAndExtension(const bchar * FullPathAndFile)
{
	BearString temp(FullPathAndFile);
	temp.seek(0);
	if (temp.to_char_with_end(BEAR_PATH[0]))
		temp++;
	else
		temp.seek(0);
	return BearString(*temp);
}

BearString BearFileManager::GetFileName(const bchar * FullPathAndFile)
{
	BearString temp(FullPathAndFile);
	if (temp.to_char_with_end(TEXT(".")[0]))
		**temp = 0;
	temp.seek(0);
	if (temp.to_char_with_end(BEAR_PATH[0]))
		temp++;
	else
		temp.seek(0);
	return 	BearString(*temp);
}

BearString BearFileManager::GetPathFile(const bchar * FullPathAndFile)
{
	BearString temp(FullPathAndFile);
	if (temp.to_char_with_end(BEAR_PATH[0]))
		**temp = 0;
	else
		return TEXT("");
	temp.seek(0);
	return BearString(*temp);
}



void BearFileManager::PathOptimization(bchar * in)
{
	{
		bchar* b = in;
		while (*b)
		{
#ifdef WINDOWS
			if (*b == TEXT("/")[0])
#else
			if (*b == TEXT("\\")[0])
#endif
				* b = BEAR_PATH[0];
			b++;
		}
	}
	bchar*path = in;
	bchar*path_lost = in;
	bchar* b = in;
	bchar* e = in+BearString::GetSize(in);
	{
		const bchar*shift = b;
		while (*shift == BEAR_PATH[0])
		{
			shift++;
		}
		if (shift - b )
		{
			bear_move(b , shift, BearString::GetSize(shift) + 1);
		}
	}
	while (b != e)
	{


		if (*b == BEAR_PATH[0])
		{

			const bchar*shift = b;
			while (*shift == BEAR_PATH[0])
			{
				shift++;
			}
			if (shift - b > 1)
			{
				bear_move(b + 1, shift, BearString::GetSize(shift) + 1);
				path = in;
				b = in;
				e = in + BearString::GetSize(in);
				continue;
			}

			path_lost = path;
			path = b;

		}
		if (b[0] == TEXT(".")[0] && b[1] == TEXT(".")[0] && b[2]!= TEXT(".")[0] && path != in)
		{
			if (path_lost[0] == BEAR_PATH[0])
			{
				if (bear_compare(&path_lost[1], TEXT(".."), 2) == 0 && path_lost[3] != TEXT(".")[0]) {

					b++;
					continue;
				}
			}
			else
			{
				if (bear_compare(&path_lost[0], TEXT(".."), 2) == 0 && path_lost[2] != TEXT(".")[0]) {

					b++;
					continue;
				}
			}

			b += 2;
			bear_move(path_lost, b, BearString::GetSize(b) + 1);
			path = in;
			path_lost = in;
			b = in;
			e = in + BearString::GetSize(in);
			continue;
		}
		b++;
	}

	{
		e = in + BearString::GetSize(in) - 1;
		while (b != e && *e == BEAR_PATH[0])
		{
			e[0] = 0;
			e--;
		}
	}
}

static void FindDirectory(BearVector<BearString>& list, const bchar * path, const bchar * curent_path, const bchar* fileextension)
{
	BearVector<BearString> temp;
	BearStringPath path_temp;
	BearString::Copy(path_temp, path);
	if (curent_path&&*curent_path)
	{
		BearString::Contact(path_temp, BEAR_PATH);
		BearString::Contact(path_temp, curent_path);
	}



	Find(temp, path_temp, fileextension, false, false);

	auto begin = temp.begin();
	auto end = temp.end();
	while (begin != end)
	{
		path_temp[0] = 0;
		if (curent_path&&*curent_path)
		{
			BearString::Copy(path_temp, curent_path);
			BearString::Contact(path_temp, BEAR_PATH);
		}

		BearString::Contact(path_temp, **begin);
		begin->assign(path_temp);

		FindDirectory(list, path, path_temp, fileextension);
		begin++;
	}
	list.insert(list.end(), temp.begin(), temp.end());
}
void FindDirectory(BearVector<BearString>& list, const bchar * path, const bchar* fileextension)
{
	BearVector<BearString> temp;
	BearStringPath path_temp;
	BearString::Copy(path_temp, path);
	 
	Find(temp, path, fileextension, true, false);
	auto begin = temp.begin();
	auto end = temp.end();
	while (begin != end)
	{
		FindDirectory(list, **begin, fileextension);

		begin++;
	}

	if(temp.size())
	
	list.insert(list.end(), temp.begin(), temp.end());
}

void BearFileManager::FindFiles(BearVector<BearString>& list, const bchar * path, const bchar * fileextension, bool allpath, bool all)
{
	Find(list, path, fileextension, allpath);
	if (all)
	{
		if (allpath)
		{
			BearVector<BearString> path_list;
			FindDirectories(path_list, path, TEXT("*"), true, true);
			auto begin = path_list.begin(), end = path_list.end();
			while (begin != end)
			{
				Find(list, **begin, fileextension, allpath);
				begin++;

			}
		}
		else
		{
			BearVector<BearString> path_list, file_list;
			FindDirectories(path_list, path, TEXT("*"), false, true);

			auto begin = path_list.begin(), end = path_list.end();
			while (begin != end)
			{
				BearStringPath path_temp;
				BearString::Copy(path_temp, path);
				BearString::Contact(path_temp, BEAR_PATH);
				BearString::Contact(path_temp, **begin);

				Find(file_list, path_temp, fileextension, false);
				auto begin1 = file_list.begin(), end1 = file_list.end();
				while (begin1 != end1)
				{
					BearStringPath temp1;
					BearString::Copy(temp1, **begin);
					BearString::Contact(temp1, BEAR_PATH);
					BearString::Contact(temp1, **begin1);
					list.push_back(temp1);
					begin1++;
				}
				file_list.clear();
				begin++;
			}

		}
	}
}

void BearFileManager::FindDirectories(BearVector<BearString>& list, const bchar * path, const bchar * fileextension, bool allpath, bool all)
{
	if (!all)
		Find(list, path, fileextension, allpath, false);
	else if (allpath)
		FindDirectory(list, path, fileextension);
	else
		FindDirectory(list, path, TEXT(""), fileextension);
}

