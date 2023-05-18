public class Data {
    public static final int N = 4;
    public static final int P = 4;
    public static final int H = N / P;
    public static int[][] MA = new int[N][N];
    public static int[][] MX = new int[N][N];
    public static int[][] MR = new int[N][N];
    public static int[][] MC = new int[N][N];
    public static int[] Z = new int[N];
    public static int d = 0;
    // Спільні ресурси
    public static final SharedResourcesMonitor sr_monitor = new SharedResourcesMonitor();
    // Елементи взаємодії
    public static final SynchronizationMonitor s_monitor = new SynchronizationMonitor();
    
    // Обчислення5 MAh= z_ min*MXh + z_ max*(MR*MCh)*d
    public static void result_calculation(int start, int[][] MR, int d, int z_min, int z_max) {
        int end = start + H;
        for (int i = start; i < end; i++) {
            for (int j = 0; j < N; j++) {
                int part = 0;
                for (int k = 0; k < N; k++) {
                    part += MR[i][k] * MC[k][j];
                }
                MA[i][j] = MX[i][j] * z_min + part * z_max * d;
            }
        }
    }
    //min of vector
    public static int VectorMin(int start){
        int min = Integer.MAX_VALUE;
        int end = start + H;
        for (int i = start; i < end; i++) 
            if(Z[i] < min)
                min = Z[i];
        return min;
    }
    //max of vector
    public static int VectorMax(int start){
        int max = Integer.MIN_VALUE;
        int end = start + H;
        for (int i = start; i < end; i++) 
            if(Z[i] > max)
                max = Z[i];
        return max;
    }
    //print answer
    public static void PrintAnswer(){
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
               System.out.print(MA[i][j] + " ");
            }
            System.out.println();
        }
    }
}