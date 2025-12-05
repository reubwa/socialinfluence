using System;
using System.Collections.Generic;
using System.Text;

namespace InfluenceScore
{
    public class Graph
    {
        public List<List<KeyValuePair<int, int>>> AdjacencyList { get; private set; } = [];
        public List<string> Values { get; private set; } = [];
        public bool IsUnweighted { get; private set; } = true;
        public void AddEdge(string start, string destination, int weight = -1)
        {
            Values.Add(start);
            Values.Add(destination);
            Values = Values.Distinct().ToList();
            IsUnweighted = weight == -1;
            int u = Values.IndexOf(start);
            int v = Values.IndexOf(destination);
            AdjacencyList.Add([]);
            if (AdjacencyList.Count == 1)
            {
                AdjacencyList.Add([]);
            }
            AdjacencyList[u].Add(new KeyValuePair<int, int>(v, weight));
            AdjacencyList[v].Add(new KeyValuePair<int, int>(u, weight));
        }

        public List<string> ShortestPathWeighted(string start, string destination)
        {
            List<string> path = [];
            int s = Values.IndexOf(start);
            int d = Values.IndexOf(destination);
            var currentNode = AdjacencyList[s];
            foreach (var node in currentNode)
            {
                if (node.Key == d)
                {
                    path.Add(Values[node.Key]);
                    return path;
                }
            }

            while (true)
            {
                currentNode = AdjacencyList[currentNode.OrderBy(node => node.Value).ToList()[0].Key];
                foreach (var node in currentNode)
                {
                    path.Add(Values[node.Key]);
                    if (node.Key == d)
                    {
                        return path;
                    }
                }
            }
        }

        public KeyValuePair<int, List<string>> CalculateCost(string start, string destination)
        {
            var path = ShortestPathWeighted(start, destination);
            int cost = 0;
            if (IsUnweighted)
            {
                cost = path.Count;
            }
            else
            {
                for (var index = 0; index < path.Count; index++)
                {
                    var node = path[index];
                    int ix = Values.IndexOf(node);
                    var ixn = Values.IndexOf(path[index + 1]);
                    var connections = AdjacencyList[ix];
                    foreach (var connection in connections.Where(connection => connection.Key == ixn))
                    {
                        cost += connection.Value;
                        break;
                    }
                }
            }
            return new KeyValuePair<int, List<string>>(cost, path);
        }

        public int CalculateInfluenceScore(string start, string destination) => (Values.Count-1)/CalculateCost(start,destination).Key;
    }
}
