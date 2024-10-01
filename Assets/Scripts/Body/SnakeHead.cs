using UnityEngine;

public class SnakeHead : SnakeNode
{
    public static SnakeHead Instance;

    public void Awake()
    {
        Instance = this;
    }
}
