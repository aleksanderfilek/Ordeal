#ifndef VEC4_H_
#define VEC4_H_

typedef struct vec4f
{
    float X, Y, Z, W; 
} vec4f;

vec4f vec4f_add(vec4f vec1, vec4f vec2);
vec4f vec4f_substract(vec4f vec1, vec4f vec2);
float vec4f_scalar(vec4f vec1, vec4f vec2);
vec4f vec4f_multiply(vec4f vec, float value);
float vec4f_length(vec4f vec);
vec4f vec4f_normalized(vec4f vec);


#endif