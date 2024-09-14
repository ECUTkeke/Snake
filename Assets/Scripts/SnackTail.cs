using UnityEngine;

public class SnackTail : SnackNode
{
    public static SnackTail Instance;

    public void Awake()
    {
        if (Instance != null)
            Debug.LogError("More than one SnackTail instance");

        Instance = this;
    }
}
