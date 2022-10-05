#include "BearCore.hpp"
BEARTOOL_API BearIncluder GIncluderDefault;
BearIncluder::BearIncluder()
{
}

BearRef< BearInputStream> BearIncluder::OpenAsStream(const bchar * name)
{
	BearFileStream *File = bear_new<BearFileStream>();
	BEAR_ASSERT(File->Open(name));
	return BearRef<BearInputStream>(File);
}

BearRef< BearBufferedReader> BearIncluder::OpenAsBuffer(const bchar * name)
{
	BearFileStream File;
	BEAR_ASSERT(File.Open(name));
	return BearRef<BearBufferedReader>(bear_new<BearMemoryStream>(File));
}

BearIncluder::~BearIncluder()
{
}
