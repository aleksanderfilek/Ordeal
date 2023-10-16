#ifndef CORE_H_
#define CORE_H_

#include "window.h"
#include "state.h"
#include "input.h"
#include "../graphics/renderer.h"

typedef enum core_state
{
    STATE_NOT_STARTED = 0,
    STATE_STARTED,
    STATE_STOPED
} core_state;

typedef struct core
{
    core_state State;

    window Window;
    input_manager InputManager;
    state_manager StateManager;
    renderer Renderer;
} core;

void core_init(core* Core, window_config WindowConfiguration, state_desc StartState);
void core_start(core* Core);
void core_destroy(core* Core);
core* core_get();

#endif