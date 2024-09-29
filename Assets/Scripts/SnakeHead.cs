using UnityEngine;

public class SnakeHead : SnakeNode
{
    public static SnakeHead Instance;

    public void Awake()
    {
        if (Instance != null)
            Debug.LogError("More than one SnackHead instance");

        Instance = this;
    }
}
