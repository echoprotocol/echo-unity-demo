//
// Created by Max on 6/28/19.
//

#ifndef ED25519ANDROID_FE_H
#define ED25519ANDROID_FE_H

#include <stdint.h>

typedef int32_t fe[10];

#define fe_frombytes crypto_sign_ed25519_ref10_fe_frombytes
#define fe_tobytes crypto_sign_ed25519_ref10_fe_tobytes
#define fe_copy crypto_sign_ed25519_ref10_fe_copy
#define fe_isnonzero crypto_sign_ed25519_ref10_fe_isnonzero
#define fe_isnegative crypto_sign_ed25519_ref10_fe_isnegative
#define fe_0 crypto_sign_ed25519_ref10_fe_0
#define fe_1 crypto_sign_ed25519_ref10_fe_1
#define fe_cmov crypto_sign_ed25519_ref10_fe_cmov
#define fe_add crypto_sign_ed25519_ref10_fe_add
#define fe_sub crypto_sign_ed25519_ref10_fe_sub
#define fe_neg crypto_sign_ed25519_ref10_fe_neg
#define fe_mul crypto_sign_ed25519_ref10_fe_mul
#define fe_sq crypto_sign_ed25519_ref10_fe_sq
#define fe_sq2 crypto_sign_ed25519_ref10_fe_sq2
#define fe_invert crypto_sign_ed25519_ref10_fe_invert
#define fe_pow22523 crypto_sign_ed25519_ref10_fe_pow22523

extern void fe_frombytes(fe, const unsigned char *);

extern void fe_tobytes(unsigned char *, const fe);

extern void fe_copy(fe, const fe);

extern int fe_isnonzero(const fe);

extern int fe_isnegative(const fe);

extern void fe_0(fe);

extern void fe_1(fe);

extern void fe_cmov(fe, const fe, unsigned int);

extern void fe_add(fe, const fe, const fe);

extern void fe_sub(fe, const fe, const fe);

extern void fe_neg(fe, const fe);

extern void fe_mul(fe, const fe, const fe);

extern void fe_sq(fe, const fe);

extern void fe_sq2(fe, const fe);

extern void fe_invert(fe, const fe);

extern void fe_pow22523(fe, const fe);

#endif //ED25519ANDROID_FE_H
