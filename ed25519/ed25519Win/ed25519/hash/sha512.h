#ifndef PROJECT_SHA512_H_
#define PROJECT_SHA512_H_

#include "sha_context.h"
    
#define SHA_512_SIZE 64  // bytes
    
    /**
     * Initializes context with specific for implementation size.
     * @param context [in]
     * @return 0 if error, non-0 otherwise.
     * @note some implementations may return bad code sometimes, some may not
     */
    extern int sha512_init(sha_context *context);
    
    /**
     * Updates hash state with given buffer
     * @param context [in]
     * @param in [in] input buffer with info to be hashed
     * @param inlen [in] buffer size
     * @return 0 if error, non-0 otherwise
     * @note some implementations may return bad code sometimes, some may not
     */
    extern int sha512_update(sha_context *context, const unsigned char *in,
                             unsigned long long inlen);
    
    /**
     * Finish hash calculation. Use this to store hash in output buffer (out).
     * @param context [in]
     * @param out [in] output buffer of exactly SHA_512_SIZE bytes
     * @return 0 if error, non-0 otherwise
     * @note some implementations may return bad code sometimes, some may not
     */
    extern int sha512_final(sha_context *context, unsigned char *out);
    
    /**
     * Inline hash calculation of sha512.
     * @param out [out] output buffer of exactly SHA_512_SIZE bytes
     * @param message [in] message buffer to be hashed
     * @param message_len [in] size of the message buffer
     * @return 0 if error, non-0 otherwise
     * @note some implementations may return bad code sometimes, some may not
     */
    extern int sha512(unsigned char *out, const unsigned char *message,
                      unsigned long long message_len);

#endif /* PROJECT_SHA512_H_ */
