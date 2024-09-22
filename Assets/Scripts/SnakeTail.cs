using UnityEngine;

public class SnakeTail : SnakeNode
{
    public static SnakeTail Instance;

    public void Awake()
    {
        if (Instance != null)
            Debug.LogError("More than one SnakeTail instance");

        Instance = this;
    }
}
