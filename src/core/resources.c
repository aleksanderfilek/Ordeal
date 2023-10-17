#include "resources.h"
#include <stdlib.h>

#define RESOURCE_CAPACITY_CHUNK_SIZE 32

void resource_manager_init(resource_manager* Manager, uint32_t Capacity)
{
    Manager->Capacity = Capacity;
    Manager->Size = 0;
    slot_array_init(&Manager->Slots, Capacity);
    Manager->Ids = calloc(Manager->Capacity, sizeof(guid));
    Manager->Resources = calloc(Manager->Capacity, sizeof(resource_data));
}

void resource_manager_destroy(resource_manager* Manager)
{
    resource_manager_clear(Manager);
    slot_array_destroy(&Manager->Slots);
    free(Manager->Ids);
    free(Manager->Resources);
}

void resource_manager_set_capacity(resource_manager* Manager, uint32_t Capacity)
{
    int capacityDifference = Capacity - Manager->Capacity;
    if(capacityDifference < 0)
    {
        for(int i = capacityDifference; i < Manager->Capacity; i++)
        {
            resource_data* data = &Manager->Resources[i];
            if(!data->Resource)
                continue;

            if(data->AutoDestroy == 0)
                continue;

            free(data->Resource);
        }
    }

    Manager->Capacity = Capacity;
    Manager->Ids = realloc(Manager->Ids, Manager->Capacity * sizeof(guid));
    Manager->Resources = realloc(Manager->Resources, Manager->Capacity * sizeof(resource_data));
}

void resource_manager_add(resource_manager* Manager, guid Id, void* Resource, uint8_t AutoDestroy)
{
    uint32_t slotIndex = slot_array_get_slot(&Manager->Slots);
    if(slotIndex >= Manager->Capacity)
    {
        Manager->Capacity += RESOURCE_CAPACITY_CHUNK_SIZE;
        Manager->Ids = realloc(Manager->Ids, Manager->Capacity * sizeof(guid));
        Manager->Resources = realloc(Manager->Resources, Manager->Capacity * sizeof(resource_data));
    }

    Manager->Size++;
    Manager->Ids[slotIndex] = Id;
    Manager->Resources[slotIndex] = (resource_data){ Resource, AutoDestroy };
}

void resource_manager_remove(resource_manager* Manager, guid Id)
{
    uint32_t slotIndex;
    uint8_t found = 0;
    for(uint32_t i = 0; i < Manager->Size; i++)
    {
        if(Manager->Ids[i] == Id)
        {
            found = 1;
            slotIndex = i;
            break;
        }
    }

    if(found == 0)
    {
        return;
    }

    resource_data* data = &Manager->Resources[slotIndex];
    if(data->AutoDestroy > 0)
    {
        free(data->Resource);
    }

    Manager->Size--;
    Manager->Ids[slotIndex] = 0;
    Manager->Resources[slotIndex] = (resource_data){ NULL, 0 };
}

void resource_manager_clear(resource_manager* Manager)
{
    for(uint32_t i = 0, checkedSize = 0; i < Manager->Capacity; i++)
    {
        resource_data* data = &Manager->Resources[i];
        if(!data->Resource)
            continue;

        if(data->AutoDestroy == 0)
            continue;

        free(data->Resource);

        checkedSize++;
        if(checkedSize == Manager->Size)
            break;
    }

    Manager->Size = 0;
    slot_array_clear(&Manager->Slots);
}