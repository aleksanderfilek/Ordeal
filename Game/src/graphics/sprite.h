#ifndef SPRITE_H_
#define SPRITE_H_

#include "../math/vec2.h"
#include "../math/vec4.h"
#include "color.h"
#include "../resources/texture.h"

typedef struct sprite
{
    vec2f Position;
    struct texture* Texture;
    color Color;
    vec4f Rect;
} sprite;

#endif