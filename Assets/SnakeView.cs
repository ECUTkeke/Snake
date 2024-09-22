using UnityEngine;
using System.Collections.Generic;


public class SnakeView 
{
    public HashSet<SnakeNode> MovingNodes = new HashSet<SnakeNode>();
    public HashSet<SnakeNode> WaitedDestory = new HashSet<SnakeNode>();

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
        MovingNodes.Add(SnackHead.Instance);
        MovingNodes.Add(SnakeTail.Instance);

        foreach (var node in WaitedDestory){
            MovingNodes.Remove(node);
            GameObject.Destroy(node.gameObject);
        }
        WaitedDestory.Clear();
        
        foreach (var node in MovingNodes){
            if (node.IsNewCreated)
                node.transform.position = Grid2WorldPosition(node.gridPos.x, node.gridPos.y);
            else{
                // #TODO: Use lerp
                node.transform.position = Grid2WorldPosition(node.gridPos.x, node.gridPos.y);
            }
        }

        MovingNodes.Clear();
    }
}
