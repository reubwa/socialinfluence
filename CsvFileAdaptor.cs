using System;
using System.Collections.Generic;
using System.Text;

namespace SocialInfluence
{
    internal class CsvFileAdaptor
    {
        public void ProcessAndDump()
        {
            var res = ReadFile.ReadCsv();
            bool hasHeader = res[0,0]=="node1";
            bool isWeighted = res.GetLength(1) > 1;
            GlobalProviders.isWeighted = res.GetLength(1) > 1;
            for (int row = 0; row < res.GetLength(0); row++)
            {
                if (hasHeader && row==0)
                {
                    row++;
                }
                GraphNode firstNewNode = new GraphNode();
                GraphNode secondNewNode = new GraphNode();
                for (int col = 0; col < res.GetLength(1); col++)
                {
                    if (col == 0)
                    {
                        firstNewNode.Value = res[row,col];
                        GlobalProviders.gr.AddNode(firstNewNode);
                    } else if (col == 1)
                    {
                        secondNewNode.Value = res[row,col];
                        GlobalProviders.gr.AddNode(secondNewNode);
                        if (!isWeighted)
                        {
                            GlobalProviders.gr.AddEdge(firstNewNode,secondNewNode,null);
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
