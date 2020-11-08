using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private float _minSpawnCountdown = 2;
    [SerializeField]
    private float _maxSpawnCountdown = 6;

    private float _spawnCountdown = 5;
    private bool _isSpawning = false;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        _isSpawning = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartSpawnObstacleOverTime();
        if (Input.GetKeyDown(KeyCode.S))
            StopSpawnObstacleOverTime();

        SpawnObstacleOverTime();
    }

    // ========== Public Methods ==========
    public void StartSpawnObstacleOverTime()
    {
        LogUtils.instance.Log(GetClassName(), "StartSpawnObstacleOverTime", "_isSpawning = ", _isSpawning.ToString());
        _isSpawning = true;
        _spawnCountdown = 0;
    }

    public void StopSpawnObstacleOverTime()
    {
        LogUtils.instance.Log(GetClassName(), "StopSpawnObstacleOverTime", "_isSpawning = ", _isSpawning.ToString());
        _isSpawning = false;
        _spawnCountdown = 0;
    }

    public void SpawnObstacle(ObjectType type)
    {
        GameObject obstacle = ObjectPool.instance.GetObject(type);
        GameObject currentBackground = GameObject.FindObjectOfType<BackgroundSpawner>().currentBackground;
        ObjectState currentBackgroundState = currentBackground.GetComponent<BaseObject>().currentState;
        ObjectState[] valueList = (ObjectState[])System.Enum.GetValues(typeof(ObjectState));
        obstacle.GetComponent<BaseObject>().TransformState(currentBackgroundState);
        obstacle.GetComponent<BaseObject>().ResetObject();
        obstacle.GetComponent<BaseObjectMovement>().MoveDown();
    }

    // ========== Private Methods ==========
    public void SpawnObstacleOverTime()
    {
        if (!_isSpawning)
            return;

        if (_spawnCountdown > 0)
        {
            _spawnCountdown -= Time.deltaTime;
            return;
        }
        _spawnCountdown = Random.Range(_minSpawnCountdown, _maxSpawnCountdown);

        ObjectType[] typeList = (ObjectType[])System.Enum.GetValues(typeof(ObjectType));

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, typeList.Length);
        }
        while (typeList[randomIndex] == ObjectType.BACK_GROUND || typeList[randomIndex] == ObjectType.BASE);

        SpawnObstacle(typeList[randomIndex]);
    }
}
