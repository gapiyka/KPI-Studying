/**
 * Паралельне програмування
 * Лабораторна робота: №3
 * Оцінка: A
 * Варіант: 27
 * Завдання: MO = MB * (MC * MM) * d + min(Z) * MC
 * • ПВВ1 – MB, MC
 * • ПВВ2 – MO
 * • ПВВ3 – ...
 * • ПВВ4 – Z, d, MM
 * Виконав: Гапій Денис Едуардович ІП-05
 * Дата: 09.04.2023
 **/


using System;
using System.Diagnostics;
using System.Threading;

namespace Lab2
{
    class Data
    {
        public static int Value = 1;
        public static int N = 4;
        public static int P = 4;
        public static int H = N / P;
        public static int d;
        public static int[] Z = new int[N];
        public static int[,] MB = new int[N, N];
        public static int[,] MC = new int[N, N];
        public static int[,] MM = new int[N, N];
        public static int[,] MO = new int[N, N];


        // Manage access to shared resources
        public static long z = Int64.MaxValue;
        public static Mutex M1 = new Mutex();
        public static object CS1 = new Object();
        public static object CS2 = new Object();
        // Засоби організації взаємодії потоків
        public static Semaphore S1 = new Semaphore(0, 3);
        public static Semaphore S2 = new Semaphore(0, 3);
        public static EventWaitHandle E1 = new
            EventWaitHandle(false, EventResetMode.ManualReset);
        public static EventWaitHandle E2 = new
            EventWaitHandle(false, EventResetMode.ManualReset);
        public static EventWaitHandle E3 = new
            EventWaitHandle(false, EventResetMode.ManualReset);
        public static EventWaitHandle E4 = new
            EventWaitHandle(false, EventResetMode.ManualReset);
        public static Barrier B1 = new Barrier(participantCount: 4);

        public static void Min(int zi) {
            if (zi < Interlocked.Read(ref z))
                Interlocked.Exchange(ref z, zi);
        }

        public static int VectorMin(int start)
        {
            int end = start + H;
            int min = Int32.MaxValue;
            for (int i = start; i < end; i++)
                if (Z[i] < min)
                    min = Z[i];
            return min;
        }

        //ABC[i,j] = sum(A[i,k] * B[k,l] * C[l,j]), for k = 1 to n and l = 1 to n
        public static void ResultCalculation(int[,] MB, int z, int d, int start)
        {
            int end = start + Data.H;
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < N; k++)
                        for (int l = 0; l < N; l++)
                            sum += MB[i, k] * MC[k, l] * MM[l, j];
                            
                    MO[i, j] = (sum * d) + (z * MC[i, j]);
                }
            }
        }

        // Вивід матриці
        public static void PrintMatrix(int[,] MO)
        {
            for (int i = 0; i < Data.N; i++)
            {
                for (int j = 0; j < Data.N; j++)
                {
                    Console.Write("{0} ", MO[i, j]);
                }
                Console.Write("\n");
            }
        }
    }

    class Program
    {
        static Stopwatch watch = new Stopwatch();
        static void Main(string[] args)
        {
            Console.WriteLine("Lab2 started");
            Console.WriteLine("N = " + Data.N);

            watch.Start();

            Thread t1 = new Thread(() => T1());
            Thread t2 = new Thread(() => T2());
            Thread t3 = new Thread(() => T3());
            Thread t4 = new Thread(() => T4());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            watch.Stop();

            Console.WriteLine("Lab2 finished for: " + watch.ElapsedMilliseconds);
        }

        static void T1()
        {
            Console.WriteLine("T1 started");
            int z1;
            int d1;
            int[,] MB1;

            // Введення MB, MC
            for (int i = 0; i < Data.N; i++)
                for (int j = 0; j < Data.N; j++)
                {
                    Data.MB[i, j] = Data.Value;
                    Data.MC[i, j] = Data.Value;
                }

            // Сигнал задачам T2-T4 про введення MB, MC
            Data.S1.Release(3);
            // Чекати на введення Z, d, MM в T4
            Data.S2.WaitOne();
            // Обчислення1 z1 = min(Zh)
            z1 = Data.VectorMin(0);
            // Обчислення2 z = min(z, z1)
            Data.Min(z1);
            // Сигнал задачам T2-T4 про обчислення z
            Data.E1.Set();
            // Чекати на завершення обчислень z в T2
            Data.E2.WaitOne();
            // Чекати на завершення обчислень z в T3
            Data.E3.WaitOne();
            // Чекати на завершення обчислень z в T4
            Data.E4.WaitOne();
            // Копіювати MB1 = MB
            Data.M1.WaitOne();
            MB1 = Data.MB;
            Data.M1.ReleaseMutex();
            // Копіювати d1 = d
            lock (Data.CS1)
            {
                d1 = Data.d;
            }
            // Копіювати z1 = z
            lock (Data.CS2)
            {
                z1 = (int)Data.z;
            }
            // Обчислення3 [MOh = MB1 * (MCh * MMh) * d1 + z1 * MCh]
            Data.ResultCalculation(MB1, d1, z1, 0);
            // Сигнал задачі T2 про обчислення MOh
            Data.B1.SignalAndWait();

            Console.WriteLine("T1 finished");
        }

        static void T2()
        {
            Console.WriteLine("T2 started");
            int z2;
            int d2;
            int[,] MB2;

            // Чекати на введення MB, MC в T1
            Data.S1.WaitOne();
            // Чекати на введення Z, d, MM в T4
            Data.S2.WaitOne();
            // Обчислення1 z2 = min(Zh)
            z2 = Data.VectorMin(Data.H);
            // Обчислення2 z = min(z, z2)
            Data.Min(z2);
            // Сигнал задачам T1, T3, T4 про обчислення z
            Data.E2.Set();
            // Чекати на завершення обчислень z в T1
            Data.E1.WaitOne();
            // Чекати на завершення обчислень z в T3
            Data.E3.WaitOne();
            // Чекати на завершення обчислень z в T4
            Data.E4.WaitOne();
            // Копіювати MB2 = MB
            Data.M1.WaitOne();
            MB2 = Data.MB;
            Data.M1.ReleaseMutex();
            // Копіювати d2 = d
            lock (Data.CS1)
            {
                d2 = Data.d;
            }
            // Копіювати z2 = z
            lock (Data.CS2)
            {
                z2 = (int)Data.z;
            }
            // Обчислення3 [MOh = MB2 * (MCh * MMh) * d2 + z2 * MCh]
            Data.ResultCalculation(MB2, d2, z2, Data.H);
            // Чекати завершення обчислення MOh в задачах T1, T3, T4
            Data.B1.SignalAndWait();
            // Виведення результату MO
            Data.PrintMatrix(Data.MO);

            Console.WriteLine("T2 finished");
        }

        static void T3()
        {
            Console.WriteLine("T3 started");
            int z3;
            int d3;
            int[,] MB3;

            // Чекати на введення MB, MC в T1
            Data.S1.WaitOne();
            // Чекати на введення Z, d, MM в T4
            Data.S2.WaitOne();
            // Обчислення1 z3 = min(Zh)
            z3 = Data.VectorMin(Data.H * 2);
            // Обчислення2 z = min(z, z3)
            Data.Min(z3);
            // Сигнал задачам T1, T2, T4 про обчислення z
            Data.E3.Set();
            // Чекати на завершення обчислень z в T1
            Data.E1.WaitOne();
            // Чекати на завершення обчислень z в T2
            Data.E2.WaitOne();
            // Чекати на завершення обчислень z в T4
            Data.E4.WaitOne();
            // Копіювати MB2 = MB
            Data.M1.WaitOne();
            MB3 = Data.MB;
            Data.M1.ReleaseMutex();
            // Копіювати d2 = d
            lock (Data.CS1)
            {
                d3 = Data.d;
            }
            // Копіювати z2 = z
            lock (Data.CS2)
            {
                z3 = (int)Data.z;
            }
            // Обчислення3 [MOh = MB3 * (MCh * MMh) * d3 + z3 * MCh]
            Data.ResultCalculation(MB3, d3, z3, Data.H * 2);
            // Сигнал задачі T2 про обчислення MOh
            Data.B1.SignalAndWait();

            Console.WriteLine("T3 finished");
        }

        static void T4()
        {
            Console.WriteLine("T4 started");
            int z4;
            int d4;
            int[,] MB4;

            // Введення Z, d, MM
            Data.d = Data.Value;
            for (int i = 0; i < Data.N; i++)
            {
                Data.Z[i] = Data.Value;
                for (int j = 0; j < Data.N; j++)
                {
                    Data.MM[i, j] = Data.Value;
                }
            }
            // Сигнал задачам T1-T3 про введення Z, d, MM
            Data.S2.Release(3);
            // Чекати на введення MB, MC в T1
            Data.S1.WaitOne();
            // Обчислення1 z4 = min(Zh)
            z4 = Data.VectorMin(Data.H * 3);
            // Обчислення2 z = min(z, z4)
            Data.Min(z4);
            // Сигнал задачам T1-T3 про обчислення z
            Data.E4.Set();
            // Чекати на завершення обчислень z в T1
            Data.E1.WaitOne();
            // Чекати на завершення обчислень z в T2
            Data.E2.WaitOne();
            // Чекати на завершення обчислень z в T3
            Data.E3.WaitOne();
            // Копіювати MB4 = MB
            Data.M1.WaitOne();
            MB4 = Data.MB;
            Data.M1.ReleaseMutex();
            // Копіювати d4 = d
            lock (Data.CS1)
            {
                d4 = Data.d;
            }
            // Копіювати z4 = z
            lock (Data.CS2)
            {
                z4 = (int)Data.z;
            }
            // Обчислення3 [MOh = MB4 * (MCh * MMh) * d4 + z4 * MCh]
            Data.ResultCalculation(MB4, d4, z4, Data.H * 3);
            // Сигнал задачі T2 про обчислення MOh
            Data.B1.SignalAndWait();

            Console.WriteLine("T4 finished");
        }
    }
}
