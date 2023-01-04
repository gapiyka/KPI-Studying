#include "allocator.h"
#include "config.h"
#include "block.h"

static inline void arena_init(struct Block* block, size_t size)
{
    block_init(block);
    block_set_size_curr(block, size);
    block_set_size_prev(block, (size_t)0);
    block_set_flag_first(block, true);
    block_set_flag_last(block, true);
}

static inline struct Block* arena_to_block(struct Block* arena)
{
    return arena + S_BLOCK_SIZE;
}

static inline struct Block* block_to_arena(struct Block* block)
{
    return block - S_BLOCK_SIZE;
}

static int arena_alloc()
{
    arena = kernel_mem_alloc(ARENA_DEFAULT_SIZE);
    if (arena == NULL)
        return -1;
    arena_start_block = arena + S_BLOCK_SIZE;
    arena_init(arena_start_block, ARENA_DEFAULT_SIZE - S_BLOCK_SIZE);
    return 0;
}

void* mem_alloc(size_t size)
{
    struct Block* block;

    if (arena == NULL)
        if (arena_alloc() < 0) return NULL;
    if (size > ARENA_BLOCK_MAX_SIZE) return NULL;
    if (size == 0) return NULL;
    size = ROUND_BYTES(size);
    for (block = arena;; block = block_next(block)) {
        if (!block_get_flag_busy(block) && block_get_size_curr(block) > size) {
            block_split(block, size);
            return block_to_payload(block);
        }
        if (block_get_flag_last(block)) break;
    }
    return NULL;
}

void mem_free(void* ptr)
{
    if (ptr == NULL) return;
    struct Block* block, * block_neighbor;
    block = payload_to_block(ptr);
    block_set_flag_busy(block, false);
    if (!block_get_flag_last(block)) {
        block_neighbor = block_next(block);
        if (!block_get_flag_busy(block_neighbor)) {
            block_merge(block, block_neighbor);
        }
    }
    if (!block_get_flag_first(block)) {
        block_neighbor = block_prev(block);
        if (!block_get_flag_busy(block_neighbor)) {
            block_merge(block_neighbor, block);
        }
    }
    if (!block_get_flag_busy(arena) &&
        block_get_flag_first(arena) &&
        block_get_flag_last(arena)) {
        kernel_mem_free(arena, ARENA_DEFAULT_SIZE);
        arena = NULL;
    }
        
}

void* mem_realloc(void* ptr, size_t size) {
    struct Block *block1, *block2; 
    void* new_ptr;
    size_t size_curr, size_start; 
    bool flag_merged = false;
    if (ptr == NULL) return mem_alloc(size); 
    if (size == 0) {
        mem_free(ptr);
        return NULL;
    }
    if (size > ARENA_BLOCK_MAX_SIZE) return NULL;
    size = ROUND_BYTES(size); 
    block1 = payload_to_block(ptr); 
    size_start = block_get_size_curr(block1);
    if (size == size_start) return ptr;
    if (!block_get_flag_last(block1)) {
        block2 = block_next(block1);
        if (!block_get_flag_busy(block2)) {
            block_merge(block1, block2);
        }
    }
    if (!block_get_flag_first(block1)) {
        block2 = block_prev(block1);
        if (!block_get_flag_busy(block2) && 
            (size < block_get_size_curr(block1) + 
                block_get_size_curr(block2) + S_BLOCK_SIZE)) {
            block_merge(block2, block1);
            memcpy(block_to_payload(block2), ptr, size_start);
            flag_merged = true;
            block1 = block2;
        }
    }
    size_curr = block_get_size_curr(block1);
    if (size < size_curr) {
        block_split(block1, size);
        return block_to_payload(block1);
    }
    new_ptr = mem_alloc(size);
    if (new_ptr) {
        memcpy(new_ptr, ptr, size_start);
        if (!flag_merged) mem_free(ptr);
        return new_ptr;
    }

    return NULL;
}

void mem_show(const char* msg)
{
    struct Block* block;
    printf("%s:\n", msg);
    if (arena == NULL) {
        printf("Arena was not created\n");
        return;
    }
    for (block = arena;; block = block_next(block)) {
        printf("%p %s %10zu %10zu%s%s\n",
            block,
            block_get_flag_busy(block) ? "busy" : "free",
            block_get_size_curr(block), block_get_size_prev(block),
            block_get_flag_first(block) ? " first" : "",
            block_get_flag_last(block) ? " last" : "");
        if (block_get_flag_last(block)) break;
    }
}

