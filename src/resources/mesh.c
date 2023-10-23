#include "mesh.h"
#include "../thirdParty/GL/Gl.h"
#include <stdio.h>
#include <stdlib.h>

void mesh_load(mesh* Mesh, const char* Path)
{
    FILE* file;
    file = fopen(Path,"rb");
    
    typedef struct Buffer
    {
        uint8_t Type;
        uint32_t Length;
        float* Data;
    } Buffer;

    uint32_t buffersSize = 0;
    uint32_t bufferCount = 0;
    fread(&bufferCount, sizeof(uint32_t), 1, file);
    Buffer* buffers = calloc(bufferCount, sizeof(Buffer));
    for(int i = 0; i < bufferCount; i++)
    {
        Buffer* buffer = &buffers[i];
        fread(&buffer->Type, sizeof(uint8_t), 1, file);
        fread(&buffer->Length, sizeof(uint32_t), 1, file);
        buffer->Data = malloc(buffer->Length * sizeof(float));
        fread(buffer->Data, sizeof(float), buffer->Length, file);
        buffersSize += buffer->Length;
    }

    uint32_t indiciesCount;
    fread(&indiciesCount, sizeof(uint32_t), 1, file);
    int* indicies = malloc(indiciesCount * sizeof(int));
    fread(indicies, sizeof(int), indiciesCount, file);

    fclose(file);

    Mesh->IndiciesCount = indiciesCount;

    glGenVertexArrays(1, &Mesh->Vao);
    gl_check_error();
    glGenBuffers(1, &Mesh->Vbo);
    gl_check_error();
    glGenBuffers(1, &Mesh->Ebo);
    gl_check_error();
    glBindVertexArray(Mesh->Vao);

    glBindBuffer(GL_ARRAY_BUFFER, Mesh->Vbo);
    glBufferData(GL_ARRAY_BUFFER, buffersSize * sizeof(float), NULL, GL_STATIC_DRAW);
    gl_check_error();

    uint32_t buffOffset = 0;

    for (int i = 0; i < bufferCount; i++)
    {
        Buffer* buffer = &buffers[i];
        uint32_t size = buffer->Length * sizeof(float);
        glBufferSubData(GL_ARRAY_BUFFER, buffOffset, size, buffer->Data);
        gl_check_error();
        buffOffset += size;
    }

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, Mesh->Ebo);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indiciesCount * sizeof(uint32_t), indicies, GL_STATIC_DRAW);
    gl_check_error();

    buffOffset = 0;
    for (int i = 0; i < bufferCount; i++)
    {
        Buffer* buffer = &buffers[i];
        glEnableVertexAttribArray(i);
        glVertexAttribPointer(i, (uint32_t)buffer->Type, GL_FLOAT, GL_FALSE, (uint32_t)buffer->Type * sizeof(float), (void*)(buffOffset));
        gl_check_error();
        buffOffset += buffer->Length * sizeof(float);
    }
    glBindVertexArray(0);

    for (int i = 0; i < bufferCount; i++)
    {
        free(buffers[i].Data);
    }
    free(buffers);
    free(indicies);
}

void mesh_destroy(mesh* Mesh)
{
    glDeleteBuffers(1, &Mesh->Vao);
    glDeleteBuffers(1, &Mesh->Vbo);
    glDeleteBuffers(1, &Mesh->Ebo);
}

void mesh_draw(mesh* Mesh)
{
    glBindVertexArray(Mesh->Vao);
    gl_check_error();
    glDrawElements(GL_TRIANGLES, Mesh->IndiciesCount, GL_UNSIGNED_INT, 0);
    gl_check_error();
    glBindVertexArray(0);
    gl_check_error();
}