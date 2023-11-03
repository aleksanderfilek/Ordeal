#include "core/core.h"
#include "states/menu/menustate.h"
#include <stdlib.h>
#include "resources/shader.h"

int WinMain(int argc, char *argv[])
{
    core* _core = (core*)malloc(sizeof(core));

    {
        window_config windowConfiguration = (window_config){
            .Title = "Ordeal",
            .Size = { 640, 480 },
            .BackgroundColor = COLOR_RED,
            .FullscreenEnabled = 0
        };

        state_desc startState = STATE(
            malloc(sizeof(menu_state)), 
            menu_state_start, 
            menu_state_update,
            menu_state_close);

        core_init(_core, windowConfiguration, startState);
    }

    core_start(_core);
    core_destroy(_core);

    free(_core);

    return 0;
}