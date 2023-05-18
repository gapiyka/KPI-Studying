public class SynchronizationMonitor {
    private static final int in_t = 3;
    private static final int z_t = 4;
    private static final int out_t = 3;
    private int F_IN = 0;
    private int F_MIN = 0;
    private int F_MAX = 0;
    private int F_OUT = 0;

    public synchronized void signal_in() {
        F_IN += 1;
        if(F_IN == in_t)
            //for(int i = 0; i < F_IN; i++)
            notifyAll();
    }
    
    public synchronized void wait_in() {
        try{
            if(F_IN != in_t)
                wait();
        }
        catch(Exception e){}
    }

    public synchronized void signal_min() {
        F_MIN += 1;
        if(F_MIN == z_t)
            //for(int i = 0; i < z_t; i++)
                notifyAll();
    }
    
    public synchronized void wait_min() {
        try{
            if(F_MIN != z_t)
                wait();
        }
        catch(Exception e){}
    }

    public synchronized void signal_max() {
        F_MAX += 1;
        if(F_MAX == z_t)
            //for(int i = 0; i < z_t; i++)
            notifyAll();
    }
    
    public synchronized void wait_max() {
        try{
            if(F_MAX != z_t)
                wait();
        }
        catch(Exception e){}
    }

    public synchronized void signal_out() {
        F_OUT += 1;
        if(F_OUT == out_t)
            //for(int i = 0; i < F_OUT; i++)
            notifyAll();
    }
    
    public synchronized void wait_out() {
        try{
            if(F_OUT != out_t)
                wait();
        }
        catch(Exception e){}
    }
}