public class T4 extends Thread {
    private int a4;
    private int d4;
    private int p4;
    private int[] Z4;
    private int[] X4;

    @Override
    public void run() {
        try {
            System.out.println("T4 is started.");
            //Введення Z
            for (int i = 0; i < Data.N; i++) {
                Data.Z[i] = 1;
            }
            //Введення MT
            for (int i = 0; i < Data.N; i++) {
                for (int j = 0; j < Data.N; j++) {
                    Data.MT[i][j] = 1;
                }
            }
            //Введення d
            Data.d = 1;
            //Сигнал задачам Т1-T3 про введення Z, MT, d
            Data.Sem24.release();
            Data.Sem25.release();
            Data.Sem26.release();
            //Чекати на введення MM, B, MX в T1
            Data.Sem3.acquire();
            //Чекати на введення p в T3
            Data.Sem18.acquire();
            //Копія d4 = d
            synchronized (Data.CS2) {
                d4 = Data.d;
            }
            //Копія Z4 = Z
            synchronized (Data.CS2) {
                Z4 = Data.Z;
            }
            //Обчислення1 Xh = sort(d * Bh + Z * MMh)
            Data.calculation1(Data.H*3, Z4, d4);
            //Сигнал задачі T3 про обчислення Xh
            Data.Sem29.release();
            //Обчислення4 a4 = (Bh * Zh)
            a4 = Data.scalarProductPart(Data.B, Data.Z, Data.H*3, Data.H*4);
            //Обчислення5 a = a + a4
            Data.a.addAndGet(a4);
            //Сигнал задачам T1-T3 про завершення обчислень a
            Data.Sem27.release();
            Data.Sem28.release();
            Data.Sem30.release();
            //Чекати на завершення обчислення Х в задачі T1
            Data.Sem6.acquire();
            //Чекати на завершення обчислення a в задачах T1-T3
            Data.Sem9.acquire();
            Data.Sem13.acquire();
            Data.Sem21.acquire();
            System.out.print("atomic(a) = " + Data.a + "\n");
            //Копія p4 = p
            synchronized (Data.CS3) {
                p4 = Data.p;
            }
            //Копія a4 = a
            synchronized (Data.CS1) {
                a4 = Data.a.get();
            }
            //Копія X4 = X
            Data.B1.lock();
            try {
                X4 = Data.X;
            } finally {
                Data.B1.unlock();
            }
            //Обчислення6 Ah = p4 * X4 * (MX * MTh) + a4 * Zh
            Data.calculation6(Data.H*3, X4, a4, p4);
            // Чекати на закінчення обчислень Ah в T1, T2, T3
            Data.Sem10.acquire();
            Data.Sem15.acquire();
            Data.Sem23.acquire();
            // Виведення A
            System.out.print("A = ");
            Data.printVector(Data.A);
            System.out.println("T4 is finished.");
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}