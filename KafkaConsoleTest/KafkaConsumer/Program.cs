namespace KafkaConsumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            ConsumeMessage message = new ConsumeMessage();
            message.ReadMessage();
        }
    }
}