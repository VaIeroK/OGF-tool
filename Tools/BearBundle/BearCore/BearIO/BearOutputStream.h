#pragma once
class BEARTOOL_API BearOutputStream
{
public:
	BearOutputStream();
	virtual ~BearOutputStream();
	void WriteUint8(uint8 data);
	void WriteUint16(uint16 data);
	void WriteUint32(uint32 data);
	void WriteUint64(uint64 data);
		
	void WriteInt8(int8 data);
	void WriteInt16(int16 data);
	void WriteInt32(int32 data);
	void WriteInt64(int64 data);
		
	void WriteFloat(float data);
	void WriteDouble(double data);
		
		
	void BeginChunk(int32 id);
	void EndChunk(); 
		
	void WriteString(const BearString&base,BearEncoding::Encoding encoding,bool info=true);
	void WriteStringWithZero(const BearString&base, BearEncoding::Encoding encoding);
	void WriteStringAndNewLine(const BearString &str, BearEncoding::Encoding encoding);
		
	virtual  bsize WriteBuffer(const void*data, bsize size) = 0;
	virtual bool Eof() const=0;
	virtual bsize Seek(bsize tell) const = 0;
	virtual bsize Tell()const = 0;
	virtual bsize Size()const = 0;
private:
	int64 m_tell_Chunk;

};