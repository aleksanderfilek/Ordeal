#ifndef GUID_H_
#define GUID_H_

#include <stdint.h>

typedef uint32_t guid;

guid guid_get(const char* Text);

#endif