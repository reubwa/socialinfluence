

namespace InfluenceScore
{
    internal class CsvFileAdaptor
    {
        public void ProcessAndDump()
        {
            var res = ReadFile.ReadCsv();
            bool hasHeader = res[0,0]=="node1";
            bool isWeighted = res.GetLength(1) > 2;
            GlobalProviders.gr.AdjacencyList.Capacity = res.GetLength(0);
            for (int row = 0; row < res.GetLength(0); row++)
            {
                if (hasHeader && row==0)
                {
                    row++;
                }
                string firstNewNode = "", secondNewNode = "";
                for (int col = 0; col < res.GetLength(1); col++)
                {
                    if (col == 0)
                    {
                        firstNewNode = res[row,col];
                    } else if (col == 1)
                    {
                        secondNewNode = res[row,col];
                        if (!isWeighted)
                        {
                            GlobalProviders.gr.AddEdge(firstNewNode,secondNewNode);
                        }
                    } else if (isWeighted && col == 2)
                    {
                        GlobalProviders.gr.AddEdge(firstNewNode, secondNewNode, int.Parse(res[row, col]));
                    }
                }
            }
            //process complete!
        }
    }
}
