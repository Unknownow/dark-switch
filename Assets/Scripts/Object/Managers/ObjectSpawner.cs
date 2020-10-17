using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    private BaseObject _currentBackground;

    // ========== MonoBehaviour Functions ==========
    private void Start()
    {
        InitBackground();
    }

    // ========== Public Functions ==========

    // ========== Private Functions ==========
    private void InitBackground()
    {
        _currentBackground = ObjectPool.instance.GetObject(ObjectType.BACK_GROUND);
        _currentBackground.isObjectActive = true;
        _currentBackground.transform.position = Vector3.zero;
        GameObject currentPlayer = GameObject.FindObjectOfType<Player>().gameObject;
        if (currentPlayer)
        {
            List<ObjectState> allowedState = currentPlayer.GetComponent<CollisionSystem>().allowedState;
            int randomAllowedStateIndex = Mathf.FloorToInt(Random.Range(0, allowedState.Count));
            _currentBackground.currentState = allowedState[randomAllowedStateIndex];
        }
        else
            _currentBackground.isObjectActive = false;
    }
}
