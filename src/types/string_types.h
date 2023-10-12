#ifndef STRING_TYPES_H_
#define STRING_TYPES_H_

#include <string.h>

// get capacity
// get length
// set
// copy
// compare

#define DEFINE_STRING(name, bytes_number)                               \
typedef struct name {                                                   \
    char Data[bytes_number];                                            \
} name;                                                                 \
                                                                        \
inline int name##_get_capacity() {                                      \
    return bytes_number;                                                \
}                                                                       \
                                                                        \
inline int name##_get_length(const name* str) {                         \
    return strlen(str->Data);                                           \
}                                                                       \
inline void name##set(name* dest, const char* str, size_t count) {      \
    strncpy(dest->Data, str, count);                                    \
}                                                                       \
inline void name##cpy(name* dest, const name* src) {                    \
    strncpy(dest->Data, src->Data, bytes_number);                       \
}                                                                       \
inline int name##cmp(const name* str1, const name* str2) {              \
    return strncmp(str1->Data, str2->Data, bytes_number);               \
}

DEFINE_STRING(string32,     32)
DEFINE_STRING(string64,     64)
DEFINE_STRING(string128,    128)
DEFINE_STRING(string256,    256)
DEFINE_STRING(string512,    512)
DEFINE_STRING(string1k,     1024)
DEFINE_STRING(string2k,     2048)
DEFINE_STRING(string4k,     4096)


#endif