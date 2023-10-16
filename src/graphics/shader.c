#include "shader.h"
#include "../thirdParty/GL/Gl.h"
#include <stdio.h>
#include <stdlib.h>

void shader_load(shader* Shader, const char* Path)
{
    FILE* file;
    file = fopen(Path,"rb");

    uint32_t vertexShaderByteSize, fragmentShaderByteSize;
    char *vertexShader = NULL, *fragmentShader = NULL;

    fread(&vertexShaderByteSize, sizeof(uint32_t), 1, file);
    vertexShader = malloc(vertexShaderByteSize + 1);
    vertexShader[vertexShaderByteSize] = '\0';
    fread(vertexShader, sizeof(char), vertexShaderByteSize, file);

    fread(&fragmentShaderByteSize, sizeof(uint32_t), 1, file);
    fragmentShader = malloc(fragmentShaderByteSize + 1);
    fragmentShader[fragmentShaderByteSize] = '\0';
    fread(fragmentShader, sizeof(char), fragmentShaderByteSize, file);

    uint32_t uniformCount, uniformNameSize, uniformGuid;
    char* uniformName = NULL;

    fread(&uniformCount, sizeof(uint32_t), 1, file);

    typedef struct uniform
    {
        guid Id;
        char* Name;
    }uniform;
    uniform* uniforms = malloc(sizeof(uniform) * uniformCount);

    for(int i = 0; i < uniformCount; i++)
    {
        fread(&uniformGuid, sizeof(uint32_t), 1, file);
        fread(&uniformNameSize, sizeof(uint32_t), 1, file);
        uniformName = malloc(uniformNameSize + 1);
        uniformName[uniformNameSize] = '\0';
        fread(uniformName, sizeof(char), uniformNameSize, file);

        uniforms[i] = (uniform){ uniformGuid, uniformName };
    }

    fclose(file);

    uint32_t program = glCreateProgram();

    uint32_t vertexShaderId = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertexShaderId, 1, (const char**)&vertexShader, NULL);
    glCompileShader(vertexShaderId);
    gl_shader_check_error(vertexShaderId, GL_COMPILE_STATUS);

    glAttachShader(program, GL_VERTEX_SHADER);
    gl_program_check_error(program, GL_ATTACHED_SHADERS);

    uint32_t fragmentShaderId = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragmentShaderId, 1, (const char**)&fragmentShader, NULL);
    glCompileShader(fragmentShaderId);
    gl_shader_check_error(fragmentShaderId, GL_COMPILE_STATUS);

    glAttachShader(program, GL_FRAGMENT_SHADER);
    gl_program_check_error(program, GL_ATTACHED_SHADERS);

    glLinkProgram(program);
    int  success;
    glGetProgramiv(program, GL_LINK_STATUS, &success);
    gl_program_check_error(program, GL_LINK_STATUS);

    glDeleteShader(vertexShaderId);
    gl_shader_check_error(vertexShaderId, GL_DELETE_STATUS);
    glDeleteShader(fragmentShaderId);
    gl_shader_check_error(fragmentShaderId, GL_DELETE_STATUS);

    free(vertexShader);
    free(fragmentShader);
    
    Shader->GlId = program;
    Shader->UniformCount = uniformCount;
    Shader->Uniforms = malloc(sizeof(shader_uniform) * uniformCount);
    shader_bind(Shader);
    for(int i = 0; i < uniformCount; i++)
    {
        Shader->Uniforms[i] = (shader_uniform){ 
            uniforms[i].Id,
            glGetUniformLocation(program, uniforms[i].Name)
        };
        free(uniforms[i].Name);
    }

    free(uniforms);
}

void shader_destroy(shader* Shader)
{
    glDeleteProgram(Shader->GlId);
    free(Shader->Uniforms);
}

void shader_set_uniform_scalar(shader* Shader, guid Id, float Value)
{
    for(int i = 0; i < Shader->UniformCount; i++)
    {
        shader_uniform* uniform = &Shader->Uniforms[i];
        if(uniform->Id != Id)
            continue;

        glUniform1f(uniform->Loc, Value);
        return;
    }
}

void shader_set_uniform_vec2(shader* Shader, guid Id, vec2f Value)
{
    for(int i = 0; i < Shader->UniformCount; i++)
    {
        shader_uniform* uniform = &Shader->Uniforms[i];
        if(uniform->Id != Id)
            continue;

        glUniform2fv(uniform->Loc, &Value.X);
        return;
    }
}

void shader_set_uniform_mat4(shader* Shader, guid Id, mat4 Value)
{
    for(int i = 0; i < Shader->UniformCount; i++)
    {
        shader_uniform* uniform = &Shader->Uniforms[i];
        if(uniform->Id != Id)
            continue;

        glUniformMatrix4fv(uniform->Loc, 1, GL_FALSE, &Value.Column[0].X);
        return;
    }
}