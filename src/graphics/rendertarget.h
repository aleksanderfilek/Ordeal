#ifndef RENDER_TARGET_H_
#define RENDER_TARGET_H_

#include "../math/vec2.h"
#include <stdint.h>

typedef struct render_target
{
	vec2i Size;
	uint32_t FrameBufferId;
	uint32_t BufferId;
} render_target;

void render_target_init(render_target* RenderTarget, vec2i Size);
void render_target_destroy(render_target* RenderTarget);

void render_target_bind(render_target* RenderTarget);
void render_target_bind_default(vec2i WindowSize);
void render_target_bind_as_texture(render_target* RenderTarget, uint32_t SlotIndex);
void render_target_blit_to_buffer(render_target* RenderTarget, uint32_t WriteBufferId, vec2i WriteBufferSize);

void render_target_clear(render_target* RenderTarget);

#endif