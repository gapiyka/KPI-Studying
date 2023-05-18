public class T3 extends Thread {
    private int a3;
    private int d3;
    private int p3;
    private int[] Z3;
    private int[] X3;

    @Override
    public void run() {
        try {
            System.out.println("T3 is started.");
            //Введення p
            Data.p = 1;
            //Сигнал задачі Т1, T2, T4 про введення p
            Data.Sem16.release();
            Data.Sem17.release();
            Data.Sem18.release();
            //Чекати на введення MM, B, MX в T1
            Data.Sem2.acquire();
            //Чекати на введення Z, MT, d в T4
            Data.Sem26.acquire();
            //Копія d3 = d
            synchronized (Data.CS2) {
                d3 = Data.d;
            }
            //Копія Z3 = Z
            synchronized (Data.CS2) {
                Z3 = Data.Z;
            }
            //Обчислення1 Xh = sort(d * Bh + Z * MMh)
            Data.calculation1(Data.H*2, Z3, d3);
            //Чекати на завершення обчислень Xh в T4
            Data.Sem29.acquire();
            //Обчислення2 X2h = sort*(Xh, Xh)
            //Data.mergeSort(Data.X, Data.H*2, Data.H*4);
            Data.mergeSort(Data.X, Data.X, Data.X, Data.H, Data.H*3, Data.H*2, Data.H*4);
            //Сигнал задачі T1 про обчислення X2h
            Data.Sem19.release();
            //Обчислення4 a3 = (Bh * Zh)
            a3 = Data.scalarProductPart(Data.B, Data.Z, Data.H*2, Data.H*3);
            //Обчислення5 a = a + a3
            Data.a.addAndGet(a3);
            //Сигнал задачі T1, T2, T4 про завершення обчислень a
            Data.Sem20.release();
            Data.Sem21.release();
            Data.Sem22.release();
            //Чекати на завершення обчислення X в задачі T1
            Data.Sem5.acquire();
            //Чекати на завершення обчислень a в T1, T2, T4
            Data.Sem8.acquire();
            Data.Sem12.acquire();
            Data.Sem30.acquire();
            //Копія p3 = p
            synchronized (Data.CS3) {
                p3 = Data.p;
            }
            //Копія a3 = a
            synchronized (Data.CS1) {
                a3 = Data.a.get();
            }
            //Копія X3 = X
            Data.B1.lock();
            try {
                X3 = Data.X;
            } finally {
                Data.B1.unlock();
            }
            //Обчислення6 Ah = p3 * X3 * (MX * MTh) + a3 * Zh
            Data.calculation6(Data.H*2, X3, a3, p3);
            //Сигнал задачі T4 про завершення обчислень Ah
            Data.Sem23.release();
            System.out.println("T3 is finished.");
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}