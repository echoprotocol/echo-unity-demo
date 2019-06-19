#include "errcode.h"
#include "randombytes.h"
#include <fcntl.h>  // for open
#include <stdio.h>
#include <unistd.h>  // for read, ssize_t

ED25519_EXPORT int randombytes(unsigned char *p, int len) {
  int source = open("/dev/random", O_RDONLY);
  if (source < 0) {
    return ED25519_ERROR; /* something went wrong */
  }

  int completed = 0;
  while (completed < len) {
    ssize_t result = read(source, p + completed, len - completed);
    if (result < 0) {
      close(source);
      return ED25519_ERROR;
    }
    completed += result;
  }

  close(source);

  return ED25519_SUCCESS;
}
