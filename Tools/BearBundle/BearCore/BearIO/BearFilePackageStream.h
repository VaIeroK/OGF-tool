#pragma once
class BEARTOOL_API BearFilePackageStream :public BearInputStream
{
public:
	friend class BearPackage;
	BearFilePackageStream();
	virtual ~BearFilePackageStream();

	BearFilePackageStream(const BearFilePackageStream&right);
	BearFilePackageStream(BearFilePackageStream&&right);
	void Copy(const BearFilePackageStream&right);
	void Swap(BearFilePackageStream&right);
	BearFilePackageStream&operator=(BearFilePackageStream&&right);
	BearFilePackageStream&operator=(const BearFilePackageStream&right);

	virtual bool Eof() const;
	virtual bsize Seek(bsize tell) const;
	virtual bsize Tell()const;
	virtual bsize Size()const;
	void Close();
	virtual BearRef<BearInputStream> ReadChunkAsInputStream(uint32 type)const;
	virtual bsize ReadBuffer(void*data, bsize size) const;
	
private:
	void OpenFile();
	mutable bsize m_tell;
	bsize m_ptr;
	BearFileStream m_file;
	BearString m_file_name;
	bsize m_size;
};
