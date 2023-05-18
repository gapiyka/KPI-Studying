/**
 * Паралельне програмування
 * Лабораторна робота: №1
 * Оцінка: A
 * Варіант: 16
 * Завдання: A = p * sort(d * B + Z * MM) * (MX * MT) + (B * Z) * Z
 * • ПВВ1 – MM, B, MX
 * • ПВВ2 – ...
 * • ПВВ3 – p
 * • ПВВ4 – A, Z, MT, d
 * Виконав: Гапій Денис Едуардович ІП-05
 * Дата: 07.04.2023
 **/


public class App {
    public static void main(String[] args) {
        long time = System.currentTimeMillis();
        System.out.print("N = " + Data.N + "\n");
        T1 T1 = new T1();
        T2 T2 = new T2();
        T3 T3 = new T3();
        T4 T4 = new T4();
        T1.start();
        T2.start();
        T3.start();
        T4.start();
        try {
            T1.join();
            T2.join();
            T3.join();
            T4.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        System.out.print("TIME PASSED = " + (System.currentTimeMillis() - time) + "\n");
    }
}