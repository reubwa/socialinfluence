using System;
using System.Collections.Generic;
using System.Text;

namespace InfluenceScore
{
    public class Graph
    {
        public List<KeyValuePair<string, List<KeyValuePair<string, int>>>> AdjacencyList { get; private set; } = [];
        public bool IsUnweighted { get; private set; } = true;
        public void AddEdge(string start, string destination, int weight = -1)
        {
            IsUnweighted = weight == -1;
            int startIndex = 0, destIndex = 0;
            for (var index = 0; index < AdjacencyList.Count; index++)
            {
                var item = AdjacencyList[index];
                if (item.Key == start)
                {
                    startIndex = index;
                    continue;
                }

                if (item.Key == destination)
                {
                    destIndex = index;
                }
            }

            if (startIndex == 0)
            {
                AdjacencyList.Add(new KeyValuePair<string, List<KeyValuePair<string, int>>>(start,[]));
                startIndex = AdjacencyList.Count;
            }

            if (destIndex == 0)
            {
                AdjacencyList.Add(new KeyValuePair<string, List<KeyValuePair<string, int>>>(destination, []));
                destIndex = AdjacencyList.Count;
            }

            AdjacencyList[startIndex].Value.Add(new KeyValuePair<string, int>(destination,weight));
            AdjacencyList[destIndex].Value.Add(new KeyValuePair<string, int>(start,weight));
        }

        public List<string> ShortestPathUnweighted(string start, string destination)
        {
            var parents = new Dictionary<string, string>();
            var distances = new Dictionary<string, int>();
            var queue = new Queue<string>();
            distances[start] = 0;
            queue.Enqueue(start);
            parents[start] = null;
            while (queue.Count > 0)
            {
                string currentNode = queue.Dequeue();
                if (currentNode == destination) break;

                if (!AdjacencyList.Exists(kvp => kvp.Key == currentNode)) continue;
                {
                    foreach (var neighbour in AdjacencyList[AdjacencyList.FindIndex(kvp => kvp.Key == currentNode)].Value.Select(ksi => ksi.Key).Where(neighbor => !distances.ContainsKey(neighbor)))
                    {
                        distances[neighbour] = distances[currentNode] + 1;
                        parents[neighbour] = currentNode;
                        queue.Enqueue(neighbour);
                    }
                }
            }

            if (!distances.ContainsKey(destination))
            {
                return []; // No path found
            }

            List<string> path = [];
            string current = destination;
            while (current != null)
            {
                path.Add(current);
                current = parents[current];
            }

            path.Reverse(); // Path is built backwards, so reverse it to get start -> end
            return path;
        }

        private static int CalculateMinimumDistance(int[] distances, bool[] shortestPathTreeSet)
        {
            int min = int.MaxValue, minIndex = -1;

            for (int v = 0; v < distances.Length; v++)
            {
                if (shortestPathTreeSet[v] || distances[v] > min) continue;
                min = distances[v];
                minIndex = v;
            }

            return minIndex;
        }

        public int[] ShortestPathWeighted(string start)
        {
            int source = AdjacencyList.FindIndex(kvp => kvp.Key==start);
            int vertices = AdjacencyList.Count;
            int[] distances = new int[vertices];
            bool[] shortestPathTreeSet = new bool[vertices];
            for (int i = 0; i < vertices; i++)
            {
                distances[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distances[source] = 0;

            for (int count = 0; count < vertices - 1; count++)
            {
                int u = CalculateMinimumDistance(distances, shortestPathTreeSet);
                shortestPathTreeSet[u] = true;

                foreach (var (key, weight) in AdjacencyList[u].Value)
                {
                    var v = AdjacencyList.FindIndex(n => n.Key == key);
                    if (!shortestPathTreeSet[v] && distances[u] != int.MaxValue && distances[u] + weight < distances[v])
                    {
                        distances[v] = distances[u] + weight;
                    }
                }
            }

            return distances;
        }

        public double CalculateInfluenceScoreWeighted(string start)
        {
            double s = ShortestPathWeighted(start).Sum();
            return (AdjacencyList.Count - 1) / s;
        }

        public double CalculateInfluenceScoreUnweighted(string start)
        {
            double cost = AdjacencyList.Sum(kvp => ShortestPathUnweighted(start, kvp.Key).Count);
            return (AdjacencyList.Count - 1) / cost;
        }
    }
}
