public class T4 extends Thread {
    private final int start = Data.H * 3;
    private int z_min4;
    private int d4;
    private int z_max4;
    private int[][] MR4;

    @Override
    public void run() {
        System.out.println("T4 is started.");
        //Введення MR, d
        for (int i = 0; i < Data.N; i++) {
            for (int j = 0; j < Data.N; j++) {
                Data.MR[i][j] = 1;
            }
        }
        Data.d = 1;
        Data.sr_monitor.set_MR(Data.MR);
        Data.sr_monitor.set_d(Data.d);
        //Сигнал задачам T2-T4 про введення MC, Z
        Data.s_monitor.signal_in();
        //Чекати на введення MX в T2 + Чекати на введення d, MR в T4
        Data.s_monitor.wait_in();
        //Обчислення 1 [z_min_4= min(Zh)]
        z_min4 = Data.VectorMin(start);
        //Обчислення 2 [z_min = min(z_min, z_ min _4)]
        Data.sr_monitor.set_z_min(z_min4);
        //Сигнал задачам T1-T3 про обчислення z_min
        Data.s_monitor.signal_min();
        //Чекати на завершення обчислення z_ min в задачах T1-T3
        Data.s_monitor.wait_min();
        //Обчислення 3 [z_max_4= max (Zh)]
        z_max4 = Data.VectorMax(start);
        //Обчислення 4 [z_max = max (z_ max, z_ max _4)]
        Data.sr_monitor.set_z_max(z_max4);
        //Сигнал задачам T1-T3 про обчислення z_max
        Data.s_monitor.signal_max();
        //Чекати на завершення обчислення z_ max в задачах T1-T3
        Data.s_monitor.wait_max();
        //Копіювати MR4 = MR
        MR4 = Data.sr_monitor.get_MR();
        //Копіювати d4 = d
        d4 = Data.sr_monitor.get_d();
        //Копіювати z_min_4 = z_min
        z_min4 = Data.sr_monitor.get_z_min();
        //Копіювати z_max_4 = z_max
        z_max4 = Data.sr_monitor.get_z_max();
        //Обчислення 3 [MAh= z_max4*MXh+z_max4*(MR4*MCh)*d4]
        Data.result_calculation(start, MR4, d4, z_min4, z_max4);
        //Сигнал задачі T3 про обчислення MAh
        Data.s_monitor.signal_out();
        System.out.println("T4 is finished.");
    }
}