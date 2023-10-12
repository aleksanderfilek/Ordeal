#ifndef MENU_STATE_H_
#define MENU_STATE_H_

#include "../../core/state.h"

typedef struct menu_state
{   
    state_base Base;
} menu_state;

void menu_state_start(state_base* State);
void menu_state_update(state_base* State, float ElapsedSeconds);
void menu_state_close(state_base* State);

#endif