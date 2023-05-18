/**
 * Паралельне програмування
 * Лабораторна робота: №5
 * Оцінка: A
 * Варіант: 9
 * Завдання: a = min(B*MZ + Z*(MR*MX))
 * • ПВВ1 – a, MX, B, Z , MZ, MR
 * • ПВВ2 – -
 * • ПВВ3 – -
 * • ПВВ4 – -
 * Виконав: Гапій Денис Едуардович ІП-05
 * Дата: 07.05.2023
 **/

using namespace std;

#pragma region Includes
#include <iostream>
#include <string>
#include <chrono>
#include "mpi.h"
#include "matrix.h" 
#pragma endregion

#pragma region Constants
const int N = 16;
const int P = 8;
const int H = N / P;
const int DEFAULT = 1;
#pragma endregion

struct arrH
{
    int arr[H];
};

struct arrH calculation1(int Bh[], int Zh[], Matrix <int> MXh, Matrix <int> MZh, Matrix <int> MRh)
{
    arrH Ah; 
    for (int i = 0; i < H; i++) {
        int part1 = 0;
        int part2 = 0;
        for (int j = 0; j < N; j++) {
            part1 += Bh[i] * MZh(i, j);
            for (int k = 0; k < N; k++) {
                part2 += Zh[i] * MRh(i, j) * MXh(i, j);
            }
        }
        Ah.arr[i] = part1 + part2;
    }
    return Ah;
}

int calculation2(int Ah[]) {
    int min = INT_MAX;
    for (int i = 0; i < H; i++) 
        if (Ah[i] < min)
            min = Ah[i];
    return min;
}

int calculation3(int a1, int a2) {
    return a1 < a2 ? a1 : a2;
}

int main(int argc, char* argv[])
{
    // Declare new datatype for matrix
    MPI_Datatype row_type;
    // Initialize the MPI environment
    MPI_Init(&argc, &argv);
    //Create row_type
    MPI_Type_contiguous(N, MPI_INT, &row_type); 
    MPI_Type_commit(&row_type);
    int a;
    arrH A;
    int B[N];
    int Z[N];
    Matrix <int> MX = Matrix <int>(N, N);
    Matrix <int> MZ = Matrix <int>(N, N);
    Matrix <int> MR = Matrix <int>(N, N);

    MPI_Comm graph_cart;
    MPI_Comm graph_comm;
    int nnodes = 8; /* number of nodes */
    int index[8] = { 3, 5, 7, 9, 11, 13, 14, 15 }; /* index definition */
    int edges[15] = { 1, 2, 3, 0, 3, 0, 4, 1, 5, 2, 6, 3, 7, 4 ,5 }; /* edges definition */
    int reorder = 1; /* allows processes reordered for efficiency */
    MPI_Graph_create(MPI_COMM_WORLD, nnodes, index, edges, reorder, &graph_comm);

    const int NUM_DIMS = 2;
    int periods[NUM_DIMS]{ 0, 0 };
    int dims[NUM_DIMS]{2, 4};
    reorder = 0;
    MPI_Dims_create(nnodes, NUM_DIMS, dims);
    MPI_Cart_create(MPI_COMM_WORLD, NUM_DIMS, dims, periods, reorder, &graph_cart);
    // Get the rank of the calling process
    int t_id;
    MPI_Comm_rank(MPI_COMM_WORLD, &t_id);
    if (t_id == 0) { // T1
        printf("Lab started\n");
        printf("T1 start\n");
        auto start = chrono::high_resolution_clock::now();
        //Введення MX, B, Z, MZ, MR
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                MX(i,j) = DEFAULT;
                MZ(i,j) = DEFAULT;
                MR(i,j) = DEFAULT;
            }
            B[i] = DEFAULT;
            Z[i] = DEFAULT;
        }
        //Передати MX4h, B4h, Z4h, MZ4h, MR4h в T2
        MPI_Send(B, N, MPI_INT, 1, 0, graph_cart);
        MPI_Send(Z, N, MPI_INT, 1, 0, graph_cart);
        MPI_Send(&MX(H, 0), H * 4 * N, MPI_INT, 1, 0, graph_cart);
        MPI_Send(&MZ(H, 0), H * 4 * N, MPI_INT, 1, 0, graph_cart);
        MPI_Send(&MR(0, 0), N * N, MPI_INT, 1, 0, graph_cart);
        ////Передати MX3h, B3h, Z3h, MZ3h, MR3h в T3
        MPI_Send(B, N, MPI_INT, 2, 1, graph_cart);
        MPI_Send(Z, N, MPI_INT, 2, 1, graph_cart);
        MPI_Send(&MX(5 * H, 0), H * 3 * N, MPI_INT, 2, 1, graph_cart);
        MPI_Send(&MZ(5 * H, 0), H * 3 * N, MPI_INT, 2, 1, graph_cart);
        MPI_Send(&MR(0, 0), N * N, MPI_INT, 2, 1, graph_cart);
        //Обчислення 1[Ah = Bh * MZh + Zh * (MRh * MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2[a_1 = min(Ah)]
        a = calculation2(A.arr);
        //Прийняти a з T2
        int a2;
        MPI_Recv(&a2, 1, MPI_INT, 1, 12, graph_cart, MPI_STATUS_IGNORE);
        //Прийняти a з T3
        int a3;
        MPI_Recv(&a3, 1, MPI_INT, 2, 13, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 3[a = min(a_1, a(2), a(3))]
        a = calculation3(a, a2);
        a = calculation3(a, a3);
        //Виведення a
        printf("Answer is: %d\n", a);
        printf("T1 end\n");
        auto stop = chrono::high_resolution_clock::now();
        auto duration = chrono::duration_cast<chrono::milliseconds>(stop - start);
        printf("Lab finished in %d ms\n", duration.count());
    }
    else if (t_id == 1) { // T2
        printf("T2 start\n");
        //Прийняти MX4h, B4h, Z4h, MZ4h, MR4h з T1
        int size_h = H * 4;
        MPI_Recv(B, N, MPI_INT, 0, 0, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(Z, N, MPI_INT, 0, 0, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MX(0, 0), size_h * N, MPI_INT, 0, 0, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MZ(0, 0), size_h * N, MPI_INT, 0, 0, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MR(0, 0), N * N, MPI_INT, 0, 0, graph_cart, MPI_STATUS_IGNORE);
        //Передати MX3h, B3h, Z3h, MZ3h, MR3h в T4
        MPI_Send(B, N, MPI_INT, 3, 2, graph_cart);
        MPI_Send(Z, N, MPI_INT, 3, 2, graph_cart);
        MPI_Send(&MX(H, 0), H * 3 * N, MPI_INT, 3, 2, graph_cart);
        MPI_Send(&MZ(H, 0), H * 3 * N, MPI_INT, 3, 2, graph_cart);
        MPI_Send(&MR(0, 0), N * N, MPI_INT, 3, 2, graph_cart);
        //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2 [a_2= min(Ah)]
        a = calculation2(A.arr);
        //Прийняти a з T4
        int a4;
        MPI_Recv(&a4, 1, MPI_INT, 3, 3, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 3 [a= min(a, a_2)]
        a = calculation3(a, a4);
        //Передати а в T1
        MPI_Send(&a, 1, MPI_INT, 0, 12, graph_cart);
        printf("T2 end\n");
    }
    else if (t_id == 2) { // T3
        printf("T3 start\n");
        //Прийняти MX3h, B3h, Z3h, MZ3h, MR3h з T1
        int size_h = H * 3;
        MPI_Recv(B, N, MPI_INT, 0, 1, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(Z, N, MPI_INT, 0, 1, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MX(0, 0), size_h * N, MPI_INT, 0, 1, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MZ(0, 0), size_h * N, MPI_INT, 0, 1, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MR(0, 0), N * N, MPI_INT, 0, 1, graph_cart, MPI_STATUS_IGNORE);
        //Передати MX2h, B2h, Z2h, MZ2h, MR2h в T5
        MPI_Send(B, N, MPI_INT, 4, 4, graph_cart);
        MPI_Send(Z, N, MPI_INT, 4, 4, graph_cart);
        MPI_Send(&MX(H, 0), H * 2 * N, MPI_INT, 4, 4, graph_cart);
        MPI_Send(&MZ(H, 0), H * 2 * N, MPI_INT, 4, 4, graph_cart);
        MPI_Send(&MR(0, 0), N * N, MPI_INT, 4, 4, graph_cart);
        //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2 [a_3= min(Ah)]
        a = calculation2(A.arr);
        //Прийняти a з T5
        int a5;
        MPI_Recv(&a5, 1, MPI_INT, 4, 5, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 3 [a= min(a, a_3)]
        a = calculation3(a, a5);
        //Передати а в T1
        MPI_Send(&a, 1, MPI_INT, 0, 13, graph_cart);
        printf("T3 end\n");
    }
    else if (t_id == 3) { // T4
        printf("T4 start\n");
        //Прийняти MX3h, B3h, Z3h, MZ3h, MR3h з T2
        int size_h = H * 3;
        MPI_Recv(B, N, MPI_INT, 1, 2, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(Z, N, MPI_INT, 1, 2, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MX(0, 0), size_h* N, MPI_INT, 1, 2, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MZ(0, 0), size_h* N, MPI_INT, 1, 2, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MR(0, 0), N* N, MPI_INT, 1, 2, graph_cart, MPI_STATUS_IGNORE);
        //Передати MX2h, B2h, Z2h, MZ2h, MR2h в T6
        MPI_Send(B, N, MPI_INT, 5, 6, graph_cart);
        MPI_Send(Z, N, MPI_INT, 5, 6, graph_cart);
        MPI_Send(&MX(H, 0), H * 2 * N, MPI_INT, 5, 6, graph_cart);
        MPI_Send(&MZ(H, 0), H * 2 * N, MPI_INT, 5, 6, graph_cart);
        MPI_Send(&MR(0, 0), N * N, MPI_INT, 5, 6, graph_cart);
        //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2 [a_4= min(Ah)]
        a = calculation2(A.arr);
        //Прийняти a з T6
        int a6;
        MPI_Recv(&a6, 1, MPI_INT, 5, 9, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 3 [a= min(a, a_4)]
        a = calculation3(a, a6);
        //Передати а в T2
        MPI_Send(&a, 1, MPI_INT, 1, 3, graph_cart);
        printf("T4 end\n");
    }
    else if (t_id == 4) { // T5
        printf("T5 start\n");
        //Прийняти MX2h, B2h, Z2h, MZ2h, MR2h з T3
        int size_h = H * 2;
        MPI_Recv(B, N, MPI_INT, 2, 4, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(Z, N, MPI_INT, 2, 4, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MX(0, 0), size_h* N, MPI_INT, 2, 4, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MZ(0, 0), size_h* N, MPI_INT, 2, 4, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MR(0, 0), N* N, MPI_INT, 2, 4, graph_cart, MPI_STATUS_IGNORE);
        //Передати MXh, Bh, Zh, MZh, MRh в T7
        MPI_Send(B, N, MPI_INT, 6, 7, graph_cart);
        MPI_Send(Z, N, MPI_INT, 6, 7, graph_cart);
        MPI_Send(&MX(H, 0), H* N, MPI_INT, 6, 7, graph_cart);
        MPI_Send(&MZ(H, 0), H* N, MPI_INT, 6, 7, graph_cart);
        MPI_Send(&MR(0, 0), N* N, MPI_INT, 6, 7, graph_cart);
        //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2 [a_5= min(Ah)]
        a = calculation2(A.arr);
        //Прийняти a з T7
        int a7;
        MPI_Recv(&a7, 1, MPI_INT, 6, 8, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 3 [a= min(a, a_5)]
        int a_mins[2]{ a, a7 };
        a = calculation3(a, a7);
        //Передати а в T3
        MPI_Send(&a, 1, MPI_INT, 2, 5, graph_cart);
        printf("T5 end\n");
    }
    else if (t_id == 5) { // T6
       printf("T6 start\n");
       //Прийняти MX2h, B2h, Z2h, MZ2h, MR2h з T4
       int size_h = H * 2;
       MPI_Recv(B, N, MPI_INT, 3, 6, graph_cart, MPI_STATUS_IGNORE);
       MPI_Recv(Z, N, MPI_INT, 3, 6, graph_cart, MPI_STATUS_IGNORE);
       MPI_Recv(&MX(0, 0), size_h* N, MPI_INT, 3, 6, graph_cart, MPI_STATUS_IGNORE);
       MPI_Recv(&MZ(0, 0), size_h* N, MPI_INT, 3, 6, graph_cart, MPI_STATUS_IGNORE);
       MPI_Recv(&MR(0, 0), N* N, MPI_INT, 3, 6, graph_cart, MPI_STATUS_IGNORE);
       //Передати MXh, Bh, Zh, MZh, MRh в T8
       MPI_Send(B + H, H, MPI_INT, 7, 10, graph_cart);
       MPI_Send(Z + H, H, MPI_INT, 7, 10, graph_cart);
       MPI_Send(&MX(H, 0), H* N, MPI_INT, 7, 10, graph_cart);
       MPI_Send(&MZ(H, 0), H* N, MPI_INT, 7, 10, graph_cart);
       MPI_Send(&MR(0, 0), N* N, MPI_INT, 7, 10, graph_cart);
       //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
       A = calculation1(B, Z, MX, MZ, MR);
       //Обчислення 2 [a_6= min(Ah)]
       a = calculation2(A.arr);
       //Прийняти a з T8
       int a8;
       MPI_Recv(&a8, 1, MPI_INT, 7, 11, graph_cart, MPI_STATUS_IGNORE);
       //Обчислення 3 [a= min(a, a_6)]
       a = calculation3(a, a8);
       //Передати а в T4
       MPI_Send(&a, 1, MPI_INT, 3, 9, graph_cart);
       printf("T6 end\n");
    }
    else if (t_id == 6) { // T7
        printf("T7 start\n");
        //Прийняти MXh, Bh, Zh, MZh, MRh з T5
        int size_h = H;
        MPI_Recv(B, N, MPI_INT, 4, 7, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(Z, N, MPI_INT, 4, 7, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MX(0, 0), size_h* N, MPI_INT, 4, 7, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MZ(0, 0), size_h* N, MPI_INT, 4, 7, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MR(0, 0), N* N, MPI_INT, 4, 7, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2 [a_7= min(Ah)]
        a = calculation2(A.arr);
        //Передати а в T5
        MPI_Send(&a, 1, MPI_INT, 4, 8, graph_cart);
        printf("T7 end\n");
    }
    else if (t_id == 7) { // T8
        printf("T8 start\n");
        //Прийняти MXh, Bh, Zh, MZh, MRh з T6
        int size_h = H;
        MPI_Recv(B, N, MPI_INT, 5, 10, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(Z, N, MPI_INT, 5, 10, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MX(0,0), size_h*N, MPI_INT, 5, 10, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MZ(0,0), size_h*N, MPI_INT, 5, 10, graph_cart, MPI_STATUS_IGNORE);
        MPI_Recv(&MR(0,0), N*N, MPI_INT, 5, 10, graph_cart, MPI_STATUS_IGNORE);
        //Обчислення 1 [Ah= Bh*MZh + Zh*(MRh*MXh)]
        A = calculation1(B, Z, MX, MZ, MR);
        //Обчислення 2 [a_8= min(Ah)]
        a = calculation2(A.arr);
        //Передати а в T6
        MPI_Send(&a, 1, MPI_INT, 5, 11, graph_cart);
        printf("T8 end\n");
    }
    MPI_Type_free(&row_type);
    // Finalize: Any resources allocated for MPI can be freed
    MPI_Finalize();
}
