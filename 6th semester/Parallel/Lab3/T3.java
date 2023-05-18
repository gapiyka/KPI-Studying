public class T3 extends Thread {
    private final int start = Data.H * 2;
    private int z_min3;
    private int d3;
    private int z_max3;
    private int[][] MR3;

    @Override
    public void run() {
        System.out.println("T3 is started.");
        //Чекати на введення в T1, T2, T4
        Data.s_monitor.wait_in();
        //Обчислення 1 [z_min_3= min(Zh)]
        z_min3 = Data.VectorMin(start);
        //Обчислення 2 [z_min = min(z_min, z_ min _2)]
        Data.sr_monitor.set_z_min(z_min3);
        //Сигнал задачам T1, T2, T4 про обчислення z_min
        Data.s_monitor.signal_min();
        //Чекати на завершення обчислення z_ min в T1, T2, T4
        Data.s_monitor.wait_min();
        //Обчислення 3 [z_max_3= max (Zh)]
        z_max3 = Data.VectorMax(start);
        //Обчислення 4 [z_max = max (z_ max, z_ max _2)]
        Data.sr_monitor.set_z_max(z_max3);
        //Сигнал задачам T1,T2,T4 про обчислення z_max
        Data.s_monitor.signal_max();
        //Чекати на завершення обчислення z_ max в T1,T2,T4
        Data.s_monitor.wait_max();
        //Копіювати MR3 = MR
        MR3 = Data.sr_monitor.get_MR();
        //Копіювати d3 = d
        d3 = Data.sr_monitor.get_d();
        //Копіювати z_min_3 = z_min
        z_min3 = Data.sr_monitor.get_z_min();
        //Копіювати z_max_3 = z_max
        z_max3 = Data.sr_monitor.get_z_max();
        //Обчислення 3 [MAh= z_min3*MXh+z_max3*(MR3*MCh)*d3]
        Data.result_calculation(start, MR3, d3, z_min3, z_max3);
        //Чекати на завершення обчислення MAh в T1, T2, T4
        Data.s_monitor.wait_out();
        //Виведення MA
        //Data.PrintAnswer();
        System.out.println("T3 is finished.");
    }
}