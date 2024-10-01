using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNode : BaseNode
{
    public SnakeNode prev;
    public SnakeNode next;
    protected Vector2Int _direction;
    public virtual Vector2Int Direction
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
}
