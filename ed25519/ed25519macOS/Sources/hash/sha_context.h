#ifndef PROJECT_SHA_CONTEXT_H_
#define PROJECT_SHA_CONTEXT_H_

typedef struct sha_context_t {
    unsigned char opaque[224]; // size of context in bytes
} sha_context;

#endif  //  PROJECT_SHA_CONTEXT_H_
