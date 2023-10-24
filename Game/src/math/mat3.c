#include "mat3.h"
#include <string.h>

mat3 mat3_sprite_model(vec2f Position, vec2f Size)
{
    mat3 matrix;
    memset(&matrix, 0, sizeof(mat3));
    matrix.Column[0].X = Size.X / 2.0f;
    matrix.Column[1].Y = Size.Y / 2.0f;
    matrix.Column[2].Z = 1.0f;
    matrix.Column[2].X = (Size.X / 2.0f) + Position.X;
    matrix.Column[2].Y = (-Size.Y / 2.0f) + Position.Y;
    return matrix;
}

mat3 mat3_sprite_uv(vec4f Rect)
{
    vec2f rectDiff = { Rect.Z - Rect.X, Rect.W - Rect.Y };

    mat3 matrix;
    memset(&matrix, 0, sizeof(mat3));
    matrix.Column[0].X = rectDiff.X / 2.0f;
    matrix.Column[1].Y = rectDiff.Y / 2.0f;
    matrix.Column[2].Z = 1.0f;
    matrix.Column[2].X = (rectDiff.X / 2.0f) + Rect.X;
    matrix.Column[2].Y = (-rectDiff.Y / 2.0f) + Rect.Y;
    return matrix;
}

mat3 mat3_screen_view(vec2f Position, vec2f Size)
{
    mat3 matrix;
    memset(&matrix, 0, sizeof(mat3));
    matrix.Column[0].X = 2.0f / Size.X;
    matrix.Column[1].Y = -2.0f / Size.Y;
    matrix.Column[3].Z = 1.0f;
    matrix.Column[2].X = -1.0f;
    matrix.Column[2].Y = 1.0f;
    return matrix;
}