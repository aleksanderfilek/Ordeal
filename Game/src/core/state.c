#include "state.h"
#include <string.h>
#include <stdlib.h>

#define STATE_NULL STATE(NULL, NULL, NULL, NULL)

static void state_manager_check_for_state_change(state_manager* Manager);

void state_manager_init(state_manager* Manager, struct core* Core)
{
    memset(Manager, 0, sizeof(state_manager));
    Manager->Core = Core;
}

void state_manager_update(state_manager* Manager, float ElapsedSeconds)
{
    state_manager_check_for_state_change(Manager);

    if(Manager->CurrentState.Data && Manager->CurrentState.UpdateFunc)
    {
        Manager->CurrentState.UpdateFunc(Manager->CurrentState.Data, ElapsedSeconds);
    }
}

void state_manager_destroy(state_manager* Manager)
{
    if(Manager->CurrentState.Data)
    {
        if(Manager->CurrentState.CloseFunc)
        {
            Manager->CurrentState.CloseFunc(Manager->CurrentState.Data);
        }
        free(Manager->CurrentState.Data);
        Manager->CurrentState = STATE_NULL;
    }

    if(Manager->NextState.Data)
    {
        free(Manager->NextState.Data);
        Manager->NextState = STATE_NULL;
    }
}

void state_manager_set_state(state_manager* Manager, state_desc NextState)
{
    if(Manager->NextState.Data)
    {
        free(Manager->NextState.Data);
        Manager->NextState = STATE_NULL;
    }

    Manager->NextState = NextState;
}

void state_manager_check_for_state_change(state_manager* Manager)
{
    if(Manager->NextState.Data)
    {
        if(Manager->CurrentState.Data)
        {
            if(Manager->CurrentState.CloseFunc)
            {
                Manager->CurrentState.CloseFunc(Manager->CurrentState.Data);
            }
            free(Manager->CurrentState.Data);
            Manager->CurrentState = STATE_NULL;
        }

        Manager->CurrentState = Manager->NextState;
        Manager->CurrentState.Data->Core = Manager->Core;
        if(Manager->CurrentState.StartFunc)
        {
            Manager->CurrentState.StartFunc(Manager->CurrentState.Data);
        }

        Manager->NextState = STATE_NULL;
    }
}