#include "slotarray.h"
#include <math.h>
#include <stdlib.h>

void slot_array_init(slot_array* Array, uint32_t Capacity)
{
    Array->Capacity = (uint32_t)ceilf((float)Capacity/32.0f); 
    if(Array->Capacity == 0)
        Array->Capacity = 1;

    Array->Chunks = calloc(Array->Capacity, sizeof(uint32_t));

    Array->Size = 0;
}

void slot_array_destroy(slot_array* Array)
{
    free(Array->Chunks);
}

uint32_t slot_array_get_slot(slot_array* Array)
{
    uint32_t maxValue = ~0;
    if(Array->Size < Array->Capacity * 32)
    {
        for(int i = 0; i < Array->Capacity; i++)
        {
            uint32_t chunk = Array->Chunks[i];
            if(chunk == maxValue)
                continue;

            for(int j = 0, b = 1; j < 32; j++, b<<1)
            {
                if(chunk & b)
                    continue;

                Array->Chunks[i] = chunk ^ b;
                Array->Size++;
                return 32 * i + j;
            }
        }
    }

    Array->Size++;
    Array->Capacity++;
    Array->Chunks = realloc(Array->Chunks, Array->Capacity * sizeof(uint32_t));
    return 32 * Array->Capacity;
}

void slot_array_free_slot(slot_array* Array, uint32_t SlotIndex)
{
    if(SlotIndex >= Array->Size)
        return 0;

    uint32_t chunkIndex = SlotIndex / 32;
    uint32_t indexInChunk = SlotIndex % 32;

    uint32_t chunk = Array->Chunks[chunkIndex];

    Array->Chunks[chunkIndex] = chunk ^ (1<<indexInChunk);
    Array->Size--;
}

uint8_t slot_array_is_slot_free(slot_array* Array, uint32_t SlotIndex)
{
    if(SlotIndex >= Array->Size)
        return 0;

    uint32_t chunkIndex = SlotIndex / 32;
    uint32_t indexInChunk = SlotIndex % 32;
    
    return Array->Chunks[chunkIndex] & (1<<indexInChunk);
}