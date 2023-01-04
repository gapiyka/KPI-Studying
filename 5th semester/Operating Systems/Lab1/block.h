#pragma once
#include <memory.h>
#include "config.h"
#include "allocator_impl.h"

// impl below will work if ALIGN > 2
#define BLOCK_FLAG_BUSY (size_t)0x1
#define BLOCK_FLAG_FIRST (size_t)0x2
#define BLOCK_FLAG_LAST (size_t)0x1
#define BLOCK_SIZE_CURR_MASK ~(BLOCK_FLAG_BUSY | BLOCK_FLAG_FIRST)
#define BLOCK_SIZE_PREV_MASK ~(BLOCK_FLAG_LAST)

struct Block {
    size_t size_curr; // current size + in mask: flag_busy | flag_first
    size_t size_prev; // previous size + in mask: flag_last
};

#define S_BLOCK_SIZE ROUND_BYTES(sizeof(struct Block))

size_t block_get_size_curr(struct Block*);
size_t block_get_size_prev(struct Block*);
bool block_get_flag_busy(struct Block*);
bool block_get_flag_first(struct Block*);
bool block_get_flag_last(struct Block*);

void block_set_size_curr(struct Block*, size_t);
void block_set_size_curr(struct Block*, size_t);
void block_set_flag_busy(struct Block*, bool);
void block_set_flag_first(struct Block*, bool);
void block_set_flag_last(struct Block*, bool);

void block_init(struct Block*);

void* block_to_payload(struct Block*);
struct Block* payload_to_block(void*);

struct Block* block_next(struct Block*);
struct Block* block_prev(struct Block*);

void block_split(struct Block*, size_t);
void block_merge(struct Block*, struct Block*);