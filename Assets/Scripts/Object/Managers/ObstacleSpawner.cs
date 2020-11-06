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
    private float _spawnCountdown = 5;

    // ========== MonoBehaviour Methods ==========

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnObstacle(ObjectType.OBSTACLE_SWING);
        // SpawnObstacleOverTime();
    }

    // ========== Public Methods ==========
    public void SpawnObstacle(ObjectType type)
    {
        GameObject obstacle = ObjectPool.instance.GetObject(type);
        GameObject currentBackground = GameObject.FindObjectOfType<BackgroundSpawner>().currentBackground;
        ObjectState currentBackgroundState = currentBackground.GetComponent<BaseObject>().currentState;
        ObjectState[] valueList = (ObjectState[])System.Enum.GetValues(typeof(ObjectState));
        // ObjectState randomState = currentBackgroundState;
        // do
        // {
        //     randomState = valueList[Random.Range(0, valueList.Length)];
        // }
        // while (randomState == currentBackgroundState);
        obstacle.GetComponent<BaseObject>().TransformState(currentBackgroundState);
        obstacle.GetComponent<BaseObject>().ResetObject();
        obstacle.GetComponent<BaseObjectMovement>().MoveDown();
    }

    // ========== Private Methods ==========
    public void SpawnObstacleOverTime()
    {
        if (_spawnCountdown > 0)
        {
            _spawnCountdown -= Time.deltaTime;
            return;
        }
        _spawnCountdown = 2;
        SpawnObstacle(ObjectType.OBSTACLE_LINE);
    }
}
