#pragma once
#ifdef DEVELOPER_VERSION
#define BEAR_CHECK(A) if(!(A)){BearCore::GetDebug()->Error(TEXT( BEAR_TO_STRING(A) ),TEXT(__FILE__),__FUNCTION__,__LINE__);}
#else
#define BEAR_CHECK(A) 
#endif

#ifdef DEVELOPER_VERSION
#define BEAR_VERIFY(A) if(!(A)){BearCore::GetDebug()->Error(TEXT( BEAR_TO_STRING(A) ),TEXT(__FILE__),__FUNCTION__,__LINE__);}
#else
#define BEAR_VERIFY(A) A
#endif

#define BEAR_ASSERT(A) if(!(A)){BearCore::GetDebug()->Error(TEXT( BEAR_TO_STRING(A) ),TEXT(__FILE__),__FUNCTION__,__LINE__);}
#define BEAR_ERRORMESSAGE(exp,text,...)if(!(exp)){BearCore::GetDebug()->ErrorMessage(text, ##__VA_ARGS__);}