using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class SnakeModel
{
    private GameSetting setting;
    private BaseNode[,] gridMap;
    public BaseNode[,] GridMap => gridMap;

    public SnakeModel(GameSetting setting){
        gridMap = new BaseNode[setting.rows, setting.cols];
        this.setting = setting;
    }

    readonly public List<Vector2Int> directions = new List<Vector2Int> {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down
    };

    public bool CheckOutOfBounds(int row, int col){
        return row < 0 || row >= setting.rows || col < 0 || col >= setting.cols;
    }

    public void InitializeSnake(){
        var headObj = SnakeHead.Instance.gameObject;
        var tailObj = SnakeTail.Instance.gameObject;

        int headRow = Random.Range(0, setting.rows);
        int headCol = Random.Range(0, setting.cols);

        int tailRow;
        int tailCol;
        Vector2Int direction;
        do{
            direction = directions[Random.Range(0, directions.Count)];
            tailRow = headRow - direction.y;
            tailCol = headCol - direction.x;
        }while(CheckOutOfBounds(tailRow, tailCol));    

        var head = headObj.GetComponent<SnakeHead>();
        gridMap[headRow, headCol] = head;
        head.Direction = direction;
        head.gridPos = new Vector2Int(headRow, headCol);

        var tail = tailObj.GetComponent<SnakeTail>();
        gridMap[tailRow, tailCol] = tail;
        tail.Direction = direction;
        tail.gridPos = new Vector2Int(tailRow, tailCol);

        Insert(head, tail);

        RespawnApple();
    }

    public void UpdateStick() {
        MoveSnack();
    }

    private void MoveSnack(){
        var eatedApple = false;

        // Compute new position
        var tail = SnakeTail.Instance;
        var newTailPos = tail.prev.gridPos;
        var newTailDir = tail.prev.Direction;

        var head = SnakeHead.Instance;
        var oldPos = head.gridPos;
        var newPos = head.gridPos + new Vector2Int(head.Direction.y, head.Direction.x);
        if (CheckOutOfBounds(newPos.x, newPos.y))
        {
            if (newPos.x < 0)
                newPos.x += setting.rows;
            if (newPos.y < 0)
                newPos.y += setting.cols;

            newPos.x %= setting.rows;
            newPos.y %= setting.cols;
        }

        // Do collision computation
        if (gridMap[newPos.x, newPos.y] != null)
        {
            if (gridMap[newPos.x, newPos.y] is SnakeNode)
            {
                head.Direction = head.next.Direction;
                SnakeController.Instance.Broadcast_OnGameOver();
                return;
            }
            else if (gridMap[newPos.x, newPos.y].gameObject.tag == "Apple")
            {
                SnakeController.Instance.DestroyNode(gridMap[newPos.x, newPos.y]);
                newTailDir = tail.Direction;
                newTailPos = tail.gridPos;
                eatedApple = true;
            }
        }

        // Respawn node ahead of head
        GameObject gameObj;
        if (head.Direction != head.next.Direction)
        {
            gameObj = SnakePrefabFactory.Instance.CreateSnakeNode(typeof(SnakeCorner)); 
            var corner = gameObj.GetComponent<SnakeCorner>();
            corner.SetRelation(head, head.next);
        }
        else
        {
            gameObj = SnakePrefabFactory.Instance.CreateSnakeNode(typeof(SnakeNode));
        }
        var body = gameObj.GetComponent<SnakeNode>();
        SnakeController.Instance.MoveNode(body);

        // Set head, body, tail pos and direction
        body.Direction = head.Direction;
        body.gridPos = oldPos;
        head.gridPos = newPos;

        Insert(head, body);

        // update value in grid map
        gridMap[newPos.x, newPos.y] = head;
        gridMap[oldPos.x, oldPos.y] = body;

        // Calful about the order of the following code
        gridMap[tail.gridPos.x, tail.gridPos.y] = null;
        gridMap[newTailPos.x, newTailPos.y] = tail;
        tail.Direction = newTailDir;
        tail.gridPos = newTailPos;

        // Do communication with controller
        if (eatedApple){
            RespawnApple();
            SnakeController.Instance.Broadcast_OnEatApple();
        }
        else if (tail.prev != head){
            SnakeController.Instance.DestroyNode(tail.prev);
            Remove(tail.prev);
        }
    }

    private void Insert(SnakeNode prev, SnakeNode node){
        var next = prev.next;
        prev.next = node;
        node.prev = prev;
        node.next = next;
        if (next != null)
            next.prev = node;
    }
    private void Remove(SnakeNode node){
        var prev = node.prev;
        var next = node.next;
        prev.next = next;
        if (next != null)
            next.prev = prev;
    }

    private void RespawnApple(){
        int row, col;
        do
        {
            row = Random.Range(0, setting.rows);
            col = Random.Range(0, setting.cols);
        } while (gridMap[row, col] != null);

        var apple = SnakePrefabFactory.Instance.CreateApple();
        var node = apple.GetComponent<BaseNode>();
        node.gridPos = new Vector2Int(row, col);
        gridMap[row, col] = node;
        SnakeController.Instance.MoveNode(node);
    }
}
