#ifndef EVENT_H_
#define EVENT_H_

typedef void (*event_func)(int argc, void* args);

typedef struct event
{ 
    event_func* Funcs;
    int Size;
} event;

void event_init(event* Event);
void event_destroy(event* Event);
void event_add(event* Event, event_func Func);
void event_remove(event* Event, event_func Func);
void event_clear(event* Event);
void event_broadcast(event* Event, int argc, void* args);

#endif