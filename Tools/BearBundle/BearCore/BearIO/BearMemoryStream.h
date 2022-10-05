#pragma once

class BEARTOOL_API BearMemoryStream :public BearBufferedReader, public BearOutputStream
{
public:
	BearMemoryStream(void*data,bsize size,bool free=false);
	BearMemoryStream();
	virtual ~BearMemoryStream();

	virtual bool Eof() const;
	virtual bsize Seek(bsize tell) const;
	virtual bsize Tell()const;
	virtual bsize Size()const;
	void Resize(bsize size);
	virtual void*Begin();
	virtual const void*Begin()const;
	virtual void*End();
	virtual const void*End()const;
	virtual void Clear();
	virtual BearRef<BearBufferedReader> ReadChunkAsBufferedReader(uint32 type)const;

	BearMemoryStream(BearMemoryStream&&right);
	void Swap(BearMemoryStream&right);
	void Copy(const BearMemoryStream&right);
	BearMemoryStream(const BearMemoryStream&right);
	BearMemoryStream(const BearInputStream&right);
	BearMemoryStream& operator=(const BearMemoryStream&right);
	BearMemoryStream& operator=(BearMemoryStream&&right);
	virtual bsize ReadBuffer(void*data, bsize size) const;
	virtual bsize WriteBuffer(const void*data, bsize size);
private:
	bool m_edit;
	bool m_free;

	mutable bsize m_tell;
	uint8* m_data;
	bsize m_size;
};
