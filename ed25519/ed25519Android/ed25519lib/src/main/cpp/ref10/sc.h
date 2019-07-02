//
// Created by Max on 6/28/19.
//

#ifndef ED25519ANDROID_SC_H
#define ED25519ANDROID_SC_H

#define sc_reduce crypto_sign_ed25519_ref10_sc_reduce
#define sc_muladd crypto_sign_ed25519_ref10_sc_muladd

extern void sc_reduce(unsigned char *);

extern void
sc_muladd(unsigned char *, const unsigned char *, const unsigned char *, const unsigned char *);

#endif //ED25519ANDROID_SC_H