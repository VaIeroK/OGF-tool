#pragma once
#define BEAR_TO_BOOL(id) BEAR_TO_BOOL_I(id)
#define BEAR_TO_BOOL_I(id) BEAR_BOOL_##id
#define BEAR_BOOL_0 0
#define BEAR_BOOL_1 1
#define BEAR_BOOL_2 1
#define BEAR_BOOL_3 1
#define BEAR_BOOL_4 1
#define BEAR_BOOL_5 1
#define BEAR_BOOL_6 1
#define BEAR_BOOL_7 1
#define BEAR_BOOL_8 1
#define BEAR_BOOL_9 1
#define BEAR_BOOL_10 1
#define BEAR_COMMA ,
#define BEAR_NIL
#define BEAR_IF(bool_,text,text1) BEAR_IF_I(bool_,text,text1)
#define BEAR_IF_I(bool_,text,text1) BEAR_IF_##bool_(text,text1)
#define BEAR_IF_0(text,text1) text1
#define BEAR_IF_1(text,text1)  text
//Если больше 0 то , инаце ничего
#define BEAR_IF_COMMA_I(count) BEAR_IF(BEAR_TO_BOOL(count),BEAR_COMMA,BEAR_NIL)
#define BEAR_IF_COMMA(count) BEAR_IF_COMMA_I(count)
//Создание парметров через запятую param0,param1,... BEAR_ENUM_PARAMS(Количество,имя_генрацыи) 
#define BEAR_ENUM_PARAMS_REPLACE(text,id) BEAR_IF_COMMA(id) text##id
#define BEAR_ENUM_PARAMS_I(count,parm) BEAR_REPEAT_##count(BEAR_ENUM_PARAMS_REPLACE,parm)
#define BEAR_ENUM_PARAMS(count,parm) BEAR_ENUM_PARAMS_I(count,parm)
#define BEAR_REPEAT(count,param,text) BEAR_REPEAT_I(count,param,text)
#define BEAR_REPEAT_I(count,param,text) BEAR_REPEAT_##count(param,text)

#define BEAR_REPEAT_0(parm,text)
#define BEAR_REPEAT_1(parm,text) parm(text,0)
#define BEAR_REPEAT_2(parm,text)  BEAR_REPEAT_1(parm,text) parm(text,1)
#define BEAR_REPEAT_3(parm,text)   BEAR_REPEAT_2(parm,text) parm(text,2)
#define BEAR_REPEAT_4(parm,text)  BEAR_REPEAT_3(parm,text) parm(text,3)
#define BEAR_REPEAT_5(parm,text)  BEAR_REPEAT_4(parm,text) parm(text,4)
#define BEAR_REPEAT_6(parm,text)  BEAR_REPEAT_5(parm,text) parm(text,5)
#define BEAR_REPEAT_7(parm,text)  BEAR_REPEAT_6(parm,text) parm(text,6)
#define BEAR_REPEAT_8(parm,text)  BEAR_REPEAT_7(parm,text) parm(text,7)
#define BEAR_REPEAT_9(parm,text)  BEAR_REPEAT_8(parm,text) parm(text,8) 
#define BEAR_REPEAT_10(parm,text)   BEAR_REPEAT_9(parm,text) parm(text,9)