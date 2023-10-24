#ifndef RESOURCES_H_
#define RESOURCES_H_

#include "../types/slotarray.h"
#include "../types/guid.h"

typedef struct resource_data
{
    void* Resource;
    void (*DestroryFunc)(void*);
} resource_data;

typedef struct resource_manager
{
    slot_array Slots;
    guid* Ids;
    resource_data* Resources;
    uint32_t Capacity;
    uint32_t Size;
} resource_manager;

void resource_manager_init(resource_manager* Manager, uint32_t Capacity);
void resource_manager_destroy(resource_manager* Manager);

void resource_manager_set_capacity(resource_manager* Manager, uint32_t Capacity);

void resource_manager_add(guid Id, void* Resource, void (*DestroryFunc)(void*));
void resource_manager_remove(guid Id);
void resource_manager_clear();
void* resource_manager_get(guid Id);

#endif