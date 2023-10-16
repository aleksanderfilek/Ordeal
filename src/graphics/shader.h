#ifndef SHADER_H_
#define SHADER_H_

#include "shadermanager.h"
#include "../types/guid.h"
#include "../math/vec2.h"
#include "../math/mat4.h"
#include <stdint.h>

typedef struct shader_uniform
{
    guid Id;
    uint32_t Loc;
} shader_uniform;

typedef struct shader
{
    uint32_t GlId;
    
    shader_uniform* Uniforms;
    uint32_t UniformCount;
} shader;

void shader_load(shader* Shader, const char* Path);
void shader_destroy(shader* Shader);

void shader_set_uniform_scalar(shader* Shader, guid Id, float Value);
void shader_set_uniform_vec2(shader* Shader, guid Id, vec2f Value);
void shader_set_uniform_mat4(shader* Shader, guid Id, mat4 Value);

#endif