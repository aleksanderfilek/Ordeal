#ifndef MESH_H_
#define MESH_H_

#include "../types/guid.h"
#include <stdint.h>

typedef struct mesh
{
    uint32_t Vao, Vbo, Ebo;
    uint32_t IndiciesCount;
} mesh;

mesh* mesh_load(guid Id, const char* Path);
void mesh_destroy(void* Mesh);

void mesh_draw(mesh* Mesh);

#endif