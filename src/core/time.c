#include "time.h"
#include <stdlib.h>
#include "../thirdParty/SDL2/SDL.h"

#define TIMER_CHUNK_SIZE 10

static time_manager* s_TimeManager = NULL;

static void time_manager_add_timer(time_manager* Manager, timer* Timer);
static void time_manager_remove_timer(time_manager* Manager, timer* Timer);
static void timer_update(timer* Timer, float ElapsedSeconds);

void time_manager_init(time_manager* Manager)
{
    if(s_TimeManager)
        return;

    s_TimeManager = Manager;

    Manager->LastFrameTick = 0;
    Manager->ElapsedTime = 0.0f;
    Manager->TimeScale = 1.0f;
    Manager->ScaledElapsedTime = 0.0f;
    Manager->Size = 0;
    Manager->Capacity = 1;
    Manager->Timers = calloc(Manager->Capacity, sizeof(timer*));
}

void time_manager_destroy(time_manager* Manager)
{
    free(Manager->Timers);
}

void time_manager_update(time_manager* Manager)
{
    Manager->ElapsedTime = (float)(SDL_GetTicks() - Manager->LastFrameTick)/1000.0f;
    Manager->ScaledElapsedTime = s_TimeManager->ElapsedTime * s_TimeManager->TimeScale;

    for(int i = 0; i < Manager->Capacity; i++)
    {
        timer* Timer = Manager->Timers[i];
        if(Timer == NULL)
            continue;

        timer_update(Timer, Manager->ScaledElapsedTime);
    }

    Manager->LastFrameTick = SDL_GetTicks();
}

time_manager* time_manager_get()
{
    return s_TimeManager;
}

float time_get_elapsed_seconds()
{
    return s_TimeManager->ScaledElapsedTime;
}

float time_get_unscaled_elapsed_seconds()
{
    return s_TimeManager->ElapsedTime;
}

void time_set_scale(float Scale)
{
    s_TimeManager->TimeScale = Scale;
}

float time_get_scale()
{
    return s_TimeManager->TimeScale;
}

void time_manager_add_timer(time_manager* Manager, timer* Timer)
{
    int searchIndex = 0;
    if(Manager->Size == Manager->Capacity)
    {
        Manager->Capacity += TIMER_CHUNK_SIZE;
        Manager->Timers = realloc(Manager->Timers, Manager->Capacity * sizeof(timer*));
        searchIndex = Manager->Size;
    }

    for(int i = searchIndex; i < Manager->Capacity; i++)
    {
        if(Manager->Timers[i])
            continue;

        Manager->Timers[i] = Timer;
        Manager->Size++;
        return;
    }
}

void time_manager_remove_timer(time_manager* Manager, timer* Timer)
{
    for(int i = 0; Manager->Capacity; i++)
    {
        if(Manager->Timers[i] != Timer)
            continue;

        Manager->Timers[i] = NULL;
        Manager->Size--;
        return;
    }
}

void timer_start(timer* Timer, float TimePeriod, uint8_t Loop, void (*Func)())
{
    Timer->Time = 0.0f;
    Timer->TimePeriod = TimePeriod;
    Timer->Loop = Loop;
    Timer->Func = Func;

    time_manager_add_timer(s_TimeManager, Timer);
}

void timer_stop(timer* Timer)
{
    time_manager_remove_timer(s_TimeManager, Timer);
}

void timer_restart(timer* Timer)
{
    Timer->Time = 0.0f;
}

void timer_update(timer* Timer, float ElapsedSeconds)
{
    Timer->Time += ElapsedSeconds;
    if(Timer->Time < Timer->TimePeriod)
    {
        return;
    }

    Timer->Func();

    if(Timer->Loop == 0)
    {
        timer_stop(Timer);
        return;
    }

    Timer->Time -= Timer->TimePeriod;
}