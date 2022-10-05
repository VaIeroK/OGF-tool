#pragma once

class BEARTOOL_API BearFileStream :public BearInputStream, public BearOutputStream
{
public:
	BEAR_CLASS_WITHOUT_COPY(BearFileStream);
	enum Mode
	{
		M_Read = 1 << 0,
		M_Write = 1 << 1
	};
	BearFileStream();
	BearFileStream(const bchar* name, BearFlags<uint8> mode = M_Read);
	virtual ~BearFileStream();
	bool Open(const bchar* name, BearFlags<uint8> mode = M_Read);
	virtual bool Eof() const;
	virtual bsize Seek(bsize tell) const;
	virtual bsize Tell()const;
	virtual bsize Size()const;
	void Close();
	virtual BearRef<BearInputStream> ReadChunkAsInputStream(uint32 type)const;

	void Swap(BearFileStream&right);

	virtual bsize ReadBuffer(void*data, bsize size) const;
	virtual bsize WriteBuffer(const void*data, bsize size);
private:
	BearFlags<uint8> m_mode;
	mutable void* m_file;
};
