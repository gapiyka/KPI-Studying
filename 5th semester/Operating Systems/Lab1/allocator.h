#pragma once
#include "block.h"
#include "kernel.h"
#if PAGE_SIZE > (SIZE_MAX / PAGE_SIZE)
#undef PAGE_SIZE
#define PAGE_SIZE (SIZE_MAX / PAGE_SIZE)
#endif
#define ARENA_DEFAULT_SIZE ((size_t)PAGE_SIZE * ARENA_PAGES)
#define ARENA_BLOCK_MAX_SIZE (ARENA_DEFAULT_SIZE - 2 * S_BLOCK_SIZE)

struct Arena {
    size_t size_curr;
    struct Arena* next_arena; 
};
#define S_ARENA_SIZE ROUND_BYTES(sizeof(struct Block))

static struct Arena* arena = NULL;
static struct Block* arena_start_block = NULL;

static inline void arena_init(struct Block*, size_t);
static inline struct Block* arena_to_header(struct Block*);
static inline struct Block* header_to_arena(struct Block*);
static int arena_alloc();
void* mem_alloc(size_t);
void mem_free(void*);
void* mem_realloc(void*, size_t);
void mem_show(const char*);