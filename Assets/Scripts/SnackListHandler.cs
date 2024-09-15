using UnityEngine;

public class SnackListHandler : MonoBehaviour
{
    public static void Insert(SnackNode first, SnackNode second)
    {
        second.next = first.next;
        if (first.next != null)
            first.next.prev = second;
        first.next = second;
        second.prev = first;
    }
    public static void Remove(SnackNode node)
    {
        if (node.prev != null)
            node.prev.next = node.next;
        if (node.next != null)
            node.next.prev = node.prev;

        Destroy(node.gameObject);
    }
}
