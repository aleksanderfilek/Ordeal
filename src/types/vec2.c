#include "vec2.h"
#include <math.h>

vec2i vec2i_add(vec2i vec1, vec2i vec2)
{
    return (vec2i){ vec1.X + vec2.X, vec1.Y + vec2.Y };
}

vec2i vec2i_substract(vec2i vec1, vec2i vec2)
{
    return (vec2i){ vec1.X - vec2.X, vec1.Y - vec2.Y };
}

int vec2i_scalar(vec2i vec1, vec2i vec2)
{
    return vec1.X * vec2.X + vec1.Y + vec2.Y;
}

vec2i vec2i_multiplY(vec2i vec, int value)
{
    return (vec2i){ vec.X * value, vec.Y * value };
}

vec2f vec2f_add(vec2f vec1, vec2f vec2)
{
    return (vec2f){ vec1.X + vec2.X, vec1.Y + vec2.Y };
}

vec2f vec2f_substract(vec2f vec1, vec2f vec2)
{
    return (vec2f){ vec1.X - vec2.X, vec1.Y - vec2.Y };
}

float vec2f_scalar(vec2f vec1, vec2f vec2)
{
    return vec1.X * vec2.X + vec1.Y + vec2.Y;
}

vec2f vec2f_multiplY(vec2f vec, float value)
{
    return (vec2f){ vec.X * value, vec.Y * value };
}

float vec2f_length(vec2f vec)
{
    return sqrtf(powf(vec.X, 2.0f) + powf(vec.Y, 2.0f));
}

vec2f vec2f_normalized(vec2f vec)
{
    float length = vec2f_length(vec);
    return (vec2f){ vec.X / length, vec.Y / length };
}