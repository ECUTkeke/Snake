
using UnityEngine;

public class SnackCorner : SnackNode
{
    public Sprite leftUp;
    public Sprite leftDown;
    public Sprite rightUp;
    public Sprite rightDown;


    public override Vector2Int Direction
    {
        get => base.Direction;
        set => base._direction = value;
    }

    public void SetRelation(SnackNode prev, SnackNode next)
    {
        this.prev = prev;
        this.next = next;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (prev.Direction == Vector2Int.left && next.Direction == Vector2Int.up)
        {
            spriteRenderer.sprite = rightUp;
        }
        else if (prev.Direction == Vector2Int.left && next.Direction == Vector2Int.down)
        {
            spriteRenderer.sprite = rightDown;
        }
        else if (prev.Direction == Vector2Int.right && next.Direction == Vector2Int.up)
        {
            spriteRenderer.sprite = leftUp;
        }
        else if (prev.Direction == Vector2Int.right && next.Direction == Vector2Int.down)
        {
            spriteRenderer.sprite = leftDown;
        }
        else if (prev.Direction == Vector2Int.up && next.Direction == Vector2Int.left)
        {
            spriteRenderer.sprite = leftDown;
        }
        else if (prev.Direction == Vector2Int.up && next.Direction == Vector2Int.right)
        {
            spriteRenderer.sprite = rightDown;
        }
        else if (prev.Direction == Vector2Int.down && next.Direction == Vector2Int.left)
        {
            spriteRenderer.sprite = leftUp;
        }
        else if (prev.Direction == Vector2Int.down && next.Direction == Vector2Int.right)
        {
            spriteRenderer.sprite = rightUp;
        }
    }
}
