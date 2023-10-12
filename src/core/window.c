#include "window.h"

void window_init(window* Window, window_config config)
{
    Window->Config = config;

    if(SDL_InitSubSystem(SDL_INIT_VIDEO) < 0)
    {
        //printf("Window: SDL_Init %s\n", SDL_GetError());
        exit(-1);
    }

    Window->SdlWindow = SDL_CreateWindow(config.Title, SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 
        config.Size.X, config.Size.Y, SDL_WINDOW_SHOWN | SDL_WINDOW_OPENGL);

    if(!Window->SdlWindow)
    {
        //printf("Window could not be created! SDL Error: %s\n",SDL_GetError());
        exit(-1);
    }

    Window->Id = SDL_GetWindowID(Window->SdlWindow);
    
    Window->Renderer = SDL_CreateRenderer(Window->SdlWindow, - 1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
    if(!Window->Renderer)
    {
        //printf("Renderer could not be created! SDL Error: %s\n",SDL_GetError());
        exit(-1);
    }

    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 4);
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 3);

    SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, 
                        SDL_GL_CONTEXT_PROFILE_CORE);

    Window->GlContext = SDL_GL_CreateContext(Window->SdlWindow);

    SDL_GL_SetSwapInterval(1);

    glewExperimental = GL_TRUE;

    glewInit();

    glViewport(0, 0, config.Size.X, config.Size.Y);

    window_set_color(Window, config.BackgroundColor);
}

void window_destroy(window* Window)
{
    SDL_GL_DeleteContext(Window->GlContext);
    SDL_DestroyRenderer(Window->Renderer);
    SDL_DestroyWindow(Window->SdlWindow);
}

void window_handle_event(window* Window, const SDL_Event* event)
{
    // if (window->sdlEvent.window.event == SdlWindowEVENT_RESIZED) 
    // {
    //     glViewport(0, 0, window->sdlEvent.window.data1, window->sdlEvent.window.data2);
    //     window->size = (Int2){ window->sdlEvent.window.data1, window->sdlEvent.window.data2 };
    //     void* args[2] = { &window->sdlEvent.window.data1, &window->sdlEvent.window.data2 };
    //     EventInvoke(window->resizeEvent, window, args, 2);
    // }
}

void window_clear(window* Window)
{
    glClear(GL_COLOR_BUFFER_BIT);
}

void window_render(window* Window)
{
    SDL_GL_SwapWindow(Window->SdlWindow);
}

void window_set_color(window* Window, color BackgroundColor)
{
    Window->Config.BackgroundColor = BackgroundColor;
    glClearColor(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, BackgroundColor.a);
}

void window_set_fullscreen_enabled(window* Window, uint8_t Enable)
{
    Window->Config.FullscreenEnabled = Enable;
    SDL_SetWindowFullscreen(Window->SdlWindow, (Enable)?SDL_WINDOW_FULLSCREEN_DESKTOP:0);
}