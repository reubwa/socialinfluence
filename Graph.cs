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
            if (!AdjacencyList.Exists(kvp => kvp.Key == start))
            {
                AdjacencyList.Add(new KeyValuePair<string, List<KeyValuePair<string, int>>>(start, []));
            }

            if (!AdjacencyList.Exists(kvp => kvp.Key == destination))
            {
                AdjacencyList.Add(new KeyValuePair<string, List<KeyValuePair<string, int>>>(destination, []));
            }

            var startNode = AdjacencyList.First(kvp => kvp.Key == start);
            startNode.Value.Add(new KeyValuePair<string, int>(destination,weight));
            var destNode = AdjacencyList.First(kvp => kvp.Key == destination);
            destNode.Value.Add(new KeyValuePair<string, int>(start,weight));
        }

        public List<string> ShortestPathUnweighted(string start, string destination)
        {
            // Stores which node led to current node
            var parents = new Dictionary<string, string>();
            // Tracks number of edges from starting node to every other node
            var distances = new Dictionary<string, int>();
            // Manages order of exploration - FIFO
            var queue = new Queue<string>();
            // marks starting node with distance of 0 and adds it to queue
            distances[start] = 0;
            queue.Enqueue(start);
            parents[start] = null;
            // Loop continues until queue is empty - layer by layer exploration
            while (queue.Count > 0)
            {
                string currentNode = queue.Dequeue();
                if (currentNode == destination) break;
                //Check neighbours
                if (!AdjacencyList.Exists(kvp => kvp.Key == currentNode)) continue;
                {
                    foreach (var neighbour in AdjacencyList[AdjacencyList.FindIndex(kvp => kvp.Key == currentNode)].Value.Select(ksi => ksi.Key).Where(neighbor => !distances.ContainsKey(neighbor)))
                    {
                        /*
                         * If neighbour hasn't been visited, mark distance as currentNode's distance + 1
                         * set currentNode as its parent
                         * Add neighbour to the queue to be explored in the next layer
                         */
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

            path.Reverse();
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
            var nodeToIndex = new Dictionary<string, int>();
            for (int i = 0; i < AdjacencyList.Count; i++)
            {
                nodeToIndex[AdjacencyList[i].Key] = i;
            }

            int vertices = AdjacencyList.Count;
            int[] distances = new int[vertices];
            //Tracks nodes that have been visited
            bool[] shortestPathTreeSet = new bool[vertices];

            for (int i = 0; i < vertices; i++)
            {
                distances[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            if (!nodeToIndex.ContainsKey(start)) return distances;

            distances[nodeToIndex[start]] = 0;

            //Loop runs for each vertex in the graph
            for (int count = 0; count < vertices - 1; count++)
            {
                //Find the node with the smallest current distance that has not yet been processed
                int u = CalculateMinimumDistance(distances, shortestPathTreeSet);
                //Something's gone wrong if either of these values are returned...
                if (u == -1 || distances[u] == int.MaxValue) break;
                //Mark node as finalised
                shortestPathTreeSet[u] = true;
                /*
                 * Look at the node's neighbours
                 * -> for each one, check if the path through the current node (to the respective neighbour)
                 * is shorter than the current known distance to the neighbour
                 * -> If a shorter path is found, update the distances array
                 */
                foreach (var (neighbourName, weight) in AdjacencyList[u].Value)
                {
                    if (!nodeToIndex.TryGetValue(neighbourName, out int v)) continue;
                    if (!shortestPathTreeSet[v] && distances[u] + weight < distances[v])
                    {
                        distances[v] = distances[u] + weight;
                    }
                }
            }
            return distances;
        }

        public double CalculateInfluenceScoreWeighted(string start)
        {
            int[] distances = ShortestPathWeighted(start);

            double totalDistance = distances.Where(d => d != int.MaxValue).Aggregate<int, double>(0, (current, d) => current + d);

            if (totalDistance == 0) return 0;
            return (SmartTotal(AdjacencyList) - 1) / totalDistance;
        }

        public double CalculateInfluenceScoreUnweighted(string start)
        {
            double totalDistance = (from node in AdjacencyList select node.Key into target where target != start select ShortestPathUnweighted(start, target) into path where path.Count > 0 select path).Aggregate<List<string>?, double>(0, (current, path) => current + (path.Count - 1));
            if (totalDistance == 0) return 0;
            return (SmartTotal(AdjacencyList) - 1) / totalDistance;
        }

        public static int SmartTotal(List<KeyValuePair<string, List<KeyValuePair<string, int>>>> kvp)
        {
            var l = kvp.DistinctBy(k => k.Key).ToList();
            int total = l.Count;
            foreach (var v in kvp.SelectMany(k => k.Value.Where(v => !l.Exists(m => m.Key == v.Key))))
            {
                total++;
                l.Add(new KeyValuePair<string, List<KeyValuePair<string,int>>>(v.Key, []));
            }
            return total;
        }
    }
}
