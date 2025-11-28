

namespace SocialInfluence;

public class CustomLinkedList
{
    public CustomLinkedListNode? Head;
    public CustomLinkedListNode? Tail;

    public int Length => ReturnLength();

    public bool IsEmpty() => Head is null;

    private static int GetLastIndex(List<CustomLinkedListNode> inp) => inp.IndexOf(inp.Last()) + 1;

    public void Shuffle()
    {
        List<CustomLinkedListNode> shuffleArray = [];
        if (IsEmpty())
        {
            Console.WriteLine("List is empty!");
            return;
        }
        CustomLinkedListNode? temp = Head;
        while (temp is not null)
        {
            shuffleArray.Add(temp++);
        }
        Random newShuffle = new Random();
        Head = null;
        int len = shuffleArray.Count;
        while (len-->0)
        {
            int ind = newShuffle.Next(0, GetLastIndex(shuffleArray));
            shuffleArray[ind].Next = shuffleArray[ind].Prev = null;
            AddNode(shuffleArray[ind]);
            shuffleArray.RemoveAt(ind);
        }
    }

    public void AddNode(CustomLinkedListNode node)
    {
        if (!IsEmpty())
        {
            Tail.Next = node;
            node.Prev = Tail;
            Tail = node;
        }
        else
        {
            Head = node;
            Tail = node;
        }
    }

    public void InsertAfter(CustomLinkedListNode insertNode, CustomLinkedListNode preceedingNode)
    {
        CustomLinkedListNode? temp = Head;
        do { temp++; } while (temp != null && temp != preceedingNode);
        if(temp is null)
        {
            Console.WriteLine("Node not found!");
            return;
        }

        insertNode.Next = temp.Next;
        insertNode.Prev = temp;
        if (temp.Next is not null)
        {
            temp.Next.Prev = insertNode;
        }
        else
        {
            Tail = insertNode;
        }

        temp.Next = insertNode;
    }

    public void RemoveNode(CustomLinkedListNode node)
    {
        if (IsEmpty())
        {
            Console.WriteLine("List is empty!");
        }
        else
        {
            CustomLinkedListNode? temp = Head;
            while (temp != node) { temp++; }

            if (temp is null)
            {
                Console.WriteLine("Element not found!");
            }
            else
            {
                if (temp.Next is null)
                {
                    temp.Prev.Next = null;
                }

                if (temp.Prev is not null && temp.Next is not null)
                {
                    temp.Prev.Next = temp.Next;
                    temp.Next.Prev = temp.Prev;
                }

                if (temp.Prev is null)
                {
                    Head = temp.Next;
                }
            }
        }
    }

    public int ReturnPosition(CustomLinkedListNode node)
    {
        CustomLinkedListNode? current = Head;
        int it = 0;
        while (true)
        {
            it++;
            if (current == node)
            {
                break;
            }
            if (current?.Next is not null)
            {
                current++;
            }
            else
            {
                break;
            }
        }

        return it;
    }

    private int ReturnLength()
    {
        CustomLinkedListNode? current = Head;
        int it = 0;
        while (current?.Next is not null)
        {
            current++;
            it++;
        }

        return it;
    }

}
public class CustomLinkedListNode
{
    public CustomLinkedListNode(IGraphItem gi)
    {
        GraphNode = gi;
    }
    public IGraphItem? GraphNode { get; set; } = null;
    public CustomLinkedListNode Next { get; set; } = null!;
    public CustomLinkedListNode Prev { get; set; } = null!;
    public static CustomLinkedListNode? operator ++(CustomLinkedListNode? node) => node?.Next;
}