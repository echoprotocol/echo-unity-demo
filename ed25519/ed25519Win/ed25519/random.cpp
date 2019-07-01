#include "stdafx.h"
#include "errcode.h"
#include "random.h"
#include <fcntl.h>  // for open
#include <stdio.h>
#include "unistd.h"

int randombytes(unsigned char *p, int len) {
    int source = _open("/dev/random", O_RDONLY);
    if (source < 0) {
        return ED25519_ERROR; /* something went wrong */
    }
    
    int completed = 0;
    while (completed < len) {
        ssize_t result = _read(source, p + completed, len - completed);
        if (result < 0) {
            _close(source);
            return ED25519_ERROR;
        }
        completed += result;
    }
    
    _close(source);
    
    return ED25519_SUCCESS;
}
