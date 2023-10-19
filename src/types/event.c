#include "event.h"
#include <stdlib.h>
#include <string.h>

void event_init(event* Event)
{
    Event->Funcs = malloc(sizeof(event_func));
    Event->Funcs[0] = NULL;
    Event->Size = 0;
}

void event_destroy(event* Event)
{
    event_clear(Event);
}

void event_add(event* Event, event_func Func)
{
    Event->Funcs = realloc(Event->Funcs, (Event->Size + 2) * sizeof(event_func));
    Event->Funcs[Event->Size] = Func;
    Event->Funcs[Event->Size + 1] = NULL;
    Event->Size++;
}

void event_remove(event* Event, event_func Func)
{
    for(int i = 0; i < Event->Size; i++)
    {
        if(Event->Funcs[i] != Func)
            continue;

        memcpy(Event->Funcs + i, Event->Funcs + i + 1, Event->Size - i);
        Event->Size--;
        Event->Funcs = realloc(Event->Funcs, (Event->Size + 1) * sizeof(event_func));
        return;
    }
}

void event_clear(event* Event)
{
    free(Event->Funcs);
    Event->Funcs = malloc(sizeof(event_func));
    Event->Funcs[0] = NULL;
    Event->Size = 0;
}

void event_broadcast(event* Event, int argc, void* args)
{
    event_func* func = Event->Funcs;
    for(; *func != NULL; func++)
    {
        (*func)(argc, args);
    }
}