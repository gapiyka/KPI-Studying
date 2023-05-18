public class T1 extends Thread {
    private int a1;
    private int d1;
    private int p1;
    private int[] Z1;
    private int[] X1;

    @Override
    public void run() {
        try {
            System.out.println("T1 is started.");
            //Введення MM
            for (int i = 0; i < Data.N; i++) {
                for (int j = 0; j < Data.N; j++) {
                    Data.MM[i][j] = 1;
                }
            }
            //Введення B
            for (int i = 0; i < Data.N; i++) {
                Data.B[i] = 1;
            }
            //Введення MX
            for (int i = 0; i < Data.N; i++) {
                for (int j = 0; j < Data.N; j++) {
                    Data.MX[i][j] = 1;
                }
            }
            
            // Сигнал задачі Т2, T3, T4 про введення MM, B, MX
            Data.Sem1.release();
            Data.Sem2.release();
            Data.Sem3.release();
            //Чекати на введення p в T3
            Data.Sem16.acquire();
            //Чекати на введення Z, MT, d в T4
            Data.Sem24.acquire();
            //Копія d1 = d
            synchronized (Data.CS2) {
                d1 = Data.d;
            }
            //Копія Z1 = Z
            synchronized (Data.CS2) {
                Z1 = Data.Z;
            }
            //Обчислення1 Xh = sort(d * Bh + Z * MMh)
            Data.calculation1(0, Z1, d1);
            //Чекати на завершення обчислень Xh в T2
            Data.Sem11.acquire();
            //Обчислення2 X2h = sort*(Xh, Xh)
            //Data.mergeSort(Data.X, 0, Data.H*2);
            Data.mergeSort(Data.X, Data.X, Data.X, 0, Data.H*2, Data.H, Data.H*3);
            //Чекати на завершення обчислень X2h в T3
            Data.Sem19.acquire();
            //Обчислення3 X = sort*(X2h, X2h)
            //Data.mergeSort(Data.X, 0, Data.H*4);
            Data.mergeSort(Data.X, Data.X, Data.X, 0, Data.H*2, Data.H*2, Data.H*4);
            System.out.print("X = ");
            Data.printVector(Data.X);
            //Сигнал задачі T2, T3, T4 про завершення обчислень X
            Data.Sem4.release();
            Data.Sem5.release();
            Data.Sem6.release();
            //Обчислення4 a1 = (Bh * Zh)
            a1 = Data.scalarProductPart(Data.B, Data.Z, 0, Data.H);
            //Обчислення5 a = a + a1
            Data.a.addAndGet(a1);
            //Сигнал задачі T2, T3, T4 про завершення обчислень a
            Data.Sem7.release();
            Data.Sem8.release();
            Data.Sem9.release();
            //Чекати на завершення обчислень a в T2
            Data.Sem14.acquire();
            //Чекати на завершення обчислень a в T3
            Data.Sem22.acquire();
            //Чекати на завершення обчислень a в T4
            Data.Sem27.acquire();
            //Копія p1 = p
            synchronized (Data.CS3) {
                p1 = Data.p;
            }
            //Копія a1 = a
            synchronized (Data.CS1) {
                a1 = Data.a.get();
            }
            //Копія X1 = X
            Data.B1.lock();
            try {
                X1 = Data.X;
            } finally {
                Data.B1.unlock();
            }
            //Обчислення6 Ah = p1 * X1 * (MX * MTh) + a1 * Zh
            Data.calculation6(0, X1, a1, p1);
            //Сигнал задачі T4 про завершення обчислень Ah
            Data.Sem10.release();
            System.out.println("T1 is finished.");
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
