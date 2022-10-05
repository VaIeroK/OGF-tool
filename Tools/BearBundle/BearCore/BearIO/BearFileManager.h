#pragma once

class BEARTOOL_API BearFileManager 
{
public:
	static void FindFiles(BearVector<BearString> &list, const bchar*  path, const bchar* fileextension, bool allpath = true, bool subPath = true);
	static void FindDirectories(BearVector<BearString> &list, const bchar* path, const bchar* fileextension, bool allpath = true, bool subPath = true);
	static bool FileExists(const bchar* name);
	static bool FileDelete(const bchar* name);
	static bool DirectoryCreate(const bchar* path);
	static bool DirectoryExists(const bchar* name);
	static bool DirectoryDelete(const bchar* name, bool deleteall);
	static bool DirectoryRename(const bchar*path, const bchar* newpath);
	static bool DirectoryMove(const bchar*path, const bchar* newpath);
	static bool FileCopy(const bchar* file, const bchar*newfile);
	static void PathOptimization(bchar*in);

	template<bsize sizeBuffer>
	static inline void PathCombine(bchar(&in)[sizeBuffer], const bchar*str)
	{
		BearString::Contact(in, BEAR_PATH);
		BearString::Contact(in, str);
		PathOptimization(in);
	}
	template<bsize sizeBuffer,  class...A>
	static inline void PathCombine(bchar(&in)[sizeBuffer], const bchar*str,const A*...a)
	{
		PathCombine(in, str);
		PathCombine(in,a...);
	}
	static bsize GetFileSize(const bchar*name);
	static void GetWorkPath(BearStringPath&path);
	static void GetApplicationPath(BearStringPath&path);
	struct FileTime
	{
		FileTime() :Year(0), Month(0), Day(0), Hour(0), Minute(0), Second(0) {}
		bsize Year;
		bsize Month;
		bsize Day;
		bsize Hour;
		bsize Minute;
		bsize Second;
		inline bool operator<(const FileTime&time)const
		{
			if (Year != time.Year)
				return Year < time.Year;
			else if (Month != time.Month)
				return Month < time.Month;
			else if (Day != time.Day)
				return Day < time.Day;
			else if (Hour != time.Hour)
				return Hour < time.Hour;
			else if (Minute != time.Minute)
				return Minute < time.Minute;
			return Second < time.Second;
		}
		inline bool operator>(const FileTime&time)const
		{
			if (Year != time.Year)
				return Year > time.Year;
			else if (Month != time.Month)
				return Month > time.Month;
			else if (Day != time.Day)
				return Day > time.Day;
			else if (Hour != time.Hour)
				return Hour > time.Hour;
			else if (Minute != time.Minute)
				return Minute > time.Minute;
			return Second > time.Second;
		}

		inline bool operator==(const FileTime&time)const
		{
			if (Year == time.Year&&Month == time.Month&&Day == time.Day&&Hour == time.Hour&&Minute == time.Minute&&Second == time.Second)
				return true;
			return false;
		}
		inline bool operator>=(const FileTime&time)const
		{
			return !this->operator<(time);
		}
		inline bool operator<=(const FileTime&time)const
		{
			return !this->operator>(time);
		}
		inline bool operator!=(const FileTime&time)const
		{
			return !this->operator==(time);;
		}
	};
	static FileTime GetFileCreateTime(const bchar* file);
	static FileTime GetFileLastWriteTime(const bchar* file);
	static BearString GetFileNameAndExtension(const bchar*FullPathAndFile);
	static BearString GetFileName(const bchar*FullPathAndFile);
	static BearString GetPathFile(const bchar* FullPathAndFile);
	static bool FileMove(const bchar* name, const bchar* newname);
};