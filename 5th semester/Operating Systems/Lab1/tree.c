#include "tree.h"

struct Node* node_right(struct Node* node) { return node->right; }
struct Node* node_left(struct Node* node) { return node->left; }

size_t node_height(struct Node* node) {
    if (node == NULL)
        return 0;
    return node->height;
}

size_t node_key(struct Node* node) {
    if (node == NULL)
        return 0;
    return node->key;
}

int node_balance(struct Node* node) {
    if (node == NULL)
        return 0;
    return node_height(node->left) - node_height(node->right);
}

struct Node* node_new(struct Node* node, size_t key) {
    node->key = key;
    node->left = NULL;
    node->right = NULL;
    node->height = 1;
    return node;
}

struct Node* node_rotate_right(struct Node* y) {
    struct Node* x = y->left;
    struct Node* branch = x->right;

    x->right = y;
    y->left = branch;

    y->height = 1 + MAX(node_height(y->left), node_height(y->right));
    x->height = 1 + MAX(node_height(x->left), node_height(x->right));

    return x;
}

struct Node* node_rotate_left(struct Node* x) {
    struct Node* y = x->right;
    struct Node* branch = y->left;

    y->left = x;
    x->right = branch;

    x->height = 1 + MAX(node_height(x->left), node_height(x->right));
    y->height = 1 + MAX(node_height(y->left), node_height(y->right));

    return y;
}

struct Node* node_insert(struct Node* node, size_t key, struct Node* node2) {
    if (node == NULL)
        return (node_new(node2, key));
    if (key < node->key)
        node->left = node_insert(node->left, key, node2);
    else if (key > node->key)
        node->right = node_insert(node->right, key, node2);
    else
        return node;

    // Update the balance factor of each node and Balance the tree
    node->height = 1 + MAX(node_height(node->left), node_height(node->right));

    int balance = node_balance(node);
    if (balance > 1 && key < node_key((struct Node*)node->left))
        return node_rotate_right(node);

    if (balance < -1 && key > node_key((struct Node*)node->right))
        return node_rotate_left(node);

    if (balance > 1 && key > node_key((struct Node*)node->left)) {
        node->left = node_rotate_left(node->left);
        return node_rotate_right(node);
    }

    if (balance < -1 && key < node_key((struct Node*)node->right)) {
        node->right = node_rotate_right(node->right);
        return node_rotate_left(node);
    }

    return node;
}

struct Node* node_min_value(struct Node* node) {
    struct Node* curr = node;

    while (curr->left != NULL)
        curr = curr->left;

    return curr;
}

struct Node* node_best_fit(struct Node* node, size_t size) {
    struct Node* curr = node;

    while (curr->left != NULL)
        if (curr->key > size)
            curr = curr->left;
        else break;

    return curr;
}

struct Node* node_delete(struct Node* node, size_t key) {
    if (node == NULL)
        return node;

    if (key < node->key)
        node->left = node_delete(node->left, key);
    else if (key > node->key)
        node->right = node_delete(node->right, key);
    else {
        if ((node->left == NULL) || (node->right == NULL)) {
            struct Node* temp = node->left ? node->left : node->right;

            if (temp == NULL) {
                temp = node;
                node = NULL;
            }
            else
                *node = *temp;
        }
        else {
            struct Node* temp = node_min_value(node->right);

            node->key = temp->key;

            node->right = node_delete(node->right, temp->key);
        }
    }

    if (node == NULL)
        return node;

    // Update the balance factor of each node and balance the tree
    node->height = 1 + MAX(node_height(node->left), node_height(node->right));

    int balance = node_balance(node);
    if (balance > 1 && node_balance(node->left) >= 0)
        return node_rotate_right(node);

    if (balance > 1 && node_balance(node->left) < 0) {
        node->left = node_rotate_left(node->left);
        return node_rotate_right(node);
    }

    if (balance < -1 && node_balance(node->right) <= 0)
        return node_rotate_left(node);

    if (balance < -1 && node_balance(node->right) > 0) {
        node->right = node_rotate_right(node->right);
        return node_rotate_left(node);
    }

    return node;
}

void node_show(struct Node* node) {
    if (node != NULL) {
        printf("key %zd     height %zd ", node->key, node->height);
        printf("\n");
        printf("LEFT ");
        node_show(node->left);
        printf("RIGHT ");
        node_show(node->right);
        printf("\n");
    }
}
