#include "resources.h"
#include <stdlib.h>

#define RESOURCE_CAPACITY_CHUNK_SIZE 32

static resource_manager* s_ResourceManager = NULL;

void resource_manager_init(resource_manager* Manager, uint32_t Capacity)
{
    if(s_ResourceManager)
        return;

    s_ResourceManager = Manager;

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

            if(data->DestroryFunc == NULL)
                continue;

            data->DestroryFunc(data->Resource);
        }
    }

    Manager->Capacity = Capacity;
    Manager->Ids = realloc(Manager->Ids, Manager->Capacity * sizeof(guid));
    Manager->Resources = realloc(Manager->Resources, Manager->Capacity * sizeof(resource_data));
}

void resource_manager_add(guid Id, void* Resource, void (*DestroryFunc)(void*))
{
    uint32_t slotIndex = slot_array_get_slot(&s_ResourceManager->Slots);
    if(slotIndex >= s_ResourceManager->Capacity)
    {
        s_ResourceManager->Capacity += RESOURCE_CAPACITY_CHUNK_SIZE;
        s_ResourceManager->Ids = realloc(s_ResourceManager->Ids, s_ResourceManager->Capacity * sizeof(guid));
        s_ResourceManager->Resources = realloc(s_ResourceManager->Resources, s_ResourceManager->Capacity * sizeof(resource_data));
    }

    s_ResourceManager->Size++;
    s_ResourceManager->Ids[slotIndex] = Id;
    s_ResourceManager->Resources[slotIndex] = (resource_data){ Resource, DestroryFunc };
}

void resource_manager_remove(guid Id)
{
    uint32_t slotIndex;
    uint8_t found = 0;
    for(uint32_t i = 0; i < s_ResourceManager->Size; i++)
    {
        if(s_ResourceManager->Ids[i] == Id)
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

    resource_data* data = &s_ResourceManager->Resources[slotIndex];
    if(data->DestroryFunc != NULL)
    {
        data->DestroryFunc(data->Resource);
    }

    s_ResourceManager->Size--;
    s_ResourceManager->Ids[slotIndex] = 0;
    s_ResourceManager->Resources[slotIndex] = (resource_data){ NULL, 0 };
}

void resource_manager_clear()
{
    for(uint32_t i = 0, checkedSize = 0; i < s_ResourceManager->Capacity; i++)
    {
        resource_data* data = &s_ResourceManager->Resources[i];
        if(!data->Resource)
            continue;

        if(data->DestroryFunc == NULL)
            continue;

        data->DestroryFunc(data->Resource);

        checkedSize++;
        if(checkedSize == s_ResourceManager->Size)
            break;
    }

    s_ResourceManager->Size = 0;
    slot_array_clear(&s_ResourceManager->Slots);
}

void* resource_manager_get(guid Id)
{
    for(uint32_t i = 0; i < s_ResourceManager->Capacity; i++)
    {
        if(s_ResourceManager->Ids[i] == Id)
        {
            return s_ResourceManager->Resources[i].Resource;
        }
    }

    return NULL;
}