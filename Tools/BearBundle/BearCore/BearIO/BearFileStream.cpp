#include "BearCore.hpp"

BearFileStream::BearFileStream() : m_mode(0), m_file(0)
{
}

BearFileStream::BearFileStream(const bchar *name, BearFlags<uint8> mode) :  m_mode(0),m_file(0)
{
	Open(name, mode);
}

BearFileStream::~BearFileStream()
{
	Close();
}

bool BearFileStream::Open(const bchar *name, BearFlags<uint8> flags)
{
	Close();
	const bchar *mode = TEXT("");
	m_mode = flags;
	if (flags.test(M_Write))
		mode = TEXT("wb");
	if (flags.test(M_Read))
		mode = TEXT("rb");
	if (m_file)
		Close();
#ifdef MSVC
#ifdef UNICODE
	_wfopen_s(reinterpret_cast<FILE **>(&m_file), name, mode);
#else
	fopen_s(reinterpret_cast<FILE **>(&m_file), name, mode);
#endif
#else
#ifdef UNICODE
	m_file = fopen((const char*)*BearEncoding::FastToAnsi(name),(const char*)*BearEncoding::FastToAnsi(mode));
#else
	m_file = fopen(name,mode);
#endif

#endif
	if (m_file)
	{
		Seek(0);
		//BearString::Copy(m_name, name);
	}
	else
	{
		Close();
	}
	return m_file;
}

bool BearFileStream::Eof() const
{
	return Tell() == Size();
}

bsize BearFileStream::Seek(bsize tell1) const
{
	if (!m_file)
		return 0;
#ifdef WINDOWS
	_fseeki64((FILE *)m_file, static_cast<int64>(tell1), SEEK_SET);
#else
	fseek((FILE *)m_file, static_cast<long>(tell1), SEEK_SET);
#endif

	return Tell();
}

bsize BearFileStream::Tell() const
{
	if (m_file)
#ifdef WINDOWS
		return static_cast<bsize>(_ftelli64((FILE *)m_file));
#else
		return static_cast<bsize>(ftell((FILE *)m_file));
#endif
	else
		return 0;
}

bsize BearFileStream::Size() const
{
	if (!m_file)
		return 0;
	bsize pos = Tell();
	fseek((FILE *)m_file, 0, SEEK_END);
	bsize size_ = Tell();
#ifdef WINDOWS
		_fseeki64((FILE *)m_file, static_cast<int64>(pos), SEEK_SET);
#else
	fseek((FILE *)m_file, static_cast<long>(pos), SEEK_SET);
#endif

	return size_;
}

void BearFileStream::Close()
{
	if (m_file)
		fclose((FILE *)m_file);
	m_file = 0;
	m_mode = 0;
}

BearRef<BearInputStream> BearFileStream::ReadChunkAsInputStream(uint32 type) const
{
	auto size = GoToChunk(type);
	if (!size)
		return BearRef<BearInputStream>();

	BearMemoryStream *temp = bear_new<BearMemoryStream>();
	temp->Resize(size);
	temp->WriteBuffer(*this->Read(size), size);
	temp->Seek(0);
	return temp;
}

void BearFileStream::Swap(BearFileStream &right)
{
	bear_swap(right.m_file, m_file);
	bear_swap(right.m_mode, m_mode);
}



bsize BearFileStream::ReadBuffer(void *data, bsize size) const
{
	if (m_file && m_mode.test(M_Read))
		return	fread(data, size, 1, (FILE *)m_file);
	return 0;
}

bsize BearFileStream::WriteBuffer(const void *data, bsize size)
{
	if (m_file && m_mode.test(M_Write))
		return fwrite(data, 1, size, (FILE *)m_file);
	
	else
		return false;
}
