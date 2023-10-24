#ifndef TIME_H_
#define TIME_H_

#include <stdint.h>

typedef struct timer
{
    float Time;
    float TimePeriod;
    uint8_t Loop;
    void (*Func)();
} timer;

void timer_start(timer* Timer, float TimePeriod, uint8_t Loop, void (*Func)());
void timer_stop(timer* Timer);
void timer_restart(timer* Timer);

typedef struct time_manager
{
    uint32_t LastFrameTick;

    float ElapsedTime;
    float TimeScale;
    float ScaledElapsedTime;

    timer** Timers;
    uint32_t Capacity;
    uint32_t Size;
} time_manager;

void time_manager_init(time_manager* Manager);
void time_manager_destroy(time_manager* Manager);
void time_manager_update(time_manager* Manager);
time_manager* time_manager_get();

float time_get_elapsed_seconds();
float time_get_unscaled_elapsed_seconds();
void time_set_scale(float Scale);
float time_get_scale();

#endif