import java.util.Arrays;
import java.util.concurrent.Semaphore;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.locks.ReentrantLock;

public class Data {
    public static final int N = 1600;
    public static final int P = 4;
    public static final int H = N / P;
    public static int[][] MM = new int[N][N];
    public static int[][] MX = new int[N][N];
    public static int[][] MT = new int[N][N];
    public static int[] A = new int[N];
    public static int[] B = new int[N];
    public static int[] Z = new int[N];
    public static int[] X = new int[N];
    public static int p = 0;
    public static int d = 0;
    public static AtomicInteger a = new AtomicInteger();
    public static final Object CS1 = new Object();
    public static final Object CS2 = new Object();
    public static final Object CS3 = new Object();
    public static final Object CS4 = new Object();
    public static final ReentrantLock B1 = new ReentrantLock();
    // Елементи взаємодії
    public static Semaphore Sem1 = new Semaphore(1, true);
    public static Semaphore Sem2 = new Semaphore(0, true);
    public static Semaphore Sem3 = new Semaphore(0, true);
    public static Semaphore Sem4 = new Semaphore(0, true);
    public static Semaphore Sem5 = new Semaphore(0, true);
    public static Semaphore Sem6 = new Semaphore(0, true);
    public static Semaphore Sem7 = new Semaphore(0, true);
    public static Semaphore Sem8 = new Semaphore(0, true);
    public static Semaphore Sem9 = new Semaphore(0, true);
    public static Semaphore Sem10 = new Semaphore(0, true);
    public static Semaphore Sem11 = new Semaphore(0, true);
    public static Semaphore Sem12 = new Semaphore(0, true);
    public static Semaphore Sem13 = new Semaphore(0, true);
    public static Semaphore Sem14 = new Semaphore(0, true);
    public static Semaphore Sem15 = new Semaphore(0, true);
    public static Semaphore Sem16 = new Semaphore(0, true);
    public static Semaphore Sem17 = new Semaphore(0, true);
    public static Semaphore Sem18 = new Semaphore(0, true);
    public static Semaphore Sem19 = new Semaphore(0, true);
    public static Semaphore Sem20 = new Semaphore(0, true);
    public static Semaphore Sem21 = new Semaphore(0, true);
    public static Semaphore Sem22 = new Semaphore(0, true);
    public static Semaphore Sem23 = new Semaphore(0, true);
    public static Semaphore Sem24 = new Semaphore(0, true);
    public static Semaphore Sem25 = new Semaphore(0, true);
    public static Semaphore Sem26 = new Semaphore(0, true);
    public static Semaphore Sem27 = new Semaphore(0, true);
    public static Semaphore Sem28 = new Semaphore(0, true);
    public static Semaphore Sem29 = new Semaphore(0, true);
    public static Semaphore Sem30 = new Semaphore(0, true);
    
    // Обчислення6 Ah = p1 * X1 * (MX * MTh) + a1 * Zh
    public static void calculation6(int start, int[] X, int a, int p) {
        int end = start + H;
        for (int i = start; i < end; i++) {
            int part = 0;
            for (int j = 0; j < N; j++) {
                for (int k = 0; k < N; k++) {
                    part += p * X[k] * (MX[i][j] * MT[i][j]);
                }
            }
            System.out.print("Ah = " + part + " + " + a * Z[i] + "\n");
            A[i] = a * Z[i] + part;
        }
    }
    // Обчислення1 Xh = sort(d * Bh + Z * MMh)
    public static void calculation1(int start, int[] Z, int d) {
        int end = start + H;
        for (int i = start; i < end; i++) {
            int part = 0;
            for (int j = 0; j < N; j++) {
                part += MM[i][j] * Z[j];
            }
            X[i] = d * B[i] + part;
        }
        Arrays.sort(X, start, end);
    }
    // S = sort*(X, Y)
    public static void mergeSort(int[] R, int[] X, int[] Y, int startX, 
                                int startY, int endX, int endY) {
        int[] buffer = new int[endX - startX + endY - startY];
        int i = startX, j = startY, k = 0;
        while (i < endX && j < endY) {
            if (X[i] <= Y[j]) {
                buffer[k] = X[i++];
            }
            else {
                buffer[k] = Y[j++];
            }
            k++;
        }
        while (i < endX) {
            buffer[k++] = X[i++];
        }
        while (j < endY) {
            buffer[k++] = Y[j++];
        }
        System.arraycopy(buffer, 0, R, startX*2, k);
    }
    //prev atomic part
    static int scalarProductPart(int[] X, int[] Y, int start, int end) {
        int result = 0;
        for (int i = start; i < end; i++) {
            result += X[i] * Y[i];
        }
        return result;
    }
    //output vector
    public static void printVector(int[] X) {
        System.out.println(Arrays.toString(X));
    }
}