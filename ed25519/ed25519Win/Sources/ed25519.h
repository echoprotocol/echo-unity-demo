#ifndef PROJECT_ED25519_HPP_
#define PROJECT_ED25519_HPP_

#import <Foundation/Foundation.h>

@interface ed25519 : NSObject

#if defined(__cplusplus)
extern "C" {
#endif

#define ed25519_pubkey_SIZE 32
#define ed25519_privkey_SIZE 32
#define ed25519_signature_SIZE 64

#include "ed25519_export.h"

/* type safe interface methods for ed25519 */

/**
 * @brief Generates a keypair. Depends on randombytes.h random generator.
 * @param sk [out] allocated buffer of ed25519_privkey_SIZE
 * @param pk [out] allocated buffer of ed25519_pubkey_SIZE
 * @return 0 if failed, non-0 otherwise
 */
ED25519_EXPORT int ed25519_create_keypair(unsigned char* sk, unsigned char* pk);

/**
 * @brief Creates a public key from given private key. For every private key
 * there is exactly one possible public key.
 *
 * Use this method to create a keypair from given randomness.
 *
 * @param sk [in] allocated buffer of ed25519_privkey_SIZE
 * @param pk [out] allocated buffer of ed25519_pubkey_SIZE
 */
ED25519_EXPORT void ed25519_derive_public_key(const unsigned char* sk, unsigned char* pk);

/**
* @brief Sign msg with keypair {pk, sk}
* @param sig [out] signature
* @param msg [in] message
* @param msglen [in] message size in bytes
* @param pk [in] public key
* @param sk [in] secret (private) key
*/
ED25519_EXPORT void ed25519_sign(unsigned char* sig, const unsigned char* msg, unsigned long long msglen, const unsigned char* pk, const unsigned char* sk);

/**
* Verifies given sig over given msg with public key pk
* @param sig [in] signature
* @param msg [in] message
* @param msglen [in] message size in bytes
* @param pk [in] public key
* @return 1 if signature is valid, 0 otherwise
*/
ED25519_EXPORT int ed25519_verify(const unsigned char* sig, const unsigned char* msg, unsigned long long msglen, const unsigned char* pk);

#if defined(__cplusplus)
}
#endif

#endif  //  PROJECT_ED25519_HPP_

@end
