#ifndef SIC_STD_H
#define SIC_STD_H

#include <stdio.h>
#include <assert.h>
#include <string.h>
#include <stdlib.h>
#include <stdint.h>

#ifdef _WIN32
#include <windows.h>
#else
#include <strings.h>
#include <unistd.h>

typedef uint8_t BYTE;
typedef uint16_t WORD;
typedef uint32_t DWORD;
typedef int HANDLE;
typedef uint8_t byte;

#ifndef _MAX_PATH
#define _MAX_PATH 260
#endif

#ifndef INVALID_HANDLE_VALUE
#define INVALID_HANDLE_VALUE (-1)
#endif

#ifndef MAKEWORD
#define MAKEWORD(lo, hi) ((WORD)(((BYTE)(lo)) | ((WORD)((BYTE)(hi)) << 8)))
#endif

#ifndef strcmpi
#define strcmpi strcasecmp
#endif

static inline void Sleep(unsigned int ms)
{
    usleep((useconds_t)ms * 1000u);
}
#endif

#endif
