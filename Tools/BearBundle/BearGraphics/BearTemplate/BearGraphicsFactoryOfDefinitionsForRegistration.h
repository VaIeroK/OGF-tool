#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...) class BEARGRAPHICS_API BearRHI##Name {public: virtual ~BearRHI##Name()=0;
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) class BEARGRAPHICS_API BearRHI##Name:public virtual  BearRHI##Parent {public: virtual ~BearRHI##Name()=0;
#define RENDER_METHOD_REGISTRATION(RetVal,Name,...) virtual RetVal Name(__VA_ARGS__)=0;
#define	RENDER_END_CLASS_REGISTRATION() };
