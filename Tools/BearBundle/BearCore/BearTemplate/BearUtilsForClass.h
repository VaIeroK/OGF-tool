#pragma once

#define BEAR_CLASS_STATIC(CL)\
	CL()=delete;\
	CL&operator=(const CL&cl)=delete;
	
#define BEAR_FUNCTION_DELETE(TYPE,FUN)\
	TYPE FUN()=delete;
	
#define BEAR_CLASS_WITHOUT_COPY(CL)\
	CL(const CL&cl) = delete;\
	CL(CL&&cl) = delete;\
	CL&operator=(const CL&cl) = delete;\
	CL&operator=(CL&&cl) = delete;
	
#ifdef DEVELOPER_VERSION
#define BEAR_FUNCTION_DEBUG(RET,FUN,...)\
	RET FUN(__VA_ARGS__);
#else
#define BEAR_FUNCTION_DEBUG(RET,FUN,...)\
	inline RET FUN(__VA_ARGS__){}	
#endif