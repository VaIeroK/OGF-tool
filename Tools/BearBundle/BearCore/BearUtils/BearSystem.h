#pragma once
class BEARTOOL_API BearSystem
{
	BEAR_CLASS_STATIC(BearSystem);
public:
	
	 static void ToClipboard(const bchar8* text);
	 static void ToClipboard(const bchar16* text);
	 static void ToClipboard(const bchar8* text, bsize size);
	 static void ToClipboard(const bchar16* text, bsize size);
	 static BearString GetClipboard();
};
