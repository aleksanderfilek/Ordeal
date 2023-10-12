#ifndef COLOR_H_
#define COLOR_H_

typedef enum color_space
{
	COLOR_SPACE_SRGB = 0,
	COLOR_SPACE_LINEAR = 1
} color_space;

typedef enum color_channel
{
	COLOR_CHANNEL_RED = 1,
	COLOR_CHANNEL_RGB = 3,
	COLOR_CHANNEL_RGBA = 4
} color_channel;

typedef struct color
{
    float r, g, b, a;
} color;

#define COLOR_WHITE     (color){ 1.0f, 1.0f, 1.0f, 1.0f }
#define COLOR_RED       (color){ 1.0f, 0.0f, 0.0f, 0.0f }
#define COLOR_GREEN     (color){ 0.0f, 1.0f, 0.0f, 1.0f }
#define COLOR_BLUE      (color){ 0.0f, 0.0f, 0.0f, 1.0f }
#define COLOR_BLACK     (color){ 0.0f, 0.0f, 0.0f, 1.0f }

#endif