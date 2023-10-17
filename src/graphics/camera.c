#include "camera.h"
#include "../thirdParty/GL/Gl.h"

#define CAMERA_UNIFORM_BLOCK_ID 0

static void camera_set_screen_matrix(camera* Camera);

void camera_init(camera* Camera, vec2f Position, vec2f Size)
{
    Camera->Position = Position;
    Camera->Size = Size;
    Camera->ViewMatrix = mat3_screen_view(Camera->Position, Camera->Size);

    glGenBuffers(1, &Camera->UniformBlockId);
	glBindBuffer(GL_UNIFORM_BUFFER, Camera->UniformBlockId);
	glBufferData(GL_UNIFORM_BUFFER, sizeof(mat3), NULL, GL_DYNAMIC_DRAW);
    glBufferSubData(GL_UNIFORM_BUFFER, 0, sizeof(mat3), &Camera->ViewMatrix.Column[0].X);
	glBindBuffer(GL_UNIFORM_BUFFER, 0);
	glBindBufferRange(GL_UNIFORM_BUFFER, CAMERA_UNIFORM_BLOCK_ID, Camera->UniformBlockId, 0, sizeof(mat3));
}

void camera_destroy(camera* Camera)
{
    glDeleteBuffers(1, &Camera->UniformBlockId);
}

void camera_set_position(camera* Camera, vec2f Position)
{
    Camera->Position = Position;
    camera_set_screen_matrix(Camera);
}

void camera_set_size(camera* Camera, vec2f Size)
{
    Camera->Size = Size;
    camera_set_screen_matrix(Camera);
}

void camera_set_screen_matrix(camera* Camera)
{
    Camera->ViewMatrix = mat3_screen_view(Camera->Position, Camera->Size);

    glBindBuffer(GL_UNIFORM_BUFFER, Camera->UniformBlockId);
    glBufferSubData(GL_UNIFORM_BUFFER, 0, sizeof(mat3), &Camera->ViewMatrix.Column[0].X);
    glBindBuffer(GL_UNIFORM_BUFFER, 0);
}