using UnityEngine;

public class BaseNode: MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    public Vector2Int gridPos;
    public bool IsNewCreated = false;
}
