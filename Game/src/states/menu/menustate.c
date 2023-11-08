#include "menustate.h"
#include "../../core/core.h"
#include <stdio.h>

shader* s;
mesh* m;
texture* t;

void menu_state_start(state_base* State)
{
    s = shader_load(guid_get("shader"), "bin/S_Simple.oda");
    //shader_destroy(s);

    m = mesh_load(guid_get("mesh"), "bin/M_Plane.oda");
    //mesh_destroy(m);

    t = texture_load(guid_get("texture"), "bin/T_Test.oda");
    //texture_destroy(t);

    shader_bind(s);
    texture_bind(t, 0);
}

void menu_state_update(state_base* State, float ElapsedSeconds)
{
    // if(input_is_key_down(KEYCODE_P))
    //     printf("%f\n", 1.0f/ElapsedSeconds);

    window_clear(&State->Core->Window);

    mesh_draw(m);

    window_render(&State->Core->Window);
}

void menu_state_close(state_base* State)
{

}