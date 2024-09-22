using UnityEngine;

public class SnackHead : SnakeNode
{
    public static SnackHead Instance;

    public void Awake()
    {
        if (Instance != null)
            Debug.LogError("More than one SnackHead instance");

        Instance = this;
    }
}
