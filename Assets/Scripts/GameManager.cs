using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices.WindowsRuntime;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action OnEatApple;

    readonly public List<Vector2Int> directions = new List<Vector2Int> {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down
    };

    [SerializeField] GameObject snackHeadPrefab;
    [SerializeField] GameObject snackTailPrefab;
    [SerializeField] GameObject snackBodyPrefab;
    [SerializeField] GameObject snackCornerPrefab;
    [SerializeField] GameObject ApplePrefab;

    private BaseNode[,] gridMap;
    private float moveTimer = 0;
    private Vector2Int? inputDirection = null;
    private GameObject currApple;

    [SerializeField] private GameSetting setting;
    public GameSetting Setting => setting;

    public bool CheckOutOfBounds(int row, int col){
        return row < 0 || row >= setting.rows || col < 0 || col >= setting.cols;
    }

    // This project require every sprite as a square in 1 * 1 ratio after pixels transform to untiy unit

    // Dont forget change the PPU of sprite to correct, otherwise the scale will not be correct
    public Vector2 Grid2WorldPosition(int row, int col){
        var offset = new Vector2(setting.cols, setting.rows) / 2;

        return (new Vector2(col ,row) - offset) * setting.scale;
    }

    private void Awake() {
        gridMap = new BaseNode[setting.rows, setting.cols];
    }
    private void Start() {
        RespwanSnack();
        RespawnApple();
    }

    public void RespwanSnack(){
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

        var headObj = Instantiate(snackHeadPrefab, Grid2WorldPosition(headRow, headCol), Quaternion.identity);
        var head = headObj.GetComponent<SnackHead>();
        gridMap[headRow, headCol] = head;
        head.Direction = direction;
        head.gridPos = new Vector2Int(headRow, headCol);

        var tailObj = Instantiate(snackTailPrefab, Grid2WorldPosition(tailRow, tailCol), Quaternion.identity);
        var tail = tailObj.GetComponent<SnackTail>();
        gridMap[tailRow, tailCol] = tail;
        tail.Direction = direction;
        tail.gridPos = new Vector2Int(tailRow, tailCol);

        SnackListHandler.Insert(head, tail);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < setting.rows; i++)
        {
            for (int j = 0; j < setting.cols; j++)
            {
                Vector2 worldPos = Grid2WorldPosition(i, j);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(worldPos, 0.1f);
            }
        }
    }
#endif

    private void Update() {
        if (setting.moveSpeed == 0)
            return;

        var currDirection = SnackHead.Instance.Direction;
        if (inputDirection == null)
        {
            if (Input.GetKeyDown(KeyCode.W) && currDirection != Vector2Int.down)
                inputDirection = Vector2Int.up;
            else if (Input.GetKeyDown(KeyCode.S) && currDirection != Vector2Int.up)
                inputDirection = Vector2Int.down;
            else if (Input.GetKeyDown(KeyCode.A) && currDirection != Vector2Int.right)
                inputDirection = Vector2Int.left;
            else if (Input.GetKeyDown(KeyCode.D) && currDirection != Vector2Int.left)
                inputDirection = Vector2Int.right;
        }


        float moveInterval = 1 / setting.moveSpeed;        
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval){
            if (inputDirection != null){
                SnackHead.Instance.Direction = inputDirection.Value;
            }
            moveTimer = 0;
            MoveSnack();
            inputDirection = null;
        }
    }

    private void MoveSnack(){
        var eatedApple = false;

        var tail = SnackTail.Instance;
        var newTailPos = tail.prev.gridPos;
        var newTailDir = tail.prev.Direction;

        var head = SnackHead.Instance;
        var oldPos = head.gridPos;
        var newPos = head.gridPos + new Vector2Int(head.Direction.y, head.Direction.x);
        if (CheckOutOfBounds(newPos.x, newPos.y)){
            if (newPos.x < 0)
                newPos.x += setting.rows;
            if (newPos.y < 0)
                newPos.y += setting.cols;
            
            newPos.x %= setting.rows;
            newPos.y %= setting.cols;
        }

        if (gridMap[newPos.x, newPos.y] != null){
            if (gridMap[newPos.x, newPos.y] is SnackNode)
            {
                Debug.Log("Game Over");
                head.Direction = head.next.Direction;
                return;
            }
            else if (gridMap[newPos.x, newPos.y].gameObject.tag == "Apple"){
                OnEatApple?.Invoke();
                newTailDir = tail.Direction;
                newTailPos = tail.gridPos;
                eatedApple = true;
            }
        }

        GameObject gameObj;
        if (head.Direction != head.next.Direction)
        {
            gameObj = Instantiate(snackCornerPrefab, Grid2WorldPosition(oldPos.x, oldPos.y), Quaternion.identity);
            var corner = gameObj.GetComponent<SnackCorner>();
            corner.SetRelation(head, head.next);
        }
        else
        {
            gameObj = Instantiate(snackBodyPrefab, Grid2WorldPosition(oldPos.x, oldPos.y), Quaternion.identity);
        }
        var body = gameObj.GetComponent<SnackNode>();
        body.Direction = head.Direction;
        body.gridPos = oldPos;

        SnackListHandler.Insert(head, body);

        head.gridPos = newPos;

        gridMap[newPos.x, newPos.y] = head;
        gridMap[oldPos.x, oldPos.y] = body;
        gridMap[tail.gridPos.x, tail.gridPos.y] = null;
        gridMap[newTailPos.x, newTailPos.y] = tail; 

        tail.Direction = newTailDir;
        tail.gridPos = newTailPos;

        RefreshPosition(new SnackNode[]{head, tail});

        if (tail.prev != head && !eatedApple)
            SnackListHandler.Remove(tail.prev);
    }

    private void RefreshPosition(SnackNode[] nodes){
        foreach (var node in nodes){
            node.transform.position = Grid2WorldPosition(node.gridPos.x, node.gridPos.y);
        }
    }

    public void RespawnApple(){
        if (currApple != null)
            Destroy(currApple);

        int row, col;
        do{
            row = Random.Range(0, setting.rows);
            col = Random.Range(0, setting.cols);
        }while(gridMap[row, col] != null);

        currApple = Instantiate(ApplePrefab, Grid2WorldPosition(row, col), Quaternion.identity);
        var apple = currApple.GetComponent<BaseNode>();
        gridMap[row, col] = apple;
        var render = currApple.GetComponent<SpriteRenderer>();
        // Four sprite is same, no matter what direction
        render.sprite = apple.up;
    } 
    public void DestoryOldApple(){
        if (currApple == null)
            return;
        
        var gidPos = currApple.GetComponent<BaseNode>().gridPos;
        gridMap[gidPos.x, gidPos.y] = null;
        Destroy(currApple.gameObject);
    }
    public void EatAppleHandler(){
        DestoryOldApple();
        RespawnApple();
    }
    private void OnEnable() {
        OnEatApple += EatAppleHandler;
    }
    private void OnDisable() {
        OnEatApple -= EatAppleHandler;
    }
}
