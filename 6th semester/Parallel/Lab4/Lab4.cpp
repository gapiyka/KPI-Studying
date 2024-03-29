/**
 * Паралельне програмування
 * Лабораторна робота: №4
 * Оцінка: A
 * Варіант: 10
 * Завдання: MM = MB*(MC*MM)*d + min(Z)*MC
 * • ПВВ1 – MB
 * • ПВВ2 – MM
 * • ПВВ3 – MC
 * • ПВВ4 – d, MM, Z
 * Виконав: Гапій Денис Едуардович ІП-05
 * Дата: 24.04.2023
 **/

using namespace std;

#pragma region Includes
#include <iostream>
#include <string>
#include <omp.h>
#include <chrono>
#pragma endregion

#pragma region Constants
const int N = 4;
const int P = 4;
const int H = N / P;
#pragma endregion

#pragma region Functions
int get_scalar() { return 1; }

int* get_vector(int size) {
    int* v = new int[size];
    for (int i = 0; i < size; ++i) 
        v[i] = get_scalar();
    return v;
}

int** get_matrix(int rows, int cols) {
    int** m = new int* [rows];
    for (int r = 0; r < rows; ++r) 
        m[r] = get_vector(cols);
    return m;
}

int** copy_matrix(int** matrix, int rows, int cols) {
    int** cp = new int* [rows];
    for (int r = 0; r < rows; ++r) {
        cp[r] = new int[cols];
        for (int c = 0; c < cols; ++c) 
            cp[r][c] = matrix[r][c];
    }
    return cp;
}

string str(int** matrix, int rows, int cols) {
    string s = "[\n";
    for (int r = 0; r < rows; ++r) {
        s.append("  [ ");
        for (int c = 0; c < cols; ++c) {
            s.append(" ").append(to_string(matrix[r][c])).append(" ");
        }
        s.append(" ]\n");
    }
    s.append("]");
    return s;
}
#pragma endregion


int main()
{
    cout << "Lab started" << endl;
    auto start = chrono::high_resolution_clock::now();
    int t;
    int d, a = INT_MAX;
    int* Z;
    int** MR, ** MC, ** MB, ** MM;

    int cp_a, cp_d;
    int** cp_MB;

    int sum, end, startH;
    MR = new int* [N];
    for (int r = 0; r < N; ++r)
        MR[r] = new int[N];

    #pragma omp parallel num_threads(P) default(shared) private(t, cp_d, cp_a, cp_MB, sum, end, startH) //shared(a, d, MB, MR)
    {
        t = omp_get_thread_num();

        #pragma omp critical(debug)
        cout << "T" << t + 1 << " started" << endl;

        switch (t) {
            case 0: // Input MB on T1
                MB = get_matrix(N, N);
                startH = 0;
                break;
            case 1:
                startH = H;
                break;
            case 2: // Input MC on T3
                MC = get_matrix(N, N);
                startH = H*2;
                break;
            case 3: // Input d, MM, Z on T4
                MM = get_matrix(N, N);
                Z = get_vector(N);
                d = get_scalar();
                startH = H*3;
                break;
            default:
                break;
        }
        end = startH + H;

        // INPUT BARRIER
        #pragma omp barrier

        #pragma omp for reduction(min: a) // CS1
        for (int i = 0; i < N; ++i) 
            if (Z[i] < a) a = Z[i];

        // CALCULATION BARRIER
        #pragma omp barrier

        #pragma omp critical(CS2)
        {
            cp_a = a;
        }

        #pragma omp critical(CS3)
        {
            cp_d = d;
        }

        #pragma omp critical(CS4)
        {
            cp_MB = copy_matrix(MB, N, N);
        }

        for (int i = startH; i < end; ++i) {
            #pragma omp for
            for (int j = 0; j < N; ++j) {
                sum = 0;
                for (int k = 0; k < N; ++k) 
                    sum += cp_MB[i][j] * MC[k][j] * MM[i][k];
                MR[i][j] = sum * cp_d + cp_a * MC[i][j];
            }
        }

        // PRE OUTPUT BARRIER
        #pragma omp barrier

        if (t == 1) {
            #pragma omp critical(debug)
            cout << str(MR, N, N) << endl;
        }

        for (int i = 0; i < N; ++i) {
            delete[] cp_MB[i];
        }
        delete[] cp_MB;

        #pragma omp critical(debug)
        cout << "T" << t + 1 << " finished" << endl;
    }  // pragma omp parallel

    for (int i = 0; i < N; ++i) {
        delete[] MC[i];
        delete[] MB[i];
        delete[] MM[i];
        delete[] MR[i];
    }
    delete[] Z;
    delete[] MC;
    delete[] MB;
    delete[] MM;
    delete[] MR;
    
    auto stop = chrono::high_resolution_clock::now();
    auto duration = chrono::duration_cast<chrono::milliseconds>(stop - start);
    cout << "Lab finished in " << duration.count() << " ms" << endl;
}
