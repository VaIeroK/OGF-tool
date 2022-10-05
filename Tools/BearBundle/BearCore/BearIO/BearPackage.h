#pragma once

	
class BEARTOOL_API BearPackage
{
public:
	BearPackage();
	~BearPackage();
	static void Compressor(const bchar*file_out,const bchar*path_to_files, const bchar*pathfs_for_archive, const bchar*path_for_archive, BearVector<BearString>&file_list);
	bool LoadFromFile(const bchar*file);
	bool ExistFile(const bchar*file);
	void GetFiles(BearVector<BearString>&files);
	void GetFiles( BearVector<BearString>&files,const bchar*e,bool subpath);
	void GetDirectories(BearVector<BearString>&dirs, const bchar*path);
	BearFilePackageStream FileOpen(const bchar*file);
	void Clear();
	BearPackage(const BearPackage&package);
	BearPackage(BearPackage&&package);
	void Swap(BearPackage&package);
	void Copy(const BearPackage&package);
	BearPackage&operator=(const BearPackage&package);
	BearPackage&operator=(BearPackage&&package);
	inline const BearString&GetFS()const
	{
		return m_fspath;
	}
private:
	struct File
	{
		BearString Path;
		bsize Size;
		bsize Ptr;
		enum CompressorType
		{
			CT_None,
			CT_LZO,
			CT_LZIB,
		};
		CompressorType Compessor;
		bsize SizeCompressor;
	};
	BearString m_fspath;
	BearString m_file;
	BearMap<BearStringConteniar, File> m_files;
};
