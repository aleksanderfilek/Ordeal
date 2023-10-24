#include "guid.h"
#include <string.h>

guid guid_get(const char* Text)
{
	uint32_t crc = 0xFFFFFFFF;
	uint32_t n = strlen(Text);

	for (uint32_t i = 0; i < n; i++) {
		char ch = Text[i];
		for (size_t j = 0; j < 8; j++) {
			uint32_t b = (ch ^ crc) & 1;
			crc >>= 1;
			if (b) crc = crc ^ 0xEDB88320;
			ch >>= 1;
		}
	}

	return ~crc;
}