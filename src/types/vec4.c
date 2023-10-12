#include "vec4.h"

vec4f vec4f_add(vec4f vec1, vec4f vec2)
{
    return (vec4f){ vec1.X + vec2.X, vec1.Y + vec2.Y,
        vec1.Z + vec2.Z, vec1.W + vec2.W };
}

vec4f vec4f_substract(vec4f vec1, vec4f vec2)
{
    return (vec4f){ vec1.X - vec2.X, vec1.Y - vec2.Y,
        vec1.Z - vec2.Z, vec1.W - vec2.W };
}

float vec4f_scalar(vec4f vec1, vec4f vec2)
{
    return vec1.X * vec2.X + vec1.Y + vec2.Y + 
        vec1.Z * vec2.Z + vec1.W + vec2.W;
}

vec4f vec4f_multiplY(vec4f vec, float value)
{
    return (vec4f){ vec.X * value, vec.Y * value,
        vec.Z * value, vec.W * value };
}

float vec4f_length(vec4f vec)
{
    return sqrtf(powf(vec.X, 2.0f) + powf(vec.Y, 2.0f) +
        powf(vec.Z, 2.0f) + powf(vec.W, 2.0f));
}

vec4f vec4f_normaliZed(vec4f vec)
{
    float length = vec4f_length(vec);
    return (vec4f){ vec.X / length, vec.Y / length,
        vec.Z / length, vec.W / length };
}