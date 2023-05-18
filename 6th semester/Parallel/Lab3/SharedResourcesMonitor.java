public class SharedResourcesMonitor {
    private int z_min;
    private int z_max;
    private int d;
    private int[][] MR;

    public SharedResourcesMonitor(){
        z_min = Integer.MAX_VALUE;
        z_max = Integer.MIN_VALUE;
    }

    public synchronized void set_z_min(int m) {
        if(m < z_min) 
            z_min = m;
    }

    public synchronized void set_z_max(int m) {
        if(m > z_max) 
            z_max = m;
    }
    
    public synchronized void set_d(int d) {
        this.d = d;
    }
    
    public synchronized void set_MR(int[][] MR) {
        this.MR = MR;
    }
    
    public synchronized int[][] get_MR() {
        return this.MR;
    }
    
    public synchronized int get_d() {
        return this.d;
    }
    
    public synchronized int get_z_min() {
        return this.z_min;
    }
    
    public synchronized int get_z_max() {
        return this.z_max;
    }
}
