#pragma once
#include <stddef.h>

#define ALIGN _Alignof(long double)
#define ROUND(x, y) (((x) + ((y) - 1)) & ~((y) - 1))
#define ROUND_BYTES(s) ROUND(s, ALIGN)