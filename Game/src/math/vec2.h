#ifndef VEC2_H_
#define VEC2_H_

typedef struct vec2i
{
    int X, Y;
} vec2i;

vec2i vec2i_add(vec2i vec1, vec2i vec2);
vec2i vec2i_substract(vec2i vec1, vec2i vec2);
int vec2i_scalar(vec2i vec1, vec2i vec2);
vec2i vec2i_multiply(vec2i vec, int value);

typedef struct vec2f
{
    float X, Y;
} vec2f;

vec2f vec2f_add(vec2f vec1, vec2f vec2);
vec2f vec2f_substract(vec2f vec1, vec2f vec2);
float vec2f_scalar(vec2f vec1, vec2f vec2);
vec2f vec2f_multiply(vec2f vec, float value);
float vec2f_length(vec2f vec);
vec2f vec2f_normalized(vec2f vec);

#endif