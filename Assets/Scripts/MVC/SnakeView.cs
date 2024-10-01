using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class SnakeView 
{
    public HashSet<BaseNode> MovingNodes = new HashSet<BaseNode>();
    public HashSet<BaseNode> WaitedDestory = new HashSet<BaseNode>();

    private GameSetting setting;

    public SnakeView(GameSetting setting){
        this.setting = setting;
    }

    // This project require every sprite as a square in 1 * 1 ratio after pixels transform to untiy unit

    // Dont forget change the PPU of sprite to correct, otherwise the scale will not be correct
    public  Vector2 Grid2WorldPosition(int row, int col){
        var offset = new Vector2(setting.cols, setting.rows) / 2;

        return (new Vector2(col ,row) - offset) * setting.scale;
    }

    public void UpdateStick(){
        MovingNodes.Add(SnakeHead.Instance);
        MovingNodes.Add(SnakeTail.Instance);

        foreach (var node in WaitedDestory){
            MovingNodes.Remove(node);
            GameObject.Destroy(node.gameObject);
        }
        WaitedDestory.Clear();

        // CoroutineManager.Instance.StopAllCoroutines();
        // CoroutineManager.Instance.StartManagedCoroutine(MoveLerpCoroutine());

        foreach (var node in MovingNodes)
            node.transform.position = Grid2WorldPosition(node.gridPos.x, node.gridPos.y);
    }
    private IEnumerator MoveLerpCoroutine(){
        var originalPos = new Dictionary<BaseNode, Vector2>();
        var timer = 0f;
        var duration = 1 / setting.moveSpeed;

        if (SnakeTail.Instance.prev != SnakeHead.Instance){
            var lerpObj = GameObject.Instantiate(SnakeTail.Instance.prev.gameObject);
            lerpObj.transform.position = Grid2WorldPosition(SnakeTail.Instance.gridPos.x, SnakeTail.Instance.gridPos.y);
            GameObject.Destroy(lerpObj, duration);
        }

        foreach (var node in MovingNodes){
            if (node.IsNewCreated){
                node.transform.position = Grid2WorldPosition(node.gridPos.x, node.gridPos.y);
                node.IsNewCreated = false;
            }
            originalPos[node] = node.transform.position;
       }

        while (timer < duration){
            timer += Time.deltaTime;
            foreach (var node in MovingNodes){
                    node.transform.position = Vector2.Lerp(originalPos[node], Grid2WorldPosition(node.gridPos.x, node.gridPos.y), timer / duration);
            }
            yield return null;
        }

        MovingNodes.Clear();

        yield return null;
    }
}
