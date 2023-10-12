#include "core.h"
#include <string.h>

static core* s_core = NULL;

void core_init(core* Core, window_config WindowConfiguration, state_desc StartState)
{
    if(s_core) {
        return;
    }

    s_core = Core;

    Core->State = STATE_NOT_STARTED;
    memset(Core, 0, sizeof(core));

    window_init(&Core->Window, WindowConfiguration);
    input_manager_init(&Core->InputManager);
    state_manager_init(&Core->StateManager, Core);
    state_manager_set_state(&Core->StateManager, StartState);
}

void core_start(core* Core)
{
    Core->State = STATE_STARTED;

    uint32_t timer = 0;
    float elapsed_seconds = 0.0f;
    while(Core->State == STATE_STARTED) {
        timer = SDL_GetTicks();

        input_manager_update(&Core->InputManager);

        SDL_Event event;
        while(SDL_PollEvent(&event) != 0) {
            switch (event.type) {
                case SDL_QUIT:
                    Core->State = STATE_STOPED;
                    break;
                case SDL_WINDOWEVENT:
                    window_handle_event(&Core->Window, &event);
                    break;
            }
        }
        
        state_manager_update(&Core->StateManager, elapsed_seconds);

        elapsed_seconds = (float)(SDL_GetTicks() - timer)/1000.0f;
    }
}

void core_destroy(core* Core)
{
    state_manager_destroy(&Core->StateManager);
    input_manager_destroy(&Core->InputManager);
    window_destroy(&Core->Window);
}

core* core_get()
{
    return s_core;
}