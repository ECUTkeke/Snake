using UnityEngine;
using System;

public class SnakePrefabFactory : MonoBehaviour
{
    public static SnakePrefabFactory Instance;

    [SerializeField] GameObject snackHeadPrefab;
    [SerializeField] GameObject snakeTailPrefab;
    [SerializeField] GameObject snackBodyPrefab;
    [SerializeField] GameObject snackCornerPrefab;
    [SerializeField] GameObject applePrefab;
    GameObject snakeRoot;

    private void Awake() {
        if (Instance != null)
            Debug.LogError("More than one SnakePrefabFactory instance");

        Instance = this;
        Initialize();
    }

    public GameObject CreateSnakeNode(Type type) {
        GameObject obj = null;
        if (type == typeof(SnakeHead))
            obj = Instantiate(snackHeadPrefab, new Vector3(0, 0, 0), Quaternion.identity, snakeRoot.transform);
        else if (type == typeof(SnakeTail))
            obj = Instantiate(snakeTailPrefab, new Vector3(0, 0, 0), Quaternion.identity, snakeRoot.transform);
        else if (type == typeof(SnakeNode))
            obj = Instantiate(snackBodyPrefab, new Vector3(0, 0, 0), Quaternion.identity, snakeRoot.transform);
        else if (type == typeof(SnakeCorner))
            obj = Instantiate(snackCornerPrefab, new Vector3(0, 0, 0), Quaternion.identity, snakeRoot.transform);

        SnakeNode node = obj?.GetComponent<SnakeNode>();
        if (node != null)
            node.IsNewCreated = true;

        return obj;
    }
    public GameObject CreateApple() {
        var obj = Instantiate(applePrefab, new Vector3(0, 0, 0), Quaternion.identity, snakeRoot.transform);
        obj.GetComponent<BaseNode>().IsNewCreated = true;
        var render = obj.GetComponent<SpriteRenderer>();
        var node = obj.GetComponent<BaseNode>();
        // Four sprite is same, no matter what direction
        render.sprite = node.up;

        return obj;
    }

    public void Initialize() {
        if (snakeRoot != null)
            Destroy(snakeRoot);
        snakeRoot = new GameObject("SnakeRoot");
    }
}
