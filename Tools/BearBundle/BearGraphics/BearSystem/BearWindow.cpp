#include "BearGraphics.hpp"
#ifdef WINDOWS
BEARTOOL_API BearMap<int32, int32>* LToWinowsKey;
BEARTOOL_API BearMap<int32, int32>* GFromWinowsKey;
static bsize LCount = 0;
LRESULT CALLBACK GlobalOnEvent(HWND handle, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_CLOSE:
		PostQuitMessage(0);
		break;
	case WM_CREATE:
		SetWindowLongPtrW(handle, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(reinterpret_cast<CREATESTRUCT*>(lParam)->lpCreateParams));
		break;
	}
	{
		auto viewport = reinterpret_cast<BearWindow*>(GetWindowLongPtrW(handle, GWLP_USERDATA));
		if (viewport)
		{
			viewport->OnEvent(handle, message, wParam, lParam);
		}
	}
	return DefWindowProc(handle, message, wParam, lParam);
}
LRESULT CALLBACK GlobalOnEventNoClosed(HWND handle, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_CLOSE:
		return 0;
		break;
	case WM_CREATE:
		SetWindowLongPtrW(handle, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(reinterpret_cast<CREATESTRUCT*>(lParam)->lpCreateParams));
		break;
	}
	{
		auto Viewport = reinterpret_cast<BearWindow*>(GetWindowLongPtrW(handle, GWLP_USERDATA));
		if (Viewport)
		{
			Viewport->OnEvent(handle, message, wParam, lParam);
		}
	}
	return DefWindowProc(handle, message, wParam, lParam);
}


static bool LBWindowsClass = false;
static bool LBWindowsClassNC = false;
static void RegisterWindowsClass(HINSTANCE hInstance, bool closed)
{
	if (closed)
	{
		if (LBWindowsClass)return;
		LBWindowsClass = true;
	}
	else
	{
		if (LBWindowsClassNC)return;
		LBWindowsClassNC = true;
	}


	WNDCLASSEX WndClass;
	WndClass.style = CS_HREDRAW | CS_VREDRAW;
	if (closed)
	{
		WndClass.lpfnWndProc = GlobalOnEvent;
	}
	else
	{
		WndClass.lpfnWndProc = GlobalOnEventNoClosed;
	}
	WndClass.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
	WndClass.cbClsExtra = 0;
	WndClass.cbWndExtra = 0;
	WndClass.hInstance = hInstance;
	WndClass.hIcon = LoadIcon(NULL, IDI_WINLOGO);
	WndClass.hIconSm = WndClass.hIcon;
	WndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
#ifdef DEBUG
	WndClass.hbrBackground = CreateSolidBrush(RGB(69, 22, 28));
#else
	WndClass.hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
#endif
	WndClass.lpszMenuName = NULL;
	if (closed)
	{
		WndClass.lpszClassName = TEXT("BEAR");
	}
	else
	{
		WndClass.lpszClassName = TEXT("BEARNC");
	}
	WndClass.cbSize = sizeof(WNDCLASSEX);
	RegisterClassEx(&WndClass);
}
BearWindow::BearWindow(bsize width, bsize height, bool fullscreen, BearFlags<int32> flags) :m_MouseShow(1),m_Width(width), m_Height(height), m_Fullscreen(false), m_MouseEnter(false)
{
	m_EventPtr = m_Events.end();
	HINSTANCE hInstance = (HINSTANCE)GetModuleHandle(0);

	RegisterWindowsClass(hInstance, !flags.test((int32)BearWindowType::WihtoutClosed));
	


	m_Style = WS_POPUP;
	if (!flags.test((int32)BearWindowType::Popup))
	{
		if (flags.test((int32)BearWindowType::OnlyClosed)) m_Style = WS_OVERLAPPED | WS_SYSMENU | WS_CLIPSIBLINGS | WS_CLIPCHILDREN | WS_DLGFRAME;
		else m_Style = WS_OVERLAPPEDWINDOW;
	}

	{
		RECT rectangle = { 0, 0, static_cast<long>(width), static_cast<long>(height) };

		if (!flags.test((int32)BearWindowType::WihtoutClosed))
		{
			m_WindowHandle = CreateWindowEx(0, TEXT("BEAR"), TEXT(""), m_Style, 0, 0, 1, 1, NULL, NULL, hInstance, this);
		}
		else
		{
			m_WindowHandle = CreateWindowEx(0, TEXT("BEARNC"), TEXT(""), m_Style, 0, 0, 1, 1, NULL, NULL, hInstance, this);
		}

		AdjustWindowRect(&rectangle, GetWindowLong((HWND)m_WindowHandle, GWL_STYLE), false);
		SetWindowPos((HWND)m_WindowHandle, NULL, 0, 0, rectangle.right - rectangle.left, rectangle.bottom - rectangle.top, SWP_NOMOVE | SWP_NOZORDER);
		
		uint32 XPosition = static_cast<int32>(((uint32)GetSystemMetrics(SM_CXSCREEN) / 2) - (width / 2));
		uint32 YPosition = static_cast<int32>(((uint32)GetSystemMetrics(SM_CYSCREEN) / 2) - (height / 2));

		SetWindowPos((HWND)m_WindowHandle, NULL, XPosition, YPosition, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
	}

	ShowWindow((HWND)m_WindowHandle, SW_SHOW);
	if (fullscreen)SetFullscreen(fullscreen);

}

BearWindow::~BearWindow()
{
	DestroyWindow(GetWindowHandle());

}

void BearWindow::Resize(bsize width, bsize height)
{
	m_Width = width;
	m_Height = height;

	if (!m_Fullscreen)
	{

		SetWindowLong((HWND)m_WindowHandle, GWL_STYLE, m_Style);
		//AdjustWindowRect(&rectangle, GetWindowLong((HWND)m_WindowHandle, GWL_STYLE), false);

		RECT			m_rcWindowBounds;
		RECT				DesktopRect;

		GetClientRect(GetDesktopWindow(), &DesktopRect);

		SetRect(&m_rcWindowBounds,
			(DesktopRect.right -static_cast<LONG>( m_Width)) / 2,
			(DesktopRect.bottom - static_cast<LONG>(m_Height)) / 2,
			(DesktopRect.right + static_cast<LONG>(m_Width)) / 2,
			(DesktopRect.bottom + static_cast<LONG>(m_Height)) / 2);

		AdjustWindowRect(&m_rcWindowBounds, m_Style, FALSE);

		SetWindowPos((HWND)m_WindowHandle,
			HWND_NOTOPMOST,
			m_rcWindowBounds.left,
			m_rcWindowBounds.top,
			(m_rcWindowBounds.right - m_rcWindowBounds.left),
			(m_rcWindowBounds.bottom - m_rcWindowBounds.top),
			SWP_SHOWWINDOW | SWP_NOCOPYBITS | SWP_DRAWFRAME);
	}
	else
	{
		SetWindowPos((HWND)m_WindowHandle, HWND_TOP, 0, 0, static_cast<int32>(m_Width), static_cast<int32>(m_Height), SWP_FRAMECHANGED);
		SetWindowLong((HWND)m_WindowHandle, GWL_STYLE, WS_EX_TOPMOST | WS_POPUP | WS_VISIBLE);
		ShowWindow((HWND)m_WindowHandle, SW_MAXIMIZE);
		SetWindowLong(m_WindowHandle, GWL_EXSTYLE, WS_EX_TOPMOST);

		SetForegroundWindow(m_WindowHandle);
	}
}
void BearWindow::SetFullscreen(bool fullscreen)
{
	m_Fullscreen = fullscreen;

	if (m_Fullscreen)
	{
		SetWindowLong((HWND)m_WindowHandle, GWL_STYLE, WS_POPUP | WS_VISIBLE);
		SetWindowLong((HWND)m_WindowHandle, GWL_EXSTYLE, WS_EX_TOPMOST);
		SetFocus(m_WindowHandle);


	}
	else
	{
		SetWindowLong((HWND)m_WindowHandle, GWL_STYLE, WS_DLGFRAME | WS_SYSMENU | WS_MINIMIZEBOX);

	}
	Resize(m_Width, m_Height);
	SetForegroundWindow((HWND)m_WindowHandle);
}

void BearWindow::BeginFrame()
{
	m_Events.clear_not_free();
	m_EventPtr = m_Events.end();
}
void BearWindow::EndFrame()
{
	m_EventPtr = m_Events.begin();
}
bool BearWindow::OnFrame()
{
	MSG Msg; Msg.message = WM_NULL;
	while (PeekMessageW(&Msg, NULL, 0, 0, PM_REMOVE))
	{
		if (Msg.message == WM_QUIT)
			return false;
		TranslateMessage(&Msg);
		DispatchMessageW(&Msg);
	}
	if (Msg.message == WM_QUIT)
		return false;
	return true;
}
void BearWindow::ShowCursor(bool show)
{
	if (m_MouseShow != show)
		::ShowCursor(show);
	m_MouseShow = show;
}

BearVector2<float> BearWindow::GetMousePosition()const
{
	POINT Point = {};
	GetCursorPos(&Point);
	if (!IsFullScreen())	ScreenToClient(GetWindowHandle(), &Point);
	return BearFVector2(static_cast<float>(Point.x), static_cast<float>(Point.y));
}

void BearWindow::SetMousePosition(const BearVector2<float>& position)
{
	POINT Point = { static_cast<LONG>(position.x),static_cast<LONG>(position.y) };
	if (!IsFullScreen())
		ClientToScreen(GetWindowHandle(), &Point);
	SetCursorPos(Point.x, Point.y);
}

bool BearWindow::GetEvent(BearEventWindows& e)
{

	if (m_EventPtr == m_Events.end())
		return false;
	while (m_EventPtr + 1 != m_Events.end() && (m_EventPtr + 1)->Type == BearWindowEventType::Resize) { m_EventPtr++; }
	e = *m_EventPtr;
	m_EventPtr++;
	return true;
}

bool BearWindow::Empty() const
{
	return m_WindowHandle == 0;
}

void BearWindow::OnEvent(BearEventWindows& e)
{
}

void BearWindow::OnEvent(HWND handle, UINT message, WPARAM wParam, LPARAM lParam)
{
	bint i = 0;
	if (m_EventPtr == m_Events.end()) i = -1;
	else i = m_Events.end() - m_EventPtr;
	BearEventWindows EventResult;
	switch (message)
	{
	case WM_SIZE:
	{

		RECT rect;
		GetClientRect(GetWindowHandle(), &rect);

		BearVector2<bsize> new_size(rect.right - rect.left, rect.bottom - rect.top);

		if (m_Fullscreen == false &&wParam != SIZE_MINIMIZED && new_size != GetSize())
		{
			if(new_size.x  == m_Width && new_size.y == m_Height ) break;
			m_Width = new_size.x;
			m_Height = new_size.y;
			EventResult.Size.width = GetSizeFloat().x;
			EventResult.Size.height = GetSizeFloat().y;
			EventResult.Type = BearWindowEventType::Resize;
			/*m_event_resize = EventResult;
			m_resize_timer.restart();*/
			OnEvent(EventResult);
		}
		break;
	}
	case WM_MOUSEMOVE:
	{
		int32 x = static_cast<int32>(LOWORD(lParam));
		int32 y = static_cast<int32>(HIWORD(lParam));

		RECT zone;
		GetClientRect(GetWindowHandle(), &zone);
		if (m_MouseEnter)
		{
			if ((zone.left > x || zone.right < x) ||
				(zone.top > y || zone.bottom < y))
			{
				EventResult.Type = BearWindowEventType::MouseLevae;
				OnEvent(EventResult);
				m_MouseEnter = false;
			}

		}
		else
		{
			if ((zone.left <x && zone.right > x) &&
				(zone.top < y && zone.bottom > y))
			{
				EventResult.Type = BearWindowEventType::MouseEnter;
				OnEvent(EventResult);
				m_MouseEnter = true;
			}
		}
		EventResult.Type = BearWindowEventType::MouseMove;
		EventResult.Position.x = static_cast<float>(x);
		EventResult.Position.y = static_cast<float>(y);
		OnEvent(EventResult);
		break;
	}
	case WM_SETFOCUS:
	{
		EventResult.Type = BearWindowEventType::Active;
		OnEvent(EventResult);
		break;
	}
	case WM_KILLFOCUS:
	{
		EventResult.Type = BearWindowEventType::Deactive;
		OnEvent(EventResult);
		break;
	}
	case WM_SYSKEYDOWN:
	case WM_KEYDOWN:
	{
		switch (wParam)
		{
		case VK_MENU:
			if (lParam & (1 << 24))
				wParam = VK_RMENU;
			else
				wParam = VK_LMENU;
			break;
		case VK_CONTROL:
			if (lParam & (1 << 24))
				wParam = VK_RCONTROL;
			else
				wParam = VK_LCONTROL;
			break;
		case VK_SHIFT:
			if (lParam & (1 << 24))
				wParam = VK_RSHIFT;
			else
				wParam = VK_LSHIFT;
			break;

		};
		auto item = GFromWinowsKey->find(DWORD(wParam));
		if (item == GFromWinowsKey->end())
			break;
		EventResult.Type = BearWindowEventType::KeyDown;
		EventResult.Key = static_cast<BearInput::Key>(item->second);
		OnEvent(EventResult);
		break;
	}
	case WM_KEYUP:
	case WM_SYSKEYUP:
	{
		switch (wParam)
		{
		case VK_MENU:
			if (lParam & (1 << 24))
				wParam = VK_RMENU;
			else
				wParam = VK_LMENU;
			break;
		case VK_CONTROL:
			if (lParam & (1 << 24))
				wParam = VK_RCONTROL;
			else
				wParam = VK_LCONTROL;
			break;
		case VK_SHIFT:
			if (lParam & (1 << 24))
				wParam = VK_RSHIFT;
			else
				wParam = VK_LSHIFT;
			break;

		};
		auto item = GFromWinowsKey->find(DWORD(wParam));
		if (item == GFromWinowsKey->end())
			break;
		EventResult.Type = BearWindowEventType::KeyUp;
		EventResult.Key = static_cast<BearInput::Key>(item->second);
		OnEvent(EventResult);
		break;
	}
	case WM_MOUSEWHEEL:
	{
		int16 delta = static_cast<int16>(HIWORD(wParam));
		if (delta > 0)
		{
			EventResult.Type = BearWindowEventType::KeyDown;
			EventResult.Change = delta / 120.f;
			EventResult.Key = BearInput::KeyMouseScrollUp;
		}
		else if (delta < 0)
		{
			EventResult.Type = BearWindowEventType::KeyDown;
			EventResult.Change = delta / 120.f;
			EventResult.Key = BearInput::KeyMouseScrollDown;
		}
		OnEvent(EventResult);
		break;
	}
	case WM_LBUTTONDOWN:
	{
		EventResult.Type = BearWindowEventType::KeyDown;
		EventResult.Key = BearInput::KeyMouseLeft;
		OnEvent(EventResult);
		break;
	}
	case WM_RBUTTONDOWN:
	{
		EventResult.Type = BearWindowEventType::KeyDown;
		EventResult.Key = BearInput::KeyMouseRight;
		OnEvent(EventResult);
		break;
	}
	case WM_MBUTTONDOWN:
	{
		EventResult.Type = BearWindowEventType::KeyDown;
		EventResult.Key = BearInput::KeyMouseMiddle;
		OnEvent(EventResult);
		break;
	}
	case WM_LBUTTONUP:
	{
		EventResult.Type = BearWindowEventType::KeyUp;
		EventResult.Key = BearInput::KeyMouseLeft;
		OnEvent(EventResult);
		break;
	}
	case WM_RBUTTONUP:
	{
		EventResult.Type = BearWindowEventType::KeyUp;
		EventResult.Key = BearInput::KeyMouseRight;
		OnEvent(EventResult);
		break;
	}
	case WM_MBUTTONUP:
	{
		EventResult.Type = BearWindowEventType::KeyUp;
		EventResult.Key = BearInput::KeyMouseMiddle;
		OnEvent(EventResult);
		break;
	}
	case WM_CHAR:
	{
		TCHAR ch = TCHAR(wParam);
		EventResult.Type = BearWindowEventType::Char;
		EventResult.Char = ch;
		OnEvent(EventResult);
		break;
	}
	default:
		return;
	}

	if (i < 0)
	{
		m_EventPtr = m_Events.end();
	}
	else if (i)
	{
		m_EventPtr = m_Events.begin() + i;
	}
	return;
}
#endif