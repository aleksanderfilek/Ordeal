#include "qoi.h"

/*
Encoder and decoder code was copies from https://github.com/phoboslab/qoi/blob/master/qoi.h repository.
*/

#include<string.h>
#include <stdlib.h>

#define QOI_OP_INDEX  0x00 /* 00xxxxxx */
#define QOI_OP_DIFF   0x40 /* 01xxxxxx */
#define QOI_OP_LUMA   0x80 /* 10xxxxxx */
#define QOI_OP_RUN    0xc0 /* 11xxxxxx */
#define QOI_OP_RGB    0xfe /* 11111110 */
#define QOI_OP_RGBA   0xff /* 11111111 */

#define QOI_MASK_2    0xc0 /* 11000000 */

#define QOI_COLOR_HASH(C) (C.rgba.r*3 + C.rgba.g*5 + C.rgba.b*7 + C.rgba.a*11)
#define QOI_MAGIC \
	(((unsigned int)'q') << 24 | ((unsigned int)'o') << 16 | \
	 ((unsigned int)'i') <<  8 | ((unsigned int)'f'))
#define QOI_HEADER_SIZE 4 // Only QOI_MAGIC

		/* 2GB is the max file size that this implementation can safely handle. We guard
		against anything larger than that, assuming the worst case with 5 bytes per
		pixel, rounded down to a nice clean value. 400 million pixels ought to be
		enough for anybody. */
#define QOI_PIXELS_MAX ((unsigned int)400000000)

typedef union {
	struct { uint8_t r, g, b, a; } rgba;
	unsigned int v;
} qoi_rgba_t;

static const unsigned char qoi_padding[8] = { 0,0,0,0,0,0,0,1 };

uint8_t* qoi_decode(uint8_t* Bytes, uint32_t ByteSize, uint32_t Width, uint32_t Height, uint8_t Channel)
{
    uint32_t header_magic;
    uint8_t* pixels;
    qoi_rgba_t index[64];
    qoi_rgba_t px;
    int px_len, chunks_len, px_pos;
    int p = 0, run = 0;

    if (Bytes == NULL)
    {
        return NULL;
    }

    if (ByteSize < QOI_HEADER_SIZE + (int)sizeof(qoi_padding))
    {
        return NULL;
    }

    header_magic = ((uint32_t)Bytes[p++]) << 24;
    header_magic |= ((uint32_t)Bytes[p++]) << 16;
    header_magic |= ((uint32_t)Bytes[p++]) << 8;
    header_magic |= ((uint32_t)Bytes[p++]);

    if (header_magic != QOI_MAGIC ||
        (uint32_t)Height >= QOI_PIXELS_MAX / (uint32_t)Width
        ) {
        return NULL;
    }

    px_len = Width * Height * Channel;
    pixels = malloc(sizeof(uint8_t) * px_len);
    if (!pixels) {
        return NULL;
    }

    memset(index, 0, sizeof(index));
    px.rgba.r = 0;
    px.rgba.g = 0;
    px.rgba.b = 0;
    px.rgba.a = 255;

    chunks_len = ByteSize - (int)sizeof(qoi_padding);
    for (px_pos = 0; px_pos < px_len; px_pos += Channel) {
        if (run > 0) {
            run--;
        }
        else if (p < chunks_len) {
            int b1 = Bytes[p++];

            if (b1 == QOI_OP_RGB) {
                px.rgba.r = Bytes[p++];
                px.rgba.g = Bytes[p++];
                px.rgba.b = Bytes[p++];
            }
            else if (b1 == QOI_OP_RGBA) {
                px.rgba.r = Bytes[p++];
                px.rgba.g = Bytes[p++];
                px.rgba.b = Bytes[p++];
                px.rgba.a = Bytes[p++];
            }
            else if ((b1 & QOI_MASK_2) == QOI_OP_INDEX) {
                px = index[b1];
            }
            else if ((b1 & QOI_MASK_2) == QOI_OP_DIFF) {
                px.rgba.r += ((b1 >> 4) & 0x03) - 2;
                px.rgba.g += ((b1 >> 2) & 0x03) - 2;
                px.rgba.b += (b1 & 0x03) - 2;
            }
            else if ((b1 & QOI_MASK_2) == QOI_OP_LUMA) {
                int b2 = Bytes[p++];
                int vg = (b1 & 0x3f) - 32;
                px.rgba.r += vg - 8 + ((b2 >> 4) & 0x0f);
                px.rgba.g += vg;
                px.rgba.b += vg - 8 + (b2 & 0x0f);
            }
            else if ((b1 & QOI_MASK_2) == QOI_OP_RUN) {
                run = (b1 & 0x3f);
            }

            index[QOI_COLOR_HASH(px) % 64] = px;
        }

        pixels[px_pos + 0] = px.rgba.r;
        pixels[px_pos + 1] = px.rgba.g;
        pixels[px_pos + 2] = px.rgba.b;

        if (Channel == 4) {
            pixels[px_pos + 3] = px.rgba.a;
        }
    }

    return pixels;
}