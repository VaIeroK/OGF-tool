#pragma once

class BEARTOOL_API BearFileSystem
{
public :
	BEAR_CLASS_WITHOUT_COPY(BearFileSystem);
	BearFileSystem();
	~BearFileSystem();

	void UpdatePath(const bchar*path, bsize id,BearStringPath&path_out);


	void Update(const bchar*path, const bchar*file, BearStringPath&path_out);
	void Update(const bchar*path, const bchar*file, const bchar*e, BearStringPath&path_out);
		
	bool ExistDirectory(const bchar*path);
	bool ExistDirectory(const bchar*path,const bchar*directory);
	bool ExistDirectory(const bchar*path,bsize id);
	bool ExistDirectoryAndUpdate(const bchar*path,bsize id, BearStringPath&path_out);
	bool ExistFileAndUpdate(const bchar*path, const bchar*file, BearStringPath&path_out);
	bool ExistFile(const bchar*path, const bchar*file);
	bool ExistFileAndUpdate(const bchar*path, const bchar*file, const bchar*e, BearStringPath&path_out);
	bool ExistFile(const bchar*path, const bchar*file, const bchar*e);
#ifdef WINDOWS
#undef CreateDirectory
#endif
	void CreateDirectory(const bchar*path, bsize id);

	bool LoadFromFile(const bchar*file,BearEncoding::Encoding type);
	void LoadFromStream(const BearInputStream&file, BearEncoding::Encoding type);
	void LoadFromBuffer(const BearBufferedReader&file, BearEncoding::Encoding type);
	void SetPackage(const bchar*fspath,const bchar*path);
	void ReadConfig(const bchar*path, const bchar*file, const bchar*e, BearINI&ini, BearEncoding::Encoding coding,BearIncluder*includer=&GIncluderDefault);
	void ReadConfig(const bchar*path, const bchar*file, BearINI&ini, BearEncoding::Encoding coding, BearIncluder*includer = &GIncluderDefault);

	void WriteConfig(const bchar*path, const bchar*file, const bchar*e, bsize id, const BearINI&ini, BearEncoding::Encoding coding);
	void WriteConfig(const bchar*path, const bchar*file, bsize id,const BearINI&ini, BearEncoding::Encoding coding);

	BearRef<BearInputStream> Read(const bchar*path, const bchar*file, const bchar*e);
	BearRef<BearInputStream> Read(const bchar*path, const bchar*file);

	BearRef<BearOutputStream> Write(const bchar*path, const bchar*file, bsize id);
	BearRef<BearOutputStream> Write(const bchar*path, const bchar*file, const bchar*e, bsize id);

	bool Delete(const bchar*path, const bchar*file);
	bool Delete(const bchar*path, const bchar*file, const bchar*e);

	void GetDirectories(BearVector<BearString>&files, const bchar*path);
	void GetFiles(BearVector<BearString>&files, const bchar*path, const bchar*e,bool subPath=false);

	void Clear();

	bsize CountPath(const bchar*Path);
	bool ExistPath(const bchar*path);
	void AppendPath(const bchar*name, const bchar*path, const bchar*parent_path, int32 priority);
	void SubPath(const bchar*name);
private:
	void decoder(BearINI&ini);
	void update(const bchar*name);
	BearMap< BearStringConteniar, BearVector<BearString>> m_paths;
	struct SourcePath
	{
		BearStringPath parent;
		BearStringPath path;
		int32 priority;
		bool operator<(const SourcePath&right)const { return priority > right.priority; }
	};
	void decoderPath(SourcePath&path, BearString&string);
	BearMap< BearStringConteniar, BearVector<SourcePath>> m_source_paths;
		
	void GetFilesFromPackage(BearVector<BearString>&files, const bchar*path, const bchar*e, bool subPath = false);
	void GetDirectoriesFromPackage(BearVector<BearString>&files, const bchar*path);
	BearVector<BearPackage> m_packages;

	bool RegisterFile(const bchar*full_path);
	void RegisterFiles();
	struct File
	{
		File() :package(0) {}
		BearString package_path;
		BearPackage*package;
	};
	BearMap<BearStringConteniar, File> m_files;
};
BEARTOOL_API extern BearFileSystem*GFS;
