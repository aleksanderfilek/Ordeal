#ifndef MATRIX3_H_
#define MATRIX3_H_

#include "vec2.h"
#include "vec3.h"
#include "vec4.h"

typedef struct mat3
{
    vec3f Column[4];
} mat3;

mat3 mat3_sprite_model(vec2f Position, vec2f Size);
mat3 mat3_sprite_uv(vec4f Rect);
mat3 mat3_screen_view(vec2f Position, vec2f WindowSize);

#endif