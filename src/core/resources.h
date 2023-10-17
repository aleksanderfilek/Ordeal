#ifndef RESOURCES_H_
#define RESOURCES_H_

#include "../types/slotarray.h"
#include "../types/guid.h"

typedef struct resource_data
{
    void* Resource;
    uint8_t AutoDestroy;
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

void resource_manager_add(resource_manager* Manager, guid Id, void* Resource, uint8_t AutoDestroy);
void resource_manager_remove(resource_manager* Manager, guid Id);
void resource_manager_clear(resource_manager* Manager);

#endif