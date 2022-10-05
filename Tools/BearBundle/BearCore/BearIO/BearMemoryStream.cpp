#include "BearCore.hpp"

BearMemoryStream::BearMemoryStream(void* data, bsize size, bool free): m_edit(false), m_free(free),m_tell(0), m_data(0), m_size(size)
{
	m_data = reinterpret_cast<uint8*>(data);
}

BearMemoryStream::BearMemoryStream(): m_edit(true), m_free(true), m_tell(0), m_data(0), m_size(0)
{
}

BearMemoryStream::~BearMemoryStream()
{
	Clear();
}

bool BearMemoryStream::Eof() const
{
	return m_tell == Size();
}

bsize BearMemoryStream::Seek(bsize tell) const
{
	if (Size() < 0)return 0;
	if (tell <= static_cast<bsize>(Size()))m_tell = tell;
	else m_tell = Size();
	return BearMemoryStream::Tell();
}

bsize BearMemoryStream::Tell() const
{
	return m_tell;
}

bsize BearMemoryStream::Size() const
{
	return m_size;
}

void BearMemoryStream::Resize(bsize size)
{
	if (!m_edit)return;
	m_data = bear_realloc(m_data, size);
	m_size = size;
	if (m_tell > size)m_tell = size;
}

void * BearMemoryStream::Begin()
{
	return m_data+m_tell;
}

const void * BearMemoryStream::Begin() const
{
	return m_data + m_tell;
}

void * BearMemoryStream::End()
{
	return m_data + m_size;
}

const void * BearMemoryStream::End() const
{
	return m_data + m_size;
}

void BearMemoryStream::Clear()
{

	if (m_free)
	{
		bear_free(m_data);
	}
	m_free = true;
	m_edit = true;
	m_tell = 0;
	
	m_data = 0;
	m_size = 0;
}

BearRef< BearBufferedReader> BearMemoryStream::ReadChunkAsBufferedReader(uint32 type) const
{
	auto size = GoToChunk(type);
	if (!size)return BearRef<BearBufferedReader>();
	BearMemoryStream *temp = bear_new<BearMemoryStream>();
	temp->Resize(size);
	ReadBuffer(temp->Begin(), size);
	return temp;
}

BearMemoryStream::BearMemoryStream(BearMemoryStream && stream) :m_edit(true), m_free(true), m_tell(0), m_data(0), m_size(0)
{
	Swap(stream);
}

void BearMemoryStream::Swap(BearMemoryStream & stream)
{
	bear_swap(m_tell, stream.m_tell);
	bear_swap(m_data, stream.m_data);
	bear_swap(m_size, stream.m_size);
}

void BearMemoryStream::Copy(const BearMemoryStream & right)
{
	Clear();
	WriteBuffer(right.m_data, right.m_size);
	m_tell = right.m_tell;
}

BearMemoryStream::BearMemoryStream(const BearMemoryStream & right) :m_edit(true), m_free(true),m_tell(0), m_data(0), m_size(0)
{
	Copy(right);
}

BearMemoryStream::BearMemoryStream(const BearInputStream & right) : m_edit(true), m_free(true), m_tell(0), m_data(0), m_size(0)
{
	if (right.Size() - right.Tell())
	{
		Resize(right.Size() - right.Tell());
		right.ReadBuffer(m_data, right.Size());
	}
}

BearMemoryStream & BearMemoryStream::operator=(const BearMemoryStream & right)
{
	Copy(right);
	return *this;
}

BearMemoryStream & BearMemoryStream::operator=(BearMemoryStream && right)
{
	Swap(right);
	return *this;
}


bsize BearMemoryStream::ReadBuffer(void * data, bsize size) const
{
	bear_copy(data, m_data + m_tell, size + m_tell>m_size ? m_size - m_tell : size);
	Seek(size + m_tell);
	return  size + m_tell > m_size ? m_size - m_tell : size;
}

bsize BearMemoryStream::WriteBuffer(const void * data, bsize  size)
{
	if (!m_edit)return 0;
	if (size + m_tell >m_size)Resize(bear_recommended_size(size + m_tell));
	bear_copy(m_data + m_tell, data, size);
	Seek(size + m_tell);
	return true;
}
