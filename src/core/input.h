#ifndef INPUT_H_
#define INPUT_H_

#include "inputkeys.h"
#include "../types/vec2.h"

#include<stdint.h>

typedef struct input_manager
{
    int KeyboardStatesCount;
    const uint8_t *CurrentKeyboardState;
    uint8_t *PreviousKeyboardState;
    uint32_t CurrentMouseState;
    uint32_t PreviousMouseState;
    vec2i CurrentMousePosition;
    vec2i PreviousMousePosition;
    uint8_t RelativeMode;
} input_manager;

void input_manager_init(input_manager* Manager);
void input_manager_update(input_manager* Manager);
void input_manager_destroy(input_manager* Manager);
input_manager* input_manager_get();

uint8_t input_is_key_pressed(key_code Key);
uint8_t input_is_key_down(key_code Key);
uint8_t input_is_key_up(key_code Key);
uint8_t input_is_mouse_button_pressed(mouse_key Button);
uint8_t input_is_mouse_button_down(mouse_key Button);
uint8_t input_is_mouse_button_up(mouse_key Button);
vec2i input_get_mouse_position();
void input_set_mouse_position(vec2i Position);
vec2i input_get_mouse_delta_position();
void input_set_relative_mode(uint8_t Enable);

#endif