#include "input.h"
#include <string.h>
#include <stdlib.h>
#include "../thirdParty/SDL2/SDL.h"

static input_manager* s_input_manager = NULL;

void input_manager_init(input_manager* Manager)
{
    if(s_input_manager) {
        return;
    }

    s_input_manager = Manager;

    memset(Manager, 0, sizeof(input_manager));

    Manager->CurrentKeyboardState = SDL_GetKeyboardState(&Manager->KeyboardStatesCount);
    Manager->PreviousKeyboardState = (uint8_t*)calloc(Manager->KeyboardStatesCount, sizeof(uint8_t));

}

void input_manager_update(input_manager* Manager)
{
    //update mouse
    Manager->PreviousMouseState = Manager->CurrentMouseState;
    Manager->PreviousMousePosition = Manager->CurrentMousePosition;

    if(Manager->RelativeMode == 0)
    {
        Manager->CurrentMouseState = SDL_GetMouseState(
            &Manager->CurrentMousePosition.X, 
            &Manager->CurrentMousePosition.Y);
    }
    else
    {
        Manager->CurrentMouseState = SDL_GetRelativeMouseState(
            &Manager->CurrentMousePosition.X, 
            &Manager->CurrentMousePosition.Y);
    }

    //update keyboard
    SDL_memcpy(Manager->PreviousKeyboardState, 
        Manager->CurrentKeyboardState, Manager->KeyboardStatesCount * sizeof(uint8_t));

}

void input_manager_destroy(input_manager* Manager)
{
    free(Manager->PreviousKeyboardState);
}

input_manager* input_manager_get()
{
    return s_input_manager;
}


uint8_t input_is_key_pressed(key_code Key)
{
    return s_input_manager->CurrentKeyboardState[Key] && 
        s_input_manager->PreviousKeyboardState[Key]; 
}

uint8_t input_is_key_down(key_code Key)
{
    return s_input_manager->CurrentKeyboardState[Key] && 
        !s_input_manager->PreviousKeyboardState[Key]; 
}

uint8_t input_is_key_up(key_code Key)
{
    return !s_input_manager->CurrentKeyboardState[Key] && 
        s_input_manager->PreviousKeyboardState[Key]; 
}

uint8_t input_is_mouse_button_pressed(mouse_key Button)
{
    return s_input_manager->CurrentMouseState&SDL_BUTTON(Button) && 
        s_input_manager->PreviousMouseState&SDL_BUTTON(Button); 
}

uint8_t input_is_mouse_button_down(mouse_key Button)
{
    return s_input_manager->CurrentMouseState&SDL_BUTTON(Button) && 
        !(s_input_manager->PreviousMouseState&SDL_BUTTON(Button)); 
}

uint8_t input_is_mouse_button_up(mouse_key Button)
{
    return !(s_input_manager->CurrentMouseState&SDL_BUTTON(Button)) && 
        s_input_manager->PreviousMouseState&SDL_BUTTON(Button); 
}

vec2i input_get_mouse_position()
{
    return s_input_manager->CurrentMousePosition;
}

void input_set_mouse_position(vec2i Position)
{
    s_input_manager->CurrentMousePosition = Position;
}

vec2i input_get_mouse_delta_position()
{
    if(s_input_manager->RelativeMode == 0)
    {
        return vec2i_substract(s_input_manager->CurrentMousePosition, 
            s_input_manager->PreviousMousePosition);
    }

    return s_input_manager->CurrentMousePosition;
}

void input_set_RelativeMode(uint8_t Enable)
{
    s_input_manager->RelativeMode = Enable;
    SDL_SetRelativeMouseMode((Enable)? SDL_TRUE : SDL_FALSE);
}
