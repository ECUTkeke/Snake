using System;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public static SnakeController Instance;
    public static Action OnEatApple;
    public static Action OnGameOver;
    public static Action OnGameStart;

    [SerializeField] private GameSetting setting;
    public GameSetting Setting => setting;

    private SnakeModel model;
    private SnakeView view;
    private float moveTimer = 0;
    private Vector2Int? inputDirection = null;

    private bool isRunning = false;

    public void RunGame(){
        isRunning = true;

        SnakePrefabFactory.Instance.Initialize();
        SnakePrefabFactory.Instance.CreateSnakeNode(typeof(SnakeHead));
        SnakePrefabFactory.Instance.CreateSnakeNode(typeof(SnakeTail));

        model = new SnakeModel(setting);
        view = new SnakeView(setting);

        moveTimer = 0;
        inputDirection = null;

        model.InitializeSnake(); 
        view.UpdateStick();

        OnGameStart?.Invoke();
    }

    private void Awake() {
        if (Instance != null)
            Debug.LogError("More than one SnakeController instance");
        Instance = this;
    }
    private void OnEnable() {
        OnGameOver += GameOver;
    }
    private void OnDisable() {
        OnGameOver -= GameOver;
    }

    private void GameOver(){
        isRunning = false;
    }

    private void ReadInputs(){
        var currDirection = SnakeHead.Instance.Direction;
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
    }

    private void Update() {
        if (setting.moveSpeed == 0 || !isRunning)        
            return;
        ReadInputs();     
        moveTimer += Time.deltaTime;
        if (moveTimer >= 1 / setting.moveSpeed) {
            moveTimer = 0;
            if (inputDirection != null)
                SnakeHead.Instance.Direction = inputDirection.Value;
            
            inputDirection = null;

            model.UpdateStick();
            view.UpdateStick();
        }
    }


    public void MoveNode(BaseNode node) {
        view.MovingNodes.Add(node);
    }
    public void DestroyNode(BaseNode node) {
        view.WaitedDestory.Add(node);
    }
    public void Broadcast_OnEatApple(){
        OnEatApple?.Invoke();
    }
    public void Broadcast_OnGameOver(){
        OnGameOver?.Invoke();
    }

    public  Vector2 Grid2WorldPosition(int row, int col){
        return view.Grid2WorldPosition(row, col);
    }

#if UNITY_EDITOR && DEBUG
    private void OnDrawGizmos()
    {
        if (view == null)
            return;
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
