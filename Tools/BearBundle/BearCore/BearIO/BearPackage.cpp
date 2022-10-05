#include "BearCore.hpp"
#define BearPackageVersion 0x00000003
BearPackage::BearPackage()
{
}
BearPackage::~BearPackage()
{
}
void BearPackage::Compressor(const bchar * file_out, const bchar*path_to_files, const bchar*pathfs_for_archive, const bchar*path_for_archive, BearVector<BearString>& file_list)
{

	BearFileStream Fout;

	BEAR_CHECK(Fout.Open(file_out, BearFileStream::M_Write));

	Fout.WriteInt32(BearPackageVersion);
	Fout.WriteString(pathfs_for_archive, BearEncoding::UTF16);
	Fout.WriteUint64(static_cast<uint64>(file_list.size()));
	BearVector<bsize> Tell_FileSize;
	{
		auto b = file_list.begin();
		auto e = file_list.end();
		while (b != e)
		{
			BearStringPath SavePathToFile,PathToFile;
			SavePathToFile[0] = 0; PathToFile[0] = 0;
			BearFileManager::PathCombine(PathToFile, path_to_files, **b);
			BearFileManager::PathCombine(SavePathToFile, path_for_archive, **b);
			BearString::ToLower(SavePathToFile);
			Fout.WriteString(SavePathToFile, BearEncoding::UTF16);
		
			Fout.WriteUint64(static_cast<uint64>(BearFileManager::GetFileSize(PathToFile)));
			Tell_FileSize.push_back(Fout.Tell());
			Fout.WriteUint64(0);
			Fout.WriteInt32(File::CT_None);
			Fout.WriteUint64(0);
			b++;
		}
	}
	for (bsize i = 0; i < file_list.size(); i++)
	{
		bsize file_tell = Fout.Tell();
		BearStringPath  PathToFile; PathToFile[0] = 0;
		BearFileManager::PathCombine(PathToFile, path_to_files, *file_list[i]);
		Fout.Seek(Tell_FileSize[i]);
		Fout.WriteUint64(file_tell);
		Fout.Seek(file_tell);
		BearFileStream Fin;
		Fin.Open(PathToFile, BearFileStream::M_Read);
		char buffer[64 * 1024];
		while (!Fin.Eof())
		{
			Fout.WriteBuffer(buffer, Fin.ReadBuffer(buffer, 64 * 1024));
		}
	}
}

bool BearPackage::LoadFromFile(const bchar * file)
{
	m_files.clear_not_free();
	BearFileStream f;
	if (!f.Open(file))
		return false;
	if (f.ReadInt32() != BearPackageVersion)return false;
	f.ReadString(m_fspath);
	bsize  count =static_cast<bsize>( f.ReadUint64());
	for (bsize i = 0; i < count; i++)
	{
		BearString str;
		f.ReadString(str);
		auto&item =m_files[*str];
		item.Path = str;
		item.Size = static_cast<bsize>(f.ReadUint64());
		item.Ptr = static_cast<bsize>(f.ReadUint64());
		item.Compessor =static_cast<File::CompressorType>( f.ReadInt32());
		item.SizeCompressor = static_cast<bsize>(f.ReadUint64());
	}
	m_file = file;
	return true;
}

bool BearPackage::ExistFile(const bchar * file)
{
	BearStringPath path;
	BearString::Copy(path, file);
	BearFileManager::PathOptimization(path);
	return m_files.find(BearStringConteniar(file))!=m_files.end();
}

void BearPackage::GetFiles(BearVector<BearString>& files)
{
	auto begin = m_files.begin();
	auto end = m_files.end();
	while (begin != end)
	{
		BearString str_new = *begin->first;
		auto item2 = bear_lower_bound(files.begin(), files.end(), str_new);
		if (item2 == files.end() || *item2 != str_new)
		{
			files.insert(bear_lower_bound(files.begin(), files.end(), *begin->first), str_new);
		}
		begin++;
	}
}

void BearPackage::GetFiles(BearVector<BearString>&files, const bchar*e, bool subpath)
{
	BearStringPath path;
	e = BearString::ReadTo(e, TEXT('*'), path);
	bsize es = BearString::GetSize(e);
	auto begin = m_files.begin();
	auto end = m_files.end();
	while (begin != end)
	{
		bsize name_size = begin->first.size();

		
		if (e)
		{
			if (BearString::ExistPossition(*begin->first, 0, path) )
			{
				if (!subpath&&BearString::ToChar((*begin->first + BearString::GetSize(path)), BEAR_PATH[0]))
				{
					begin++; continue;
				}
				if (BearString::Compare((*begin->first) + name_size - es, e) == 0)
				{
					BearString str_new = *begin->first +BearString::GetSize(path);
					auto item2 = bear_lower_bound(files.begin(), files.end(), str_new);
					if (item2 == files.end() || *item2 != str_new)
					{
						files.insert(bear_lower_bound(files.begin(), files.end(), *begin->first), str_new);
					}
				}

			}
		}
		else if (BearString::Compare((*begin->first) + name_size - es, path)==0)
		{
			auto item2 = bear_lower_bound(files.begin(), files.end(), *begin->first);
			if (item2 == files.end() || *item2 != *begin->first)
			{
				files.insert(bear_lower_bound(files.begin(), files.end(), *begin->first), *begin->first);
			}
		}
		begin++;
	}
}

void BearPackage::GetDirectories(BearVector<BearString>& dirs, const bchar * e)
{
	bsize es = BearString::GetSize(e);
	auto begin = m_files.begin();
	auto end = m_files.end();
	while (begin != end)
	{
		if (BearString::ExistPossition(*begin->first, 0, e))
		{
			const bchar*path = *begin->first+es+1;
			if (BearString::CountElement(path, BEAR_PATH[0]) == 1)
			{
				BearStringPath dir;
				BearString::ReadTo(path, BEAR_PATH[0], dir);
				auto item2 = bear_lower_bound(dirs.begin(), dirs.end(), dir);
				if (item2 == dirs.end() || *item2 != dir)
				{
					dirs.insert(bear_lower_bound(dirs.begin(), dirs.end(), dir), dir);
				}
			}
		}
		begin++;
	}
}

BearFilePackageStream BearPackage::FileOpen(const bchar * file)
{
	BearStringPath path;
	BearString::Copy(path, file);
	BearFileManager::PathOptimization(path);
	auto item = m_files.find(BearStringConteniar(file));
	if (item != m_files.end())
	{
		BearFilePackageStream f;
		f.m_file_name = m_file;
		f.m_ptr = item->second.Ptr;
		f.m_size = item->second.Size;
		f.OpenFile();
		return f;
	}
	return BearFilePackageStream();
}

void BearPackage::Clear()
{
	m_file.clear();
	m_files.clear();
}

BearPackage::BearPackage(const BearPackage & package)
{
	Copy(package);
}

BearPackage::BearPackage(BearPackage && package)
{
	Swap(package);
}

void BearPackage::Swap(BearPackage & package)
{
	m_fspath.swap(package.m_fspath);
	m_file.swap(package.m_file);
	m_files.swap(package.m_files);
}

void BearPackage::Copy(const BearPackage & package)
{
	m_fspath = package.m_fspath;
	m_file = package.m_file;
	m_files = package.m_files;
}

BearPackage & BearPackage::operator=(const BearPackage & package)
{
	Copy(package);
	return*this;
	// TODO: вставьте здесь оператор return
}

BearPackage & BearPackage::operator=(BearPackage && package)
{
	Swap(package);
	return*this;
	// TODO: вставьте здесь оператор return
}
