public class T2 extends Thread {
    private int a2;
    private int d2;
    private int p2;
    private int[] Z2;
    private int[] X2;

    @Override
    public void run() {
        try {
            System.out.println("T2 is started.");
            //Чекати на введення MM, B, MX в T1
            Data.Sem1.acquire();
            //Чекати на введення p в T3
            Data.Sem17.acquire();
            //Чекати на введення Z, MT, d в T4
            Data.Sem25.acquire();
            //Копія d2 = d
            synchronized (Data.CS2) {
                d2 = Data.d;
            }
            //Копія Z2 = Z
            synchronized (Data.CS2) {
                Z2 = Data.Z;
            }
            //Обчислення1 Xh = sort(d * Bh + Z * MMh)
            Data.calculation1(Data.H, Z2, d2);
            //Сигнал задачі T1 про обчислення Xh
            Data.Sem11.release();
            //Обчислення4 a2 = (Bh * Zh)
            a2 = Data.scalarProductPart(Data.B, Data.Z, Data.H, Data.H*2);
            //Обчислення5 a = a + a2
            Data.a.addAndGet(a2);
            //Сигнал задачі T1, T3, T4 про завершення обчислень a
            Data.Sem12.release();
            Data.Sem13.release();
            Data.Sem14.release();
            //Чекати на завершення обчислення Х в задачі T1
            Data.Sem4.acquire();
            //Чекати на завершення обчислення a в задачах T1, T3, T4
            Data.Sem7.acquire();
            Data.Sem20.acquire();
            Data.Sem28.acquire();
            //Копія p2 = p
            synchronized (Data.CS3) {
                p2 = Data.p;
            }
            //Копія a2 = a
            synchronized (Data.CS1) {
                a2 = Data.a.get();
            }
            //Копія X2 = X
            Data.B1.lock();
            try {
                X2 = Data.X;
            } finally {
                Data.B1.unlock();
            }
            //Обчислення6 Ah = p2 * X2 * (MX * MTh) + a2 * Zh
            Data.calculation6(Data.H, X2, a2, p2);
            //Сигнал задачі T4 про завершення обчислень Ah
            Data.Sem15.release();
            System.out.println("T2 is finished.");
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}