#pragma once
class BEARTOOL_API BearIncluder
{
	BEAR_CLASS_WITHOUT_COPY(BearIncluder);
public:
	BearIncluder();
	virtual BearRef<BearInputStream> OpenAsStream(const bchar*name);
	virtual BearRef<BearBufferedReader> OpenAsBuffer(const bchar*name);
	~BearIncluder();
		
};
extern BEARTOOL_API BearIncluder GIncluderDefault;
