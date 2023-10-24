#ifndef GL_DEBUG_H_
#define GL_DEBUG_H_

#ifdef DEBUG

int internal_gl_shaderCheckError(unsigned int shader, unsigned int pname, const char *file, int line);
#define gl_shader_check_error(shader, type) internal_gl_shaderCheckError(shader, type, __FILE__, __LINE__) 

int internal_glProgramCheckError(unsigned int program, unsigned int pname, const char *file, int line);
#define gl_program_check_error(program, type) internal_glProgramCheckError(program, type, __FILE__, __LINE__) 

unsigned int internal_gl_check_error(const char* file, int line);
#define gl_check_error() internal_gl_check_error(__FILE__, __LINE__)

#else

#define gl_shader_check_error(shader, type)
#define gl_program_check_error(program, type)
#define gl_check_error()

#endif

#endif