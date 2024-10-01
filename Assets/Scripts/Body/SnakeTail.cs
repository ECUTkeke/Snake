using UnityEngine;

public class SnakeTail : SnakeNode
{
    public static SnakeTail Instance;

    public void Awake()
    {
        Instance = this;
    }
}
