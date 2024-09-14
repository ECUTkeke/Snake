using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackNode : BaseNode
{
    public SnackNode prev;
    public SnackNode next;
    private Vector2Int _direction;
    public Vector2Int Direction
    {
        get => _direction;
        set
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            _direction = value;
            if (value == Vector2Int.left){
                spriteRenderer.sprite = left;
            }
            else if (value == Vector2Int.right){
                spriteRenderer.sprite = right;
            }
            else if (value == Vector2Int.up){
                spriteRenderer.sprite = up;
            }
            else if (value == Vector2Int.down){
                spriteRenderer.sprite = down;
            }

        }
    }
#if DEBUG
    public Vector2Int gridPos;
#endif
}
