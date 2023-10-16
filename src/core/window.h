#ifndef WINDOW_H_
#define WINDOW_H_

#include "../graphics/color.h"
#include "../math/vec2.h"
#include"../thirdParty/SDL2/SDL.h"
#include"../thirdParty/GL/Gl.h"
#include"../thirdParty/SDL2/SDL_opengl.h"
#include<stdint.h>

typedef struct window_config
{
    const char* Title;
    vec2i Size;
    color BackgroundColor;
    uint8_t FullscreenEnabled;
}window_config;

typedef struct window
{
    window_config Config;

    uint32_t Id;
    SDL_Window* SdlWindow;
    SDL_Renderer* Renderer;
    SDL_GLContext GlContext;
} window;

void window_init(window* Window, window_config Config);
void window_destroy(window* Window);

void window_handle_event(window* Window, const SDL_Event* Event);
void window_clear(window* Window);
void window_render(window* Window);

void window_set_color(window* Window, color BackgroundColor);
void window_set_fullscreen_enabled(window* Window, uint8_t Enable);

#endif