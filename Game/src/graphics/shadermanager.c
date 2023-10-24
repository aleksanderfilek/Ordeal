#include "shadermanager.h"
#include "../resources/shader.h"
#include "../thirdParty/GL/Gl.h"
#include <string.h>

static shader_manager* s_ShaderManager = NULL;

void shader_manager_init(shader_manager* Manager)
{
    if(s_ShaderManager)
        return;

    s_ShaderManager = Manager;

    memset(Manager, 0, sizeof(shader_manager));
}

void shader_manager_destroy(shader_manager* Manager)
{

}

void shader_bind(struct shader* Shader)
{
    if(s_ShaderManager->CurrentShader == Shader)
        return;

    if(Shader == NULL)
        return;

    s_ShaderManager->CurrentShader = Shader;
    glUseProgram(Shader->GlId);
}