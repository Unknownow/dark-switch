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
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnObstacle(ObjectType.OBSTACLE_LINE);
        SpawnObstacleOverTime();
    }

    // ========== Public Methods ==========
    public void SpawnObstacle(ObjectType type)
    {
        GameObject obstacle = ObjectPool.instance.GetObject(type);
        Vector3 cameraPosition = Camera.main.transform.position;
        float cameraHeight = Camera.main.orthographicSize * 2f;
        obstacle.transform.position = new Vector3(cameraPosition.x, cameraPosition.y + cameraHeight, 0);
        obstacle.GetComponent<BaseObjectMovement>().MoveDownToEndScreen();
    }

    // ========== Private Methods ==========
    public void SpawnObstacleOverTime()
    {
        if (_spawnCountdown > 0)
        {
            _spawnCountdown -= Time.deltaTime;
            return;
        }
        _spawnCountdown = 5;
        SpawnObstacle(ObjectType.OBSTACLE_LINE);
    }
}
