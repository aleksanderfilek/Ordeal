#include "menustate.h"
#include "../../core/core.h"
#include <stdio.h>

void menu_state_start(state_base* State)
{

}

void menu_state_update(state_base* State, float ElapsedSeconds)
{
    if(input_is_key_down(KEYCODE_P))
        printf("%f\n", 1.0f/ElapsedSeconds);

    window_clear(&State->Core->Window);

    window_render(&State->Core->Window);
}

void menu_state_close(state_base* State)
{

}