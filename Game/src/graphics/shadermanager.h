#ifndef SHADER_MANAGER_H_
#define SHADER_MANAGER_H_

typedef struct shader_manager
{
    struct shader* CurrentShader;
} shader_manager;

void shader_manager_init(shader_manager* Manager);
void shader_manager_destroy(shader_manager* Manager);

void shader_bind(struct shader* Shader);

#endif