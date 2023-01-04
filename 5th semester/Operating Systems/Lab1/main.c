#include "allocator.h"

#define N 10

struct T {
    void* ptr;
    size_t size;
    unsigned int checksum;
};

static void buf_fill(unsigned char* c, size_t size)
{
    while (size--)
        *c++ = (unsigned char)rand();
}

static unsigned int buf_checksum(unsigned char* c, size_t size)
{
    unsigned int checksum = 0;

    while (size--)
        checksum = (checksum << 5) ^ (checksum >> 3) ^ *c++;
    return checksum;
}

static void* buf_alloc(size_t size)
{
    void* ptr;

    ptr = mem_alloc(size);
    if (ptr != NULL)
        buf_fill(ptr, size);
    return ptr;
}

static void tester(unsigned int seed)
{
    const size_t SZ_MAX = 10000;
    const size_t SZ_MIN = 10;
    const unsigned long M = 1000;
    //const size_t N = 10;
    struct T t[N];
    void* ptr;
    size_t idx, size, size_min;
    unsigned int checksum;

    seed = (seed == NULL) ? time(NULL) : seed;
    srand(seed);
    for (idx = 0; idx < N; ++idx)
        t[idx].ptr = NULL;
    for (unsigned long i = 0; i < M; ++i) {
        mem_show("----------------------------------------------");
        for (idx = 0; idx < N; ++idx) {
            if (t[idx].ptr != NULL) {
                if (t[idx].checksum != buf_checksum(t[idx].ptr, t[idx].size)) {
                    printf("1. Checksum check failed at [%p]\n", t[idx].ptr);
                    return;
                }
            }
        }
        idx = (size_t)rand() % N;
        if (t[idx].ptr == NULL) {
            size = ((size_t)rand() % (SZ_MAX - SZ_MIN)) + SZ_MIN;
            ptr = buf_alloc(size);
            if (ptr != NULL) {
                t[idx].ptr = ptr;
                t[idx].size = size;
                t[idx].checksum = buf_checksum(ptr, size);
            }
        }
        else {
            if (rand() & 1) {
                size = ((size_t)rand() % (SZ_MAX - SZ_MIN)) + SZ_MIN;
                size_min = size < t[idx].size ? size : t[idx].size;
                checksum = buf_checksum(t[idx].ptr, size_min);
                ptr = mem_realloc(t[idx].ptr, size);
                if (ptr != NULL) {
                    if (checksum != buf_checksum(ptr, size_min)) {
                        printf("2. Checksum check failed at [%p]\n", ptr);
                        return;
                    }
                    t[idx].ptr = ptr;
                    t[idx].size = size;
                    t[idx].checksum = buf_checksum(ptr, size);
                }
                else printf("Have not free %u in mem to realloc [%p]\n",
                    size, t[idx].ptr);
            }
            else {
                mem_free(t[idx].ptr);
                t[idx].ptr = NULL;
            }
        }
    }
    for (size_t idx = 0; idx < N; ++idx) {
        if (t[idx].ptr != NULL) {
            mem_show("----------------------------------------------");
            if (t[idx].checksum != buf_checksum(t[idx].ptr, t[idx].size)) {
                printf("3. Checksum check failed at [%p]\n", t[idx].ptr);
                return;
            }
            mem_free(t[idx].ptr);
            t[idx].ptr = NULL;
        }
    }
    mem_show("END----------------------------------------------");
}



int main(void)
{
    //printf("Alignment of max_align_t is %zu (%#zx)\n", ALIGN, ALIGN);
    //printf("S_BLOCK_SIZE: %u\n", S_BLOCK_SIZE);
    void *ptr, *ptr2, *ptr3, *ptr4, *ptr5;
    /*
    // FULL PROCCESS TESTS
    ptr = mem_alloc(5);
    mem_show("mem_alloc(5) -- ptr");
    ptr2 = mem_alloc(32);
    mem_show("mem_alloc(32) -- ptr2");
    ptr3 = mem_alloc(64);
    mem_show("mem_alloc(64) -- ptr3");
    ptr4 = mem_alloc(65000);
    mem_show("mem_alloc(65000) -- ptr4");
    ptr5 = mem_alloc(1);
    mem_show("mem_alloc(1) -- ptr5");

    printf("\nFREE_TESTS:\n");
    mem_free(ptr3);
    mem_show("mem_free(ptr3) -- bcb");
    mem_free(ptr2);
    mem_show("mem_free(ptr2) -- bcf");
    mem_free(ptr4);
    mem_show("mem_free(ptr4) -- fcb");
    mem_free(ptr);
    mem_show("mem_free(ptr) -- first one");
    mem_free(ptr5);
    mem_show("mem_free(ptr5) -- last one + fcf");

    printf("\nSIZE_MAX_TESTS:\n");
    ptr5 = mem_alloc(0);
    mem_show("mem_alloc(0) -- ptr5");
    ptr = mem_alloc(SIZE_MAX);
    mem_show("mem_alloc(SIZE_MAX) -- ptr");
    ptr2 = mem_alloc(ARENA_BLOCK_MAX_SIZE);
    mem_show("mem_alloc(ARENA_BLOCK_MAX_SIZE) -- ptr2");
    ptr3 = mem_alloc(8);
    mem_show("mem_alloc(8) that greater than possible free size -- ptr3");
    mem_free(ptr2);
    mem_show("mem_free(ptr2)");

    printf("\nREALLOC_TESTS(init):\n");
    ptr = mem_alloc(5);
    mem_show("mem_alloc(5) -- ptr");
    ptr2 = mem_alloc(32);
    mem_show("mem_alloc(32) -- ptr2");
    ptr3 = mem_alloc(64);
    mem_show("mem_alloc(64) -- ptr3");
    ptr4 = mem_alloc(12);
    mem_show("mem_alloc(12) -- ptr4");
    ptr5 = mem_alloc(8);
    mem_show("mem_alloc(8) -- ptr5");

    printf("\nREALLOC_TESTS(dumps):\n");
    ptr = mem_realloc(ptr, 100); // try change 100 to 2, to check fcf in ptr5
    mem_show("mem_realloc(ptr, 100)");
    ptr2 = mem_realloc(ptr2, 16);
    mem_show("mem_realloc(ptr2, 16)");
    mem_free(ptr2);
    mem_show("mem_free(ptr2)");
    ptr3 = mem_realloc(ptr3, 24);
    mem_show("mem_realloc(ptr3, 24)");
    mem_free(ptr4);
    mem_show("mem_free(ptr4)");
    ptr5 = mem_realloc(ptr5, 100);
    mem_show("mem_realloc(ptr5, 100)");
    */

    tester(1);
}