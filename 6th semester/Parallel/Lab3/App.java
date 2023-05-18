/**
 * Паралельне програмування
 * Лабораторна робота: №3
 * Оцінка: A
 * Варіант: 13
 * Завдання: MA= min(Z)*MX + max(Z)*(MR*MC)*d
 * • ПВВ1 – MC, Z
 * • ПВВ2 – MX
 * • ПВВ3 – MA
 * • ПВВ4 – MR, d
 * Виконав: Гапій Денис Едуардович ІП-05
 * Дата: 14.04.2023
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