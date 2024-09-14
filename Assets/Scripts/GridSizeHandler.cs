using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSizeHandler : MonoBehaviour
{
    public GameSetting gameSetting;


    public void Awake(){
        transform.localScale = new Vector3(gameSetting.scale, gameSetting.scale, gameSetting.scale);
    }
}
