#include "BearCore.hpp"
#ifdef WINDOWS
BEARTOOL_API BearMap<int32, int32>* LToWinowsKey=0;
BEARTOOL_API BearMap<int32, int32> *GFromWinowsKey=0;
static void RegisterWindowsKey(int32 win, BearInput::Key key)
{
	LToWinowsKey->insert(key, win);
	GFromWinowsKey->insert(win, key);
}
void GKeyWindowsDestroy()
{
	bear_delete(LToWinowsKey);
	bear_delete(GFromWinowsKey);
}
void GKeyWindowsInitialize()
{
	LToWinowsKey = bear_new<BearMap<int32, int32>>();
	GFromWinowsKey = bear_new<BearMap<int32, int32>>();
	RegisterWindowsKey('A', BearInput::KeyA);
	RegisterWindowsKey('B', BearInput::KeyB);
	RegisterWindowsKey('C', BearInput::KeyC);
	RegisterWindowsKey('D', BearInput::KeyD);
	RegisterWindowsKey('E', BearInput::KeyE);
	RegisterWindowsKey('F', BearInput::KeyF);
	RegisterWindowsKey('G', BearInput::KeyG);
	RegisterWindowsKey('H', BearInput::KeyH);
	RegisterWindowsKey('I', BearInput::KeyI);
	RegisterWindowsKey('J', BearInput::KeyJ);
	RegisterWindowsKey('K', BearInput::KeyK);
	RegisterWindowsKey('L', BearInput::KeyL);
	RegisterWindowsKey('M', BearInput::KeyM);
	RegisterWindowsKey('N', BearInput::KeyN);
	RegisterWindowsKey('O', BearInput::KeyO);
	RegisterWindowsKey('P', BearInput::KeyP);
	RegisterWindowsKey('Q', BearInput::KeyQ);
	RegisterWindowsKey('R', BearInput::KeyR);
	RegisterWindowsKey('S', BearInput::KeyS);
	RegisterWindowsKey('T', BearInput::KeyT);
	RegisterWindowsKey('U', BearInput::KeyU);
	RegisterWindowsKey('V', BearInput::KeyV);
	RegisterWindowsKey('W', BearInput::KeyW);
	RegisterWindowsKey('X', BearInput::KeyX);
	RegisterWindowsKey('Y', BearInput::KeyY);
	RegisterWindowsKey('Z', BearInput::KeyZ);
	RegisterWindowsKey('0', BearInput::KeyNum0);
	RegisterWindowsKey('1', BearInput::KeyNum1);
	RegisterWindowsKey('2', BearInput::KeyNum2);
	RegisterWindowsKey('3', BearInput::KeyNum3);
	RegisterWindowsKey('4', BearInput::KeyNum4);
	RegisterWindowsKey('5', BearInput::KeyNum5);
	RegisterWindowsKey('6', BearInput::KeyNum6);
	RegisterWindowsKey('7', BearInput::KeyNum7);
	RegisterWindowsKey('8', BearInput::KeyNum8);
	RegisterWindowsKey('9', BearInput::KeyNum9);
	RegisterWindowsKey(VK_ESCAPE, BearInput::KeyEscape);
	RegisterWindowsKey(VK_LCONTROL, BearInput::KeyLControl);
	RegisterWindowsKey(VK_LSHIFT, BearInput::KeyLShift);
	RegisterWindowsKey(VK_LMENU, BearInput::KeyLAlt);
	RegisterWindowsKey(VK_LWIN, BearInput::KeyLSystem);
	RegisterWindowsKey(VK_RCONTROL, BearInput::KeyRControl);
	RegisterWindowsKey(VK_RSHIFT, BearInput::KeyRShift);
	RegisterWindowsKey(VK_RMENU, BearInput::KeyRAlt);
	RegisterWindowsKey(VK_RWIN, BearInput::KeyRSystem);
	RegisterWindowsKey(VK_APPS, BearInput::KeyMenu);
	RegisterWindowsKey(VK_OEM_4, BearInput::KeyLBracket);
	RegisterWindowsKey(VK_OEM_6, BearInput::KeyRBracket);
	RegisterWindowsKey(VK_OEM_1, BearInput::KeySemiColon);
	RegisterWindowsKey(VK_OEM_COMMA, BearInput::KeyComma);
	RegisterWindowsKey(VK_OEM_PERIOD, BearInput::KeyPeriod);
	RegisterWindowsKey(VK_OEM_7, BearInput::KeyQuote);
	RegisterWindowsKey(VK_OEM_2, BearInput::KeySlash);
	RegisterWindowsKey(VK_OEM_5, BearInput::KeyBackSlash);
	RegisterWindowsKey(VK_OEM_3, BearInput::KeyTilde);
	RegisterWindowsKey(VK_OEM_PLUS, BearInput::KeyEqual);
	RegisterWindowsKey(VK_OEM_MINUS, BearInput::KeyDash);
	RegisterWindowsKey(VK_SPACE, BearInput::KeySpace);
	RegisterWindowsKey(VK_RETURN, BearInput::KeyReturn);
	RegisterWindowsKey(VK_BACK, BearInput::KeyBackSpace);
	RegisterWindowsKey(VK_TAB, BearInput::KeyTab);
	RegisterWindowsKey(VK_PRIOR, BearInput::KeyPageUp);
	RegisterWindowsKey(VK_NEXT, BearInput::KeyPageDown);
	RegisterWindowsKey(VK_END, BearInput::KeyEnd);
	RegisterWindowsKey(VK_HOME, BearInput::KeyHome);
	RegisterWindowsKey(VK_INSERT, BearInput::KeyInsert);
	RegisterWindowsKey(VK_DELETE, BearInput::KeyDelete);
	RegisterWindowsKey(VK_ADD, BearInput::KeyAdd);
	RegisterWindowsKey(VK_SUBTRACT, BearInput::KeySubtract);
	RegisterWindowsKey(VK_MULTIPLY, BearInput::KeyMultiply);
	RegisterWindowsKey(VK_DIVIDE, BearInput::KeyDivide);
	RegisterWindowsKey(VK_LEFT, BearInput::KeyLeft);
	RegisterWindowsKey(VK_RIGHT, BearInput::KeyRight);
	RegisterWindowsKey(VK_UP, BearInput::KeyUp);
	RegisterWindowsKey(VK_DOWN, BearInput::KeyDown);
	RegisterWindowsKey(VK_NUMPAD0, BearInput::KeyNumpad0);
	RegisterWindowsKey(VK_NUMPAD1, BearInput::KeyNumpad1);
	RegisterWindowsKey(VK_NUMPAD2, BearInput::KeyNumpad2);
	RegisterWindowsKey(VK_NUMPAD3, BearInput::KeyNumpad3);
	RegisterWindowsKey(VK_NUMPAD4, BearInput::KeyNumpad4);
	RegisterWindowsKey(VK_NUMPAD5, BearInput::KeyNumpad5);
	RegisterWindowsKey(VK_NUMPAD6, BearInput::KeyNumpad6);
	RegisterWindowsKey(VK_NUMPAD7, BearInput::KeyNumpad7);
	RegisterWindowsKey(VK_NUMPAD8, BearInput::KeyNumpad8);
	RegisterWindowsKey(VK_NUMPAD9, BearInput::KeyNumpad9);
	RegisterWindowsKey(VK_F1, BearInput::KeyF1);
	RegisterWindowsKey(VK_F2, BearInput::KeyF2);
	RegisterWindowsKey(VK_F3, BearInput::KeyF3);
	RegisterWindowsKey(VK_F4, BearInput::KeyF4);
	RegisterWindowsKey(VK_F5, BearInput::KeyF5);
	RegisterWindowsKey(VK_F6, BearInput::KeyF6);
	RegisterWindowsKey(VK_F7, BearInput::KeyF7);
	RegisterWindowsKey(VK_F8, BearInput::KeyF8);
	RegisterWindowsKey(VK_F9, BearInput::KeyF9);
	RegisterWindowsKey(VK_F10, BearInput::KeyF10);
	RegisterWindowsKey(VK_F11, BearInput::KeyF11);
	RegisterWindowsKey(VK_F12, BearInput::KeyF12);
	RegisterWindowsKey(VK_PAUSE, BearInput::KeyPause);
	RegisterWindowsKey(VK_LBUTTON, BearInput::KeyMouseLeft);
	RegisterWindowsKey(VK_RBUTTON, BearInput::KeyMouseRight);
	RegisterWindowsKey(VK_MBUTTON, BearInput::KeyMouseMiddle);
}


#endif
bool BearInput::KeyState(Key key)
{
#ifdef WINDOWS
	auto item = LToWinowsKey->find(key);
	if (item != LToWinowsKey->end())
		return ::GetKeyState(item->second) & 0x8000;
#endif
	return false;
}

BearVector2<float> BearInput::GetMousePosition()
{
	POINT P;
	GetCursorPos(&P);
	return BearFVector2(static_cast<float>(P.x), static_cast<float>(P.y));
}

void BearInput::SetMousePosition(const BearVector2<float>& position)
{
	POINT point = { static_cast<LONG>(position.x),  static_cast<LONG>(position.y) };
	SetCursorPos(static_cast<LONG>(position.x), static_cast<LONG>(position.y));
}

void BearInput::SetCursor(Cursor type)
{
	LPTSTR Win32Cursor = IDC_ARROW;
	switch (type)
	{
	case BearInput::Cursor_None:
		::SetCursor(NULL);
		return;
		break;
	case BearInput::Cursor_Arrow:
		Win32Cursor = IDC_ARROW;
		break;
	case BearInput::Cursor_TextInput:
		Win32Cursor = IDC_IBEAM;
		break;
	case BearInput::Cursor_ResizeAll:
		Win32Cursor = IDC_SIZEALL;
		break;
	case BearInput::Cursor_ResizeEW:
		Win32Cursor = IDC_SIZEWE;
		break;
	case BearInput::Cursor_ResizeNS:
		Win32Cursor = IDC_SIZENS;
		break;
	case BearInput::Cursor_ResizeNESW:
		Win32Cursor = IDC_SIZENESW;
		break;
	case BearInput::Cursor_ResizeNWSE:
		Win32Cursor = IDC_SIZENWSE;
		break;
	case BearInput::Cursor_Hand:
		Win32Cursor = IDC_HAND;
		break;
	case BearInput::Cursor_NotAllowed:
		Win32Cursor = IDC_NO;
		break;
	default:
		Win32Cursor = IDC_ARROW;
		break;
	}
	::SetCursor(::LoadCursor(NULL, Win32Cursor));
}
