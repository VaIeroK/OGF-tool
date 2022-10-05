#pragma once

class BEARTOOL_API BearInput
{
	BEAR_CLASS_STATIC(BearInput);
public:
	enum Key
	{ 
#define KEY(A) Key##A,
#include "..\BearIO\BearKeys.h"
		Key_Count,
	};
	
	static bool KeyState(Key key);
	static	BearVector2<float> GetMousePosition();
	static void SetMousePosition(const BearVector2<float>&position);
	enum Cursor
	{
		Cursor_None,
		Cursor_Arrow,
		Cursor_TextInput,
		Cursor_ResizeAll,
		Cursor_ResizeEW,
		Cursor_ResizeNS,
		Cursor_ResizeNESW,
		Cursor_ResizeNWSE,
		Cursor_Hand,
		Cursor_NotAllowed,
	};
	static void SetCursor(Cursor type);
};
