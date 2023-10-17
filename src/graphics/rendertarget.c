#include "rendertarget.h"
#include "../thirdParty/GL/Gl.h"

static uint32_t s_BindedBufferId = 0;

void render_target_init(render_target* RenderTarget, vec2i Size)
{
    RenderTarget->Size = Size;

    glGenFramebuffers(1, &RenderTarget->FrameBufferId);
    gl_check_error();
    glBindFramebuffer(GL_FRAMEBUFFER, RenderTarget->FrameBufferId);
    gl_check_error();
    glGenTextures(1, &RenderTarget->BufferId);
    glBindTexture(GL_TEXTURE_2D, RenderTarget->BufferId);
    glTexStorage2D(GL_TEXTURE_2D, 1, GL_RGBA8, Size.X, Size.Y);
    gl_check_error();

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glFramebufferTexture(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, RenderTarget->BufferId, 0);
    gl_check_error();

    GLenum bufferType = GL_COLOR_ATTACHMENT0;
    glDrawBuffers(1, &bufferType);
    gl_check_error();

    glBindFramebuffer(GL_FRAMEBUFFER, 0);
    gl_check_error();
}

void render_target_destroy(render_target* RenderTarget)
{
    glDeleteTextures(1, &RenderTarget->BufferId);
    glDeleteFramebuffers(1, &RenderTarget->FrameBufferId);
}

void render_target_bind(render_target* RenderTarget)
{
    glBindFramebuffer(GL_FRAMEBUFFER, RenderTarget->FrameBufferId);
    glViewport(0, 0, RenderTarget->Size.X, RenderTarget->Size.Y);
    s_BindedBufferId = RenderTarget->FrameBufferId;
}

void render_target_bind_default(vec2i WindowSize)
{
    glBindFramebuffer(GL_FRAMEBUFFER, 0);
    glViewport(0, 0, WindowSize.X, WindowSize.Y);
    s_BindedBufferId = 0;
}

void render_target_bind_as_texture(render_target* RenderTarget, uint32_t SlotIndex)
{
    glActiveTexture(GL_TEXTURE0 + SlotIndex);
    glBindTexture(GL_TEXTURE_2D, RenderTarget->BufferId);
}

void render_target_blit_to_buffer(render_target* RenderTarget, uint32_t WriteBufferId, vec2i WriteBufferSize)
{
    glBindFramebuffer(GL_READ_FRAMEBUFFER, RenderTarget->FrameBufferId);
    glBindFramebuffer(GL_DRAW_FRAMEBUFFER, WriteBufferId);

    glBlitFramebuffer(0, 0, RenderTarget->Size.X, RenderTarget->Size.Y, 0, 0, WriteBufferSize.X, WriteBufferSize.Y, GL_COLOR_BUFFER_BIT, GL_NEAREST);

    glBindFramebuffer(GL_FRAMEBUFFER, 0);
    s_BindedBufferId = 0;
}

void render_target_clear(render_target* RenderTarget)
{
    if(s_BindedBufferId != RenderTarget->FrameBufferId)
    {
        render_target_bind(RenderTarget);
    }
    glClear(GL_COLOR_BUFFER_BIT);
}