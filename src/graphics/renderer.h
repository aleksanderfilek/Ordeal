#ifndef RENDERER_H_
#define RENDERER_H_

#include "../resources/shader.h"
#include "sprite.h"
#include "../resources/mesh.h"
#include "rendertarget.h"

typedef struct renderer
{
    struct window* Window;
    vec2i WindowSize;

    mesh* RenderPlane;
    shader* RenderShader;

    shader_manager ShaderManager;

    sprite* Sprites;
    uint32_t SpritesCount;
    uint32_t SpritesCapacity;
    shader* SpriteShader;

    render_target GameRenderTarget;
    render_target UIRenderTarget;
} renderer;

void renderer_init(renderer* Renderer, vec2i WindowSize);
void renderer_destroy(renderer* Renderer);

void renderer_draw(renderer* Renderer);

#endif