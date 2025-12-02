// See https://aka.ms/new-console-template for more information

namespace SocialInfluence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                CsvFileAdaptor adaptor = new CsvFileAdaptor();
                adaptor.ProcessAndDump();
                Console.Write("Enter value to find social influence score for: ");
                GraphNode infNode = new GraphNode(Console.ReadLine());
                var score = GlobalProviders.gr.CalculateInfluenceScore(infNode,GlobalProviders.isWeighted);
                Console.WriteLine($"Result is {score}");
            }
        }
    }
}