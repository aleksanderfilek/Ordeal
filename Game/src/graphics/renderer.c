#include "renderer.h"
#include "../core/core.h"
#include "../core/window.h"
#include "../core/resources.h"
#include <stdlib.h>
#include <stdio.h>

static renderer* s_Renderer;

static void react_to_window_resized(int argc, void* args);

void renderer_init(renderer* Renderer, vec2i WindowSize)
{
    if(s_Renderer)
        return;

    s_Renderer = Renderer;

    Renderer->Window = &core_get()->Window;
    Renderer->WindowSize = WindowSize;

    event_add(&Renderer->Window->OnWindowSizeChanged, react_to_window_resized);

    shader_manager_init(&Renderer->ShaderManager);

    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

    render_target_init(&Renderer->GameRenderTarget, Renderer->WindowSize, 1);
    render_target_init(&Renderer->UIRenderTarget, Renderer->WindowSize, 0);

    // Renderer->RenderPlane = mesh_load(guid_get("Plane"), "Resources/Plane.he");
    // Renderer->RenderShader = shader_load(guid_get("RenderShader"), "Resources/RenderShader.he");    
    // Renderer->SpriteShader = shader_load(guid_get("SpriteShader"), "Resources/SpriteShader.he");
}

void renderer_destroy(renderer* Renderer)
{
    render_target_destroy(&Renderer->GameRenderTarget);
    render_target_destroy(&Renderer->UIRenderTarget);

    shader_manager_destroy(&Renderer->ShaderManager);
}

void renderer_draw(renderer* Renderer)
{
    render_target_bind(&Renderer->GameRenderTarget);
    render_target_clear(&Renderer->GameRenderTarget);
    // draw game

    render_target_bind(&Renderer->UIRenderTarget);
    render_target_clear(&Renderer->UIRenderTarget);
    // draw ui

    // draw render targets to window
    render_target_bind_default(Renderer->WindowSize);
    //shader_bind(Renderer->RenderShader);
    window_clear(Renderer->Window);

    // render_target_bind_as_texture(&Renderer->GameRenderTarget, 0);
    // mesh_draw(Renderer->RenderPlane);
    // render_target_bind_as_texture(&Renderer->UIRenderTarget, 0);
    // mesh_draw(Renderer->RenderPlane);

    window_render(Renderer->Window);
}

void react_to_window_resized(int argc, void* args)
{
    s_Renderer->WindowSize = *(vec2i*)args;

    printf("%d %d\n", s_Renderer->WindowSize.X, s_Renderer->WindowSize.Y);

    render_target_destroy(&s_Renderer->GameRenderTarget);
    render_target_destroy(&s_Renderer->UIRenderTarget);

    render_target_init(&s_Renderer->GameRenderTarget, s_Renderer->WindowSize, 1);
    render_target_init(&s_Renderer->UIRenderTarget, s_Renderer->WindowSize, 0);
}