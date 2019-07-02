#include <jni.h>
#include <string>
#include "ed25519.h"

extern "C" {
JNIEXPORT jint JNICALL
Java_com_example_ed25519_MainActivity_CreateKeypair(JNIEnv *env, jobject obj,
                                                    jbyteArray secret_k,
                                                    jbyteArray public_k) {
    int s_len = env->GetArrayLength(secret_k);
    int p_len = env->GetArrayLength(public_k);
    unsigned char *sk = new unsigned char[s_len];
    unsigned char *pk = new unsigned char[p_len];

    env->GetByteArrayRegion(secret_k, 0, s_len, reinterpret_cast<jbyte *>(sk));
    env->GetByteArrayRegion(public_k, 0, p_len, reinterpret_cast<jbyte *>(pk));

    jint result = ed25519_create_keypair(sk, pk);

    env->SetByteArrayRegion(secret_k, 0, s_len, reinterpret_cast<jbyte *>(sk));
    env->SetByteArrayRegion(public_k, 0, s_len, reinterpret_cast<jbyte *>(pk));

    delete[] sk;
    delete[] pk;

    return result;
}

JNIEXPORT void JNICALL
Java_com_example_ed25519_MainActivity_DerivePublicKey(JNIEnv *env, jobject obj,
                                                      jbyteArray secret_k,
                                                      jbyteArray public_k) {
    int s_len = env->GetArrayLength(secret_k);
    int p_len = env->GetArrayLength(public_k);

    unsigned char *sk = new unsigned char[s_len];
    unsigned char *pk = new unsigned char[p_len];

    env->GetByteArrayRegion(secret_k, 0, s_len, reinterpret_cast<jbyte *>(sk));
    env->GetByteArrayRegion(public_k, 0, p_len, reinterpret_cast<jbyte *>(pk));

    ed25519_derive_public_key(sk, pk);

    env->SetByteArrayRegion(public_k, 0, s_len, reinterpret_cast<jbyte *>(pk));

    delete[] sk;
    delete[] pk;
}

JNIEXPORT void JNICALL
Java_com_example_ed25519_MainActivity_Sign(JNIEnv *env, jobject obj, jbyteArray signature,
                                           jbyteArray message, jlong msg_len,
                                           jbyteArray public_k, jbyteArray secret_k) {
    int signature_len = env->GetArrayLength(signature);
    int message_len = env->GetArrayLength(message);
    int p_len = env->GetArrayLength(public_k);
    int s_len = env->GetArrayLength(secret_k);

    unsigned char *sig = new unsigned char[signature_len];
    unsigned char *msg = new unsigned char[message_len];
    unsigned char *pk = new unsigned char[p_len];
    unsigned char *sk = new unsigned char[s_len];

    env->GetByteArrayRegion(signature, 0, signature_len, reinterpret_cast<jbyte *>(sig));
    env->GetByteArrayRegion(message, 0, message_len, reinterpret_cast<jbyte *>(msg));
    env->GetByteArrayRegion(secret_k, 0, s_len, reinterpret_cast<jbyte *>(sk));
    env->GetByteArrayRegion(public_k, 0, p_len, reinterpret_cast<jbyte *>(pk));

    ed25519_sign(sig, msg, msg_len, pk, sk);

    env->SetByteArrayRegion(signature, 0, signature_len, reinterpret_cast<jbyte *>(sig));

    delete[] sig;
    delete[] msg;
    delete[] pk;
    delete[] sk;
}

JNIEXPORT jint JNICALL
Java_com_example_ed25519_MainActivity_Verify(JNIEnv *env, jobject obj, jbyteArray signature,
                                             jbyteArray message, jlong msg_len,
                                             jbyteArray public_k) {
    int signature_len = env->GetArrayLength(signature);
    int message_len = env->GetArrayLength(message);
    int p_len = env->GetArrayLength(public_k);

    unsigned char *sig = new unsigned char[signature_len];
    unsigned char *msg = new unsigned char[message_len];
    unsigned char *pk = new unsigned char[p_len];

    env->GetByteArrayRegion(signature, 0, signature_len, reinterpret_cast<jbyte *>(sig));
    env->GetByteArrayRegion(message, 0, message_len, reinterpret_cast<jbyte *>(msg));
    env->GetByteArrayRegion(public_k, 0, p_len, reinterpret_cast<jbyte *>(pk));

    jint result = ed25519_verify(sig, msg, msg_len, pk);

    delete[] sig;
    delete[] msg;
    delete[] pk;

    return result;
}
}