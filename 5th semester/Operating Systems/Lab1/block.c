#include "block.h"

size_t block_get_size_curr(struct Block* block) 
{ return block->size_curr & BLOCK_SIZE_CURR_MASK; }
size_t block_get_size_prev(struct Block* block) 
{ return block->size_prev & BLOCK_SIZE_PREV_MASK; }
bool block_get_flag_busy(struct Block* block) 
{ return block->size_curr & BLOCK_FLAG_BUSY; }
bool block_get_flag_first(struct Block* block) 
{ return block->size_curr & BLOCK_FLAG_FIRST; }
bool block_get_flag_last(struct Block* block) 
{ return block->size_prev & BLOCK_FLAG_LAST; }

void block_set_size_curr(struct Block* block, size_t size) 
{ block->size_curr = size | (block->size_curr & ~BLOCK_SIZE_CURR_MASK); }
void block_set_size_prev(struct Block* block, size_t size) 
{ block->size_prev = size | (block->size_prev & ~BLOCK_SIZE_PREV_MASK); }
void block_set_flag_busy(struct Block* block, bool flag) 
{
    block->size_curr = (flag) ? 
        block->size_curr | BLOCK_FLAG_BUSY : 
        block->size_curr & ~BLOCK_FLAG_BUSY; 
}
void block_set_flag_first(struct Block* block, bool flag) 
{
    block->size_curr = (flag) ?
        block->size_curr | BLOCK_FLAG_FIRST :
        block->size_curr & ~BLOCK_FLAG_FIRST;
}
void block_set_flag_last(struct Block* block, bool flag) 
{
    block->size_prev = (flag) ?
        block->size_prev | BLOCK_FLAG_LAST :
        block->size_prev & ~BLOCK_FLAG_LAST;
}

void block_init(struct Block* block)
{
    block_set_flag_busy(block, false);
    block_set_flag_first(block, false);
    block_set_flag_last(block, false);
}

void* block_to_payload(struct Block* block)
{
    return (char*)block + S_BLOCK_SIZE;
}

struct Block* payload_to_block(void* ptr)
{
    return (struct Block*)((char*)ptr - S_BLOCK_SIZE);
}

struct Block* block_next(struct Block* block)
{
    return (struct Block*)
        ((char*)block + S_BLOCK_SIZE + block_get_size_curr(block));
}
struct Block* block_prev(struct Block* block)
{
    return (struct Block*)
        ((char*)block - S_BLOCK_SIZE - block_get_size_prev(block));
}

void block_split(struct Block* block, size_t size)
{
    struct Block* block_new;
    size_t size_curr = block_get_size_curr(block);

    if (size_curr >= size + S_BLOCK_SIZE) {
        size_curr -= size + S_BLOCK_SIZE;
        block_set_size_curr(block, size);
        if (size_curr != 0) {
            block_new = block_next(block);
            block_init(block_new);
            block_set_size_curr(block_new, size_curr);
            block_set_size_prev(block_new, size);
            if (block_get_flag_last(block)) {
                block_set_flag_last(block, false);
                block_set_flag_last(block_new, true);
            }
            else {
                block_set_size_prev(block_next(block_new), size_curr);
            }
        }
    }
    block_set_flag_busy(block, true);
}

void block_merge(struct Block* block, struct Block* block_right) {
    size_t size_curr = block_get_size_curr(block) +
        block_get_size_curr(block_right) + S_BLOCK_SIZE;
    block_set_size_curr(block, size_curr);
    if (block_get_flag_last(block_right))
        block_set_flag_last(block, true);
    else 
        block_set_size_prev(block_next(block), size_curr);
}