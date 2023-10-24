#ifndef STATE_H_
#define STATE_H_

typedef struct state_base
{
    struct core* Core;
} state_base;

typedef struct state_desc
{
    state_base* Data;
    void (*StartFunc)(state_base* State);
    void (*UpdateFunc)(state_base* State, float Elapsed_seconds);
    void (*CloseFunc)(state_base* State);
} state_desc;

#define STATE(Data, StartFunc, UpdateFunc, CloseFunc) \
    (state_desc){ Data, StartFunc, UpdateFunc, CloseFunc }

typedef struct state_manager
{
    struct core* Core;
    state_desc CurrentState;
    state_desc NextState;
} state_manager;

void state_manager_init(state_manager* Manager, struct core* Core);
void state_manager_update(state_manager* Manager, float ElapsedSeconds);
void state_manager_destroy(state_manager* Manager);
void state_manager_set_state(state_manager* Manager, state_desc NextState);

#endif