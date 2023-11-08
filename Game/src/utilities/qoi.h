#ifndef QOI_H_
#define QOI_H_

#include <stdint.h>

uint8_t* qoi_decode(uint8_t* Bytes, uint32_t ByteSize, uint32_t Width, uint32_t Height, uint8_t Channel);

#endif