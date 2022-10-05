#include "BearCore.hpp"

BearFilePackageStream::BearFilePackageStream():m_tell(0),m_ptr(0),m_size(0)
{

}

BearFilePackageStream::~BearFilePackageStream()
{
}

BearFilePackageStream::BearFilePackageStream(const BearFilePackageStream & right) :m_tell(0), m_ptr(0), m_size(0)
{
	Copy(right);
}

BearFilePackageStream::BearFilePackageStream(BearFilePackageStream && right) : m_tell(0), m_ptr(0), m_size(0)
{
	Swap(right);
}

void BearFilePackageStream::Copy(const BearFilePackageStream & right)
{
	m_file_name = right.m_file_name;
	m_ptr = right.m_ptr;
	m_tell = right.m_tell;
	m_size = right.m_size;
	OpenFile();
}

void BearFilePackageStream::Swap(BearFilePackageStream & right)
{
	m_file_name.swap(right.m_file_name);
	m_file.Swap(right.m_file);
	bear_swap(m_ptr , right.m_ptr);
	bear_swap(m_tell , right.m_tell);
	bear_swap(m_size, right.m_size);
}

BearFilePackageStream & BearFilePackageStream::operator=(BearFilePackageStream && right)
{
	Swap(right);
	return *this;
}

BearFilePackageStream & BearFilePackageStream::operator=(const BearFilePackageStream & right)
{
	Copy(right);
	return *this;
}

bool BearFilePackageStream::Eof() const
{
	return m_size+m_ptr==m_file.Tell();
}

bsize BearFilePackageStream::Seek(bsize tell) const
{
	if (tell > m_size)
		m_tell = m_size;
	else 
		m_tell = tell;
	if (m_size)m_file.Seek(m_ptr + m_tell);
	return m_tell;
}

bsize BearFilePackageStream::Tell() const
{
	return m_tell;
}

bsize BearFilePackageStream::Size() const
{
	return m_size;
}

void BearFilePackageStream::Close()
{
	m_file.Close();
	m_file_name.clear();
	m_ptr =0;
	m_tell =0;
	m_size = 0;
}

BearRef<BearInputStream> BearFilePackageStream::ReadChunkAsInputStream(uint32 type) const
{
	auto size = GoToChunk(type);
	if (!size)return BearRef<BearInputStream>();

	BearFilePackageStream *temp = bear_new<BearFilePackageStream>();
	*temp = *this;
	return temp;
}


bsize BearFilePackageStream::ReadBuffer(void * data, bsize size) const
{
	if (m_tell + size > m_size)size = m_size - m_tell;
	m_file.ReadBuffer(data, size);
	m_tell += size;
	return size;
}


void BearFilePackageStream::OpenFile()
{
	if (m_size == 0)return;
	BEAR_ASSERT(m_file.Open(*m_file_name));
	m_file.Seek(m_ptr + m_tell);
}
