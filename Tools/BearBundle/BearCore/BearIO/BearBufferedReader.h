#pragma once

class BEARTOOL_API BearBufferedReader:public BearInputStream
{
public:
	virtual void*Begin() = 0;
	inline const void*Begin()const
	{
		return const_cast<BearBufferedReader*>(this)->Begin();
	}
	inline  const void*End()const
	{
		return const_cast<BearBufferedReader*>(this)->End();
	}
	virtual void*End()=0;
	void ReadStringToLine(BearString&str, BearEncoding::Encoding encoding)const;
	virtual BearRef<BearBufferedReader> ReadChunkAsBufferedReader(uint32 type)const = 0;
	void ReadStringToZero(BearString&str, BearEncoding::Encoding encoding)const;
	virtual BearRef<BearInputStream> ReadChunkAsInputStream(uint32 type)const;

};
