#pragma once
#include <stdlib.h>

#define MAX(a, b) ((a > b) ? a : b)

struct Node {
    size_t key;
    void* left;
    void* right;
    size_t height;
};

//#define S_NODE_SIZE ROUND_BYTES(sizeof(struct Node))
#define S_NODE_SIZE sizeof(struct Node)

struct Node* node_right(struct Node*);
struct Node* node_left(struct Node*);

size_t node_height(struct Node*);
size_t node_key(struct Node*);
int node_balance(struct Node*);
struct Node* node_new(size_t);
struct Node* node_rotate_right(struct Node*);
struct Node* node_rotate_left(struct Node*);
struct Node* node_insert(struct Node*, size_t, struct Node*);
struct Node* node_min_value(struct Node*);
struct Node* node_best_fit(struct Node*, size_t);
struct Node* node_delete(struct Node*, size_t);
void node_show(struct Node*);