#ifndef TEXTURE_H_
#define TEXTIRE_H_

#include "color.h"
#include "../math/vec2.h"
#include "../math/vec4.h"
#include <stdint.h>

typedef enum texture_filter_method
{
    TEXTURE_FILTER_NEAREST =   0,
    TEXTURE_FILTER_LINEAR =    1
} texture_filter_method;

typedef enum texture_wrap_method
{
    TEXTURE_WRAP_REPEAT =                0,
    TEXTURE_WRAP_CLAMP_TO_EDGE =         1,
    TEXTURE_WRAP_CLAMP_TO_BORDER =       2,
    TEXTURE_WRAP_MIRRORED_REPEAT =       3,
    TEXTURE_WRAP_MIRROR_CLAMP_TO_EDGE =  4
} texture_wrap_method;

typedef struct texture
{
    uint32_t GlId;
    vec2i Size;
    color_channel Channel;
    color_space ColorSpace;
    texture_filter_method FilterMethod;
    texture_wrap_method WrapMethod;
    uint8_t GenerateMipmapsEnabled;
    vec2i AtlasSize;
} texture;

void texture_load(texture* Texture, const char* Path);
void texture_destroy(texture* Texture);

void texture_bind(texture* Texture, uint32_t SlotIndex);
vec4f texture_get_sprite_rect(texture* Texture, int SpriteIndex);

#endif