using UnityEngine;
using UnityEngine.UI;


public class UIHandler : MonoBehaviour
{
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private RectTransform speedPanel;
    [SerializeField] private RectTransform sizePanel;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private RectTransform[] ToggleShowUIs;

    private ToggleGroup speedGroup; 
    private ToggleGroup sizeGroup; 

    private void Awake() {
        speedGroup = speedPanel.GetComponent<ToggleGroup>();
        sizeGroup = sizePanel.GetComponent<ToggleGroup>();
    }

    private void OnEnable() {
        SnakeController.OnGameOver += OpenUI;    
    }
    private void OnDisable() {
        SnakeController.OnGameOver -= OpenUI; 
    }

    public void BootGame(){
        SpeedToggle speed = null;
        SizeToggle size = null;

        foreach (var speedToggle in speedGroup.ActiveToggles()){
            speed = speedToggle as SpeedToggle;
        }

        foreach (var sizeToggle in sizeGroup.ActiveToggles()){
            size = sizeToggle as SizeToggle;
        }

        gameSetting.moveSpeed = speed.moveSpeed;
        gameSetting.rows = size.rows;
        gameSetting.cols = size.columns;
        gameSetting.scale = size.scales;

        CloseUI();
        snakeController.RunGame();
    }

    public void CloseUI(){
        foreach (var ui in ToggleShowUIs){
            ui.gameObject.SetActive(false);
        }
    }

    public void OpenUI(){
        foreach (var ui in ToggleShowUIs){
            ui.gameObject.SetActive(true);
        }
    }
}
