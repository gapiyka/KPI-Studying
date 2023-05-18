public class T2 extends Thread {
    private final int start = Data.H;
    private int z_min2;
    private int d2;
    private int z_max2;
    private int[][] MR2;

    @Override
    public void run() {
        //Введення MX
        for (int i = 0; i < Data.N; i++)
            for (int j = 0; j < Data.N; j++) 
                Data.MX[i][j] = 1;
        //Сигнал задачам T1, T3, T4 про введення MX
        Data.s_monitor.signal_in();
        //Чекати на введення MC, Z в T1 + Чекати на введення d, MR в T4
        Data.s_monitor.wait_in();
        //Обчислення 1 [z_min_2= min(Zh)]
        z_min2 = Data.VectorMin(start);
        //Обчислення 2 [z_min = min(z_min, z_ min _2)]
        Data.sr_monitor.set_z_min(z_min2);
        //Сигнал задачам T1, T3, T4 про обчислення z_min
        Data.s_monitor.signal_min();
        //Чекати на завершення обчислення z_ min в T1, T3, T4
        Data.s_monitor.wait_min();
        //Обчислення 3 [z_max_2= max (Zh)]
        z_max2 = Data.VectorMax(start);
        //Обчислення 4 [z_max = max (z_ max, z_ max _2)]
        Data.sr_monitor.set_z_max(z_max2);
        //Сигнал задачам T1,T3,T4 про обчислення z_max
        Data.s_monitor.signal_max();
        //Чекати на завершення обчислення z_ max в T1,T3,T4
        Data.s_monitor.wait_max();
        //Копіювати MR2 = MR
        MR2 = Data.sr_monitor.get_MR();
        //Копіювати d2 = d
        d2 = Data.sr_monitor.get_d();
        //Копіювати z_min_2 = z_min
        z_min2 = Data.sr_monitor.get_z_min();
        //Копіювати z_max_2 = z_max
        z_max2 = Data.sr_monitor.get_z_max();
        //Обчислення 3 [MAh= z_min2*MXh+z_max2*(MR2*MCh)*d2]
        Data.result_calculation(start, MR2, d2, z_min2, z_max2);
        //Сигнал задачі T3 про обчислення MAh
        Data.s_monitor.signal_out();
        System.out.println("T2 is finished.");
    }
}