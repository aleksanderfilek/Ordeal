#ifndef CAMERA_H_
#define CAMERA_H_

#include "../math/vec2.h"
#include "../math/mat3.h"
#include <stdint.h>

typedef struct camera
{
    vec2f Position;
    vec2f Size;
    mat3 ViewMatrix;
    uint32_t UniformBlockId;
} camera;

void camera_init(camera* Camera, vec2f Position, vec2f Size);
void camera_destroy(camera* Camera);

void camera_set_position(camera* Camera, vec2f Position);
void camera_set_size(camera* Camera, vec2f Size);


#endif