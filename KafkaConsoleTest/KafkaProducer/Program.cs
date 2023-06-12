namespace KafkaProducer
{
    public class Program
    {
        static void Main(string[] args)
        {
            ProduceMessage produceMessage = new ProduceMessage();
            produceMessage.CreateMessage().Wait();
        }
    }
}