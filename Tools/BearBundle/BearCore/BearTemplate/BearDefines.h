#pragma once
#define  BEAR_TO_STRING_HELPER(x) #x
#define  BEAR_TO_STRING(x)  BEAR_TO_STRING_HELPER(x)
#define  BEAR_MAKE_WITH_COMMA(a,b) a,b