using UnityEngine;

public class BorderCamera : MonoBehaviour
{
    private Camera myCamera;

    private void Awake() {
        myCamera = GetComponent<Camera>();
    }

    private void OnEnable() {
        SnakeController.OnGameStart += UpdateBorder;
    }
    private void OnDisable() {
        SnakeController.OnGameStart -= UpdateBorder;
    }

    private void UpdateBorder(){
        myCamera.rect = new Rect(0, 0, 1, 1);

        var leftBottom = myCamera.WorldToViewportPoint(SnakeController.Instance.Grid2WorldPosition(-1, -1));
        var rowAndCol = new Vector2Int(SnakeController.Instance.Setting.rows, SnakeController.Instance.Setting.cols);
        var rightTop = myCamera.WorldToViewportPoint(SnakeController.Instance.Grid2WorldPosition(rowAndCol.x + 1, rowAndCol.y + 1));

        var rect = new Rect(leftBottom.x, leftBottom.y, rightTop.x - leftBottom.x, rightTop.y - leftBottom.y);
        myCamera.rect = rect;
    }

}
