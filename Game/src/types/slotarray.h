#ifndef SLOT_ARRAY_H_
#define SLOT_ARRAY_H_

#include <stdint.h>

typedef struct slot_array
{
    uint32_t* Chunks;
    uint32_t Size;
    uint32_t Capacity;
} slot_array;

void slot_array_init(slot_array* Array, uint32_t Capacity);
void slot_array_destroy(slot_array* Array);

void slot_array_clear(slot_array* Array);

uint32_t slot_array_get_slot(slot_array* Array);
void slot_array_free_slot(slot_array* Array, uint32_t Slot);
uint8_t slot_array_is_slot_free(slot_array* Array, uint32_t Slot);

#endif