#ifndef SHADER_H_
#define SHADER_H_

#include "../graphics/shadermanager.h"
#include "../types/guid.h"
#include "../math/vec2.h"
#include "../math/vec4.h"
#include "../math/mat3.h"
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

shader* shader_load(guid Id, const char* Path);
void shader_destroy(void* Shader);

void shader_set_uniform_scalar(shader* Shader, guid Id, float Value);
void shader_set_uniform_vec2(shader* Shader, guid Id, vec2f Value);
void shader_set_uniform_vec4(shader* Shader, guid Id, vec4f Value);
void shader_set_uniform_mat3(shader* Shader, guid Id, mat3 Value);

#endif