

 void BearSystem::ToClipboard(const bchar8 * text)
{
	 ToClipboard(text, BearString::GetSize(text));
}

 void  BearSystem::ToClipboard(const bchar16 * text)
{
	 ToClipboard(*BearEncoding::ToAnsi(text));
}


 void  BearSystem::ToClipboard(const bchar8 * text, bsize size)
 {
	 if (OpenClipboard(0))
	 {
		 HGLOBAL hgBuffer;
		 bchar8* chBuffer;
		 EmptyClipboard(); //очищаем буфер
		 hgBuffer = GlobalAlloc(GMEM_DDESHARE, size + 1);//выделяем память
		 chBuffer = (char*)GlobalLock(hgBuffer); //блокируем память
		BearString::CopyWithSizeLimit(chBuffer, size + 1, text, size);
		 GlobalUnlock(hgBuffer);//разблокируем память
		 SetClipboardData(CF_TEXT, hgBuffer);//помещаем текст в буфер обмена
		 CloseClipboard();
	 }
	
 }

  void  BearSystem::ToClipboard(const bchar16 * text, bsize size)
 {
	  ToClipboard(*BearEncoding::ToAnsi(text), size);
 }


  BearString  BearSystem::GetClipboard()
{
	 BearString result;
	 if (OpenClipboard(0))
	 {
		 HANDLE hData = GetClipboardData(CF_TEXT);//извлекаем текст из буфера обмена
		 bchar8* chBuffer = (bchar8*)GlobalLock(hData);//блокируем память

#ifdef UNICODE
		 result = BearEncoding::ToUTF16(chBuffer);
#else
		 result.assign(chBuffer);
#endif
		
		 GlobalUnlock(hData);//разблокируем память
		 CloseClipboard();//закрываем буфер обмена
	 }
	 return result;
}
