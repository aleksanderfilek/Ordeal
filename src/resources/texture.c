#include "texture.h"
#include "../thirdParty/GL/Gl.h"
#include <stdio.h>
#include <stdlib.h>
#include "../core/resources.h"

static int convert_filter_to_gl(texture_filter_method Method);
static int convert_wrapl_to_gl(texture_wrap_method Method);
static int convert_color_channel_to_gl(color_channel Channel);

texture* texture_load(guid Id, const char* Path)
{
    FILE* file;
    file = fopen(Path,"rb");

    uint32_t width, height, atlasWidth, atlasHeight;
    uint8_t channel, colorSpace, filter, wrap, generateMipmap;
    fread(&width, sizeof(uint32_t), 1, file);
    fread(&height, sizeof(uint32_t), 1, file);
    fread(&channel, sizeof(uint8_t), 1, file);
    fread(&colorSpace, sizeof(uint8_t), 1, file);
    fread(&filter, sizeof(uint8_t), 1, file);
    fread(&wrap, sizeof(uint8_t), 1, file);
    fread(&generateMipmap, sizeof(uint8_t), 1, file);
    fread(&atlasWidth, sizeof(uint32_t), 1, file);
    fread(&atlasHeight, sizeof(uint32_t), 1, file);
    uint32_t pixelDataSize = width * height * channel;
    uint8_t* pixelData = malloc(pixelDataSize);
    fread(pixelData, pixelDataSize, 1, file);
    fclose(file);

    texture* _texture = malloc(sizeof(texture));

    *_texture = (texture){
        .Size = (vec2i){width, height},
        .Channel = channel,
        .ColorSpace = colorSpace,
        .FilterMethod = filter,
        .WrapMethod = wrap,
        .GenerateMipmapsEnabled = generateMipmap,
        .AtlasSize = (vec2i){atlasWidth, atlasHeight}
    };

    glGenTextures(1, &_texture->GlId);
    glBindTexture(GL_TEXTURE_2D, _texture->GlId);

    int glFilter = convert_filter_to_gl(_texture->FilterMethod);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, glFilter);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, glFilter);
    gl_check_error();

    int glWrap = convert_wrapl_to_gl(_texture->WrapMethod);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, glWrap);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, glWrap);
    gl_check_error();

    int glChannel = convert_color_channel_to_gl(_texture->Channel);
    glTexImage2D(GL_TEXTURE_2D, 0, glChannel, _texture->Size.X, 
        _texture->Size.Y, 0, glChannel, GL_UNSIGNED_BYTE, pixelData);

    if (_texture->GenerateMipmapsEnabled) 
    {
        glGenerateMipmap(GL_TEXTURE_2D);
        gl_check_error();
    }

    glBindTexture(GL_TEXTURE_2D, 0);
    gl_check_error();

    free(pixelData);

    resource_manager_add(Id, _texture, texture_destroy);

    return _texture;
}

void texture_destroy(void* Texture)
{
    texture* _texture = (texture*)Texture;
    glDeleteTextures(1, &_texture->GlId);
    gl_check_error();
    free(_texture);
}

void texture_bind(texture* Texture, uint32_t SlotIndex)
{
    glActiveTexture(GL_TEXTURE0 + SlotIndex);
    gl_check_error();
    glBindTexture(GL_TEXTURE_2D, Texture->GlId);
    gl_check_error();
}

vec4f texture_get_sprite_rect(texture* Texture, int SpriteIndex)
{
    int horizontalSpriteCount = Texture->Size.X / Texture->AtlasSize.X;
    int verticalSpriteCount = Texture->Size.Y / Texture->AtlasSize.Y;

    int xCoord = SpriteIndex % horizontalSpriteCount;
    int yCoord = SpriteIndex / horizontalSpriteCount;

    vec4f result;
    result.Z = 1.0f / (float)(horizontalSpriteCount + 1);
    result.W = 1.0f / (float)(verticalSpriteCount + 1);
    result.X = result.Z * xCoord;
    result.Y = result.W * yCoord;

    return result;
}

int convert_filter_to_gl(texture_filter_method Method)
{
    int gl = 0;

    switch (Method)
    {
    case TEXTURE_FILTER_NEAREST:
        gl = GL_NEAREST;
        break;
    case TEXTURE_FILTER_LINEAR:
        gl = GL_LINEAR;
        break;
    }

    return gl;
}

int convert_wrapl_to_gl(texture_wrap_method Method)
{
    int gl = 0;

    switch (Method)
    {
    case TEXTURE_WRAP_REPEAT:
        gl = GL_REPEAT;
        break;
    case TEXTURE_WRAP_CLAMP_TO_EDGE:
        gl = GL_CLAMP_TO_EDGE;
        break;
    case TEXTURE_WRAP_CLAMP_TO_BORDER:
        gl = GL_CLAMP_TO_BORDER;
        break;
    case TEXTURE_WRAP_MIRRORED_REPEAT:
        gl = GL_MIRRORED_REPEAT;
        break;
    case TEXTURE_WRAP_MIRROR_CLAMP_TO_EDGE:
        gl = GL_MIRROR_CLAMP_TO_EDGE;
        break;
    }

    return gl;
}

int convert_color_channel_to_gl(color_channel Channel)
{
    int gl = 0;

    switch (Channel)
    {
    case COLOR_CHANNEL_RED:
        gl = GL_RED;
        break;
    case COLOR_CHANNEL_RGB:
        gl = GL_RGB;
        break;
    case COLOR_CHANNEL_RGBA:
        gl = GL_RGBA;
        break;
    }

    return gl;
}