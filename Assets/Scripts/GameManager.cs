using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    readonly public List<Vector2Int> directions = new List<Vector2Int> {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down
    };

    [SerializeField] GameObject snackHeadPrefab;
    [SerializeField] GameObject snackTailPrefab;

    private BaseNode[,] gridMap;

    [SerializeField] private GameSetting setting;
    public GameSetting Setting => setting;

    public bool CheckOutOfBounds(int row, int col){
        return row < 0 || row >= setting.rows || col < 0 || col >= setting.cols;
    }
    public Vector2 Grid2WorldPosition(int row, int col){
        var offset = new Vector2(setting.cols, setting.rows) / 2;

        return (new Vector2(col ,row) - offset) * setting.scale;
    }

    private void Awake() {
        gridMap = new BaseNode[setting.rows, setting.cols];
    }
    private void Start() {
        var a = Grid2WorldPosition(0, 0);
        var b = Grid2WorldPosition(0, 1);
        RespwanSnack();
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
#if DEBUG
        head.gridPos = new Vector2Int(headRow, headCol);
#endif

        var tailObj = Instantiate(snackTailPrefab, Grid2WorldPosition(tailRow, tailCol), Quaternion.identity);
        var tail = tailObj.GetComponent<SnackTail>();
        gridMap[tailRow, tailCol] = tail;
        tail.Direction = direction;
#if DEBUG
        tail.gridPos = new Vector2Int(tailRow, tailCol);
#endif

        head.next = tail;
        tail.prev = head;
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
}
