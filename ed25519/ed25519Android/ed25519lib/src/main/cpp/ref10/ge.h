//
// Created by Max on 6/28/19.
//

#ifndef ED25519ANDROID_GE_H
#define ED25519ANDROID_GE_H

#include "fe.h"

typedef struct {
    fe X;
    fe Y;
    fe Z;
} ge_p2;

typedef struct {
    fe X;
    fe Y;
    fe Z;
    fe T;
} ge_p3;

typedef struct {
    fe X;
    fe Y;
    fe Z;
    fe T;
} ge_p1p1;

typedef struct {
    fe yplusx;
    fe yminusx;
    fe xy2d;
} ge_precomp;

typedef struct {
    fe YplusX;
    fe YminusX;
    fe Z;
    fe T2d;
} ge_cached;

#define ge_frombytes_negate_vartime crypto_sign_ed25519_ref10_ge_frombytes_negate_vartime
#define ge_tobytes crypto_sign_ed25519_ref10_ge_tobytes
#define ge_p3_tobytes crypto_sign_ed25519_ref10_ge_p3_tobytes

#define ge_p2_0 crypto_sign_ed25519_ref10_ge_p2_0
#define ge_p3_0 crypto_sign_ed25519_ref10_ge_p3_0
#define ge_precomp_0 crypto_sign_ed25519_ref10_ge_precomp_0
#define ge_p3_to_p2 crypto_sign_ed25519_ref10_ge_p3_to_p2
#define ge_p3_to_cached crypto_sign_ed25519_ref10_ge_p3_to_cached
#define ge_p1p1_to_p2 crypto_sign_ed25519_ref10_ge_p1p1_to_p2
#define ge_p1p1_to_p3 crypto_sign_ed25519_ref10_ge_p1p1_to_p3
#define ge_p2_dbl crypto_sign_ed25519_ref10_ge_p2_dbl
#define ge_p3_dbl crypto_sign_ed25519_ref10_ge_p3_dbl

#define ge_madd crypto_sign_ed25519_ref10_ge_madd
#define ge_msub crypto_sign_ed25519_ref10_ge_msub
#define ge_add crypto_sign_ed25519_ref10_ge_add
#define ge_sub crypto_sign_ed25519_ref10_ge_sub
#define ge_scalarmult_base crypto_sign_ed25519_ref10_ge_scalarmult_base
#define ge_double_scalarmult_vartime crypto_sign_ed25519_ref10_ge_double_scalarmult_vartime

extern void ge_tobytes(unsigned char *, const ge_p2 *);

extern void ge_p3_tobytes(unsigned char *, const ge_p3 *);

extern int ge_frombytes_negate_vartime(ge_p3 *, const unsigned char *);

extern void ge_p2_0(ge_p2 *);

extern void ge_p3_0(ge_p3 *);

extern void ge_precomp_0(ge_precomp *);

extern void ge_p3_to_p2(ge_p2 *, const ge_p3 *);

extern void ge_p3_to_cached(ge_cached *, const ge_p3 *);

extern void ge_p1p1_to_p2(ge_p2 *, const ge_p1p1 *);

extern void ge_p1p1_to_p3(ge_p3 *, const ge_p1p1 *);

extern void ge_p2_dbl(ge_p1p1 *, const ge_p2 *);

extern void ge_p3_dbl(ge_p1p1 *, const ge_p3 *);

extern void ge_madd(ge_p1p1 *, const ge_p3 *, const ge_precomp *);

extern void ge_msub(ge_p1p1 *, const ge_p3 *, const ge_precomp *);

extern void ge_add(ge_p1p1 *, const ge_p3 *, const ge_cached *);

extern void ge_sub(ge_p1p1 *, const ge_p3 *, const ge_cached *);

extern void ge_scalarmult_base(ge_p3 *, const unsigned char *);

extern void
ge_double_scalarmult_vartime(ge_p2 *, const unsigned char *, const ge_p3 *, const unsigned char *);

#endif //ED25519ANDROID_GE_H