#ifndef RENDERER_H_
#define RENDERER_H_

#include "shader.h"

typedef enum renderer_layer
{
    RENDERER_LAYER_GAME = 0,
    RENDERER_LAYER_POSTPROCESS = 1,
    RENDERER_LAYER_UI = 2
} renderer_layer;

typedef struct renderer
{
    shader_manager ShaderManager;

} renderer;

void renderer_init(renderer* Renderer);
void renderer_destroy(renderer* Renderer);

void renderer_draw(renderer* Renderer);

#endif