using InfluenceScore;
internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            CsvFileAdaptor adaptor = new CsvFileAdaptor();
            adaptor.ProcessAndDump();
            Console.Write("Enter value to find social influence score for: ");
            string toFind = Console.ReadLine();
            var score = GlobalProviders.gr.CalculateInfluenceScore(toFind, GlobalProviders.gr.Values.Last());
            Console.WriteLine($"Result is {score}");
        }
    }
}