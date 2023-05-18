public class T1 extends Thread {
    private final int start = 0;
    private int z_min1;
    private int d1;
    private int z_max1;
    private int[][] MR1;

    @Override
    public void run() {
        System.out.println("T1 is started.");
        //Введення MC, Z
        for (int i = 0; i < Data.N; i++) {
            for (int j = 0; j < Data.N; j++) {
                Data.MC[i][j] = 1;
            }
            Data.Z[i] = 1;
        }
        //Сигнал задачам T2-T4 про введення MC, Z
        Data.s_monitor.signal_in();
        //Чекати на введення MX в T2 + Чекати на введення d, MR в T4
        Data.s_monitor.wait_in();
        //Обчислення 1 [z_min_1= min(Zh)]
        z_min1 = Data.VectorMin(start);
        //Обчислення 2 [z_min = min(z_min, z_ min _1)]
        Data.sr_monitor.set_z_min(z_min1);
        //Сигнал задачам T2-T4 про обчислення z_min
        Data.s_monitor.signal_min();
        //Чекати на завершення обчислення z_ min в задачах T2-T4
        Data.s_monitor.wait_min();
        //Обчислення 3 [z_max_1= max (Zh)]
        z_max1 = Data.VectorMax(start);
        //Обчислення 4 [z_max = max (z_ max, z_ max _1)]
        Data.sr_monitor.set_z_max(z_max1);
        //Сигнал задачам T2-T4 про обчислення z_max
        Data.s_monitor.signal_max();
        //Чекати на завершення обчислення z_ max в задачах T2-T4
        Data.s_monitor.wait_max();
        //Копіювати MR1 = MR
        MR1 = Data.sr_monitor.get_MR();
        //Копіювати d1 = d
        d1 = Data.sr_monitor.get_d();
        //Копіювати z_min_1 = z_min
        z_min1 = Data.sr_monitor.get_z_min();
        //Копіювати z_max_1 = z_max
        z_max1 = Data.sr_monitor.get_z_max();
        //Обчислення 3 [MAh= z_min1*MXh+z_max1*(MR1*MCh)*d1]
        Data.result_calculation(start, MR1, d1, z_min1, z_max1);
        //Сигнал задачі T3 про обчислення MAh
        Data.s_monitor.signal_out();
        System.out.println("T1 is finished.");
    }
}
