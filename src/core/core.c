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
    time_manager_init(&Core->TimeManager);
    resource_manager_init(&Core->ResourceManager, 10);
}

void core_start(core* Core)
{
    Core->State = STATE_STARTED;

    while(Core->State == STATE_STARTED) {
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
        
        time_manager_update(&Core->TimeManager);
        float elapsedSeconds = time_get_elapsed_seconds();
        state_manager_update(&Core->StateManager, elapsedSeconds);
    }
}

void core_destroy(core* Core)
{
    resource_manager_destroy(&Core->ResourceManager);
    time_manager_destroy(&Core->TimeManager);
    state_manager_destroy(&Core->StateManager);
    input_manager_destroy(&Core->InputManager);
    window_destroy(&Core->Window);
}

core* core_get()
{
    return s_core;
}