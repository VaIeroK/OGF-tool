#pragma once
class BEARTOOL_API BearLZO
{
	BEAR_CLASS_STATIC(BearLZO);
public:
	static bsize Compressor(const void*in, bsize in_size_block, void*out,bsize out_size_block);
	static bsize Decompressor(const void*in, bsize in_size_block, void*out, bsize out_size_block);
	static bsize Compressor9(const void*in, bsize in_size_block, void*out, bsize out_size_block);
	static bsize Decompressor9(const void*in, bsize in_size_block, void*out, bsize out_size_block);
};

class BEARTOOL_API BearZLIB
{
	BEAR_CLASS_WITHOUT_COPY(BearZLIB);
public:

	enum ZLIBType
	{
		Z_Decompressor=0,
		Z_Best,
		Z_Fast,

	};

	BearZLIB(ZLIBType type);
	~BearZLIB();
	void SetIn(const void*in, bsize size);
	void SetOut(void*out, bsize size);
	bool IsInNull()const;
	bool IsOutNull()const;
	bool Run();
	bool Stop();
	bsize GetWriteSize()const;
private:
	ZLIBType m_type;
	void*m_ptr;
	void*m_buffer_out;

};