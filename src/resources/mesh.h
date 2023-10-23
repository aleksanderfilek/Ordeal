#ifndef MESH_H_
#define MESH_H_

#include <stdint.h>

typedef struct mesh
{
    uint32_t Vao, Vbo, Ebo;
    uint32_t IndiciesCount;
} mesh;

void mesh_load(mesh* Mesh, const char* Path);
void mesh_destroy(mesh* Mesh);
void mesh_draw(mesh* Mesh);

#endif