//
// Created by Max on 6/27/19.
//

#ifndef ED25519ANDROID_SHA_CONTEXT_H
#define ED25519ANDROID_SHA_CONTEXT_H

typedef struct sha_context_t {
    unsigned char opaque[224]; // size of context in bytes
} sha_context;

#endif //ED25519ANDROID_SHA_CONTEXT_H
