namespace SocialInfluence
{
    internal class Graph
    {
        private CustomLinkedList _nodes = new();
        private CustomLinkedList _edges = new();

        public void AddNode(GraphNode item)
        {
            CustomLinkedListNode newNode = new CustomLinkedListNode(item);
            _nodes.AddNode(newNode);
        }

        public void AddEdge(GraphNode originatingNode, GraphNode destinationNode, int? weight)
        {
            GraphEdge ge;
            if (weight is null)
            {
                ge = new GraphEdge(originatingNode, destinationNode, -1);
            }
            else
            {
                ge = new GraphEdge(originatingNode, destinationNode, weight);
            }
            CustomLinkedListNode newNode = new CustomLinkedListNode(ge);
            _edges.AddNode(newNode);
        }

        public void Remove(IGraphItem itemToRemove)
        {
            CustomLinkedListNode matcher = new CustomLinkedListNode(itemToRemove);
            if (itemToRemove is GraphNode)
            {
                _nodes.RemoveNode(matcher);
                return;
            } 
            if (itemToRemove is GraphEdge)
            {
                _edges.RemoveNode(matcher);
                return;
            }

            throw new Exception("Item not removed - probably invalid type!");
        }

        public List<CustomLinkedListNode> ShortestPath(GraphNode originatingNode, GraphNode destinationNode, bool isWeighted)
        {
            CustomLinkedListNode currentNode = _nodes.Head;
            List<CustomLinkedListNode> path = [];
            while (true)
            {
                path.Add(currentNode);
                if (currentNode.GraphNode == destinationNode)
                {
                    return path;
                }
                List<CustomLinkedListNode> destinationCandidates = [];
                CustomLinkedListNode iteratorNode = _edges.Head;
                while (iteratorNode is not null)
                {
                    if (iteratorNode.GraphNode.Chameleon()[0] == currentNode.GraphNode)
                    {
                        destinationCandidates.Add(iteratorNode);
                    }

                    iteratorNode++;
                    if (iteratorNode is null)
                    {
                        break;
                    }
                }

                if (isWeighted)
                {
                    destinationCandidates = destinationCandidates.OrderBy(destination => destination.GraphNode.Chameleon()[2]).ToList();
                }

                currentNode = destinationCandidates[0];

                if (currentNode is null)
                {
                    break;
                }
            }

            throw new Exception("Shortest path resulted in null");
        }

        public int UnweightedDistanceCost(GraphNode startNode)
        {
            int total = 0;
            CustomLinkedListNode currentNode = _nodes.Head;
            while (currentNode is not null)
            {
                if ((GraphNode)currentNode.GraphNode == startNode)
                {
                    continue;
                }
                total += ShortestPath(startNode, (GraphNode)currentNode++.GraphNode, false).Count;
                if (currentNode is null)
                {
                    break;
                }
            }
            return total;
        }

        public int WeightedDistanceCost(GraphNode startNode)
        {
            int total = 0;
            CustomLinkedListNode currentNode = _nodes.Head;
            while (currentNode is not null)
            {
                if ((GraphNode)currentNode.GraphNode == startNode)
                {
                    continue;
                }
                var ps = ShortestPath(startNode,(GraphNode)currentNode++.GraphNode,true);
                for (var index = 1; index < ps.Count; index++)
                {
                    var p = ps[index];
                    total += (int)p.GraphNode.Chameleon()[2];
                }

                if (currentNode is null)
                {
                    break;
                }
            }
            return total;
        }

        public int CalculateInfluenceScore(GraphNode node, bool isWeighted)
        {
            return (_nodes.Length-1)/(isWeighted ? WeightedDistanceCost(node) : UnweightedDistanceCost(node));
        }
    }

    public interface IGraphItem
    {
        public List<Object> Chameleon();
    }

    public class GraphNode : IGraphItem {
        public object? Value { get; set; }

        public List<Object> Chameleon() => [Value];

        public GraphNode()
        {
            Value = null;
        }

        public GraphNode(object value)
        {
            Value = value;
        }
    }

    public class GraphEdge : IGraphItem
    {
        public GraphNode? OriginatingNode { internal set; get; }
        public GraphNode? DestinationNode { internal set; get; }

        public int? Weight { internal set; get; } = -1;

        public List<Object> Chameleon() => [OriginatingNode, DestinationNode, Weight];

        public GraphEdge()
        {
            OriginatingNode = null;
            DestinationNode = null;
        }

        public GraphEdge(GraphNode originating, GraphNode destination, int? weight)
        {
            OriginatingNode = originating;
            DestinationNode = destination;
            Weight = weight;
        }
    }
}
