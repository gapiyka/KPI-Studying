namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            CommunicationLayer communication = new CommunicationLayer(new CFSManager(), new LogicManager(), args);
        }
    }
}
