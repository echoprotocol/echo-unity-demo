//
// Created by Max on 6/27/19.
//

#ifndef ED25519ANDROID_RANDOM_H
#define ED25519ANDROID_RANDOM_H

/**
 * Fills preallocated buffer p of length len with random data.
 * @param[out] p buffer of length len
 * @param[in] len length
 * @return 0 if failed, non-0 otherwise
 * @note You should always check return code of randombytes
 */
int randombytes(unsigned char *p, int len);

#endif //ED25519ANDROID_RANDOM_H