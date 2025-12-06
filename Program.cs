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
            int score = GlobalProviders.gr.IsUnweighted ? GlobalProviders.gr.CalculateInfluenceScoreUnweighted(toFind) : GlobalProviders.gr.CalculateInfluenceScoreWeighted(toFind);
            Console.WriteLine($"Result is {score}");
        }
    }
}