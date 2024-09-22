using System;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public static SnakeController Instance;

    [SerializeField] private GameSetting setting;

    private SnakeModel model;
    private SnakeView view;
    
    private float moveTimer = 0;

    private void Awake() {
        model = new SnakeModel(setting);
        view = new SnakeView(setting);
    }

    public void Start() {
        SnakePrefabFactory.Instance.CreateSnakeNode(typeof(SnackHead));
        SnakePrefabFactory.Instance.CreateSnakeNode(typeof(SnakeTail));
        
        model.InitializeSnake();
        view.UpdateStick();
    }

    private void Update() {
        if (setting.moveSpeed == 0)        
            return;
        
        moveTimer += Time.deltaTime;
        if (moveTimer >= 1 / setting.moveSpeed) {
            moveTimer = 0;

            model.UpdateStick();
            view.UpdateStick();
        }
    }


    public void MoveNode(SnakeNode node) {
        view.MovingNodes.Add(node);
    }
    public void DestroyNode(SnakeNode node) {
        view.WaitedDestory.Add(node);
    }

#if UNITY_EDITOR && DEBUG
    private Vector2 Grid2WorldPosition(int row, int col){
        var offset = new Vector2(setting.cols, setting.rows) / 2;

        return (new Vector2(col ,row) - offset) * setting.scale;
    }
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
