//
// Created by Max on 6/27/19.
//

#include "ed25519.h"
#include "crypto_verify.h"
#include "old.h"
#include <string.h>
#include <stdio.h>
//#include "ge.h"
//#include "sc.h"
#include "random.h"
#include "errcode.h"
//#include "sha512.h"

int ed25519_create_keypair(unsigned char *sk, unsigned char *pk) {
    if (!randombytes(sk, ed25519_privkey_SIZE))
        return ED25519_ERROR;            /* RNG failed, not enough entropy */
//    ed25519_derive_public_key(sk, pk); /* fill with data */
    return ED25519_SUCCESS;            /* ok */
}