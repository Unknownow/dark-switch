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
    private BackgroundObject _currentBackground;

    // ========== MonoBehaviour Methods ==========
    private void Start()
    {
        InitBackground();
    }

    // ========== Public Methods ==========

    // ========== Private Methods ==========
    private void InitBackground()
    {
        _currentBackground = ObjectPool.instance.GetObject(ObjectType.BACK_GROUND).GetComponent<BackgroundObject>();
        _currentBackground.transform.position = Vector3.zero;
        GameObject currentPlayer = GameObject.FindObjectOfType<Player>().gameObject;
        if (currentPlayer)
        {
            List<ColliderType> allowedCollider = currentPlayer.GetComponent<CollisionSystem>().allowedCollider;
            int randomAllowedStateIndex = Mathf.FloorToInt(Random.Range(0, allowedCollider.Count));
            _currentBackground.TransformRandomlyToStateWithAllowedCollider(allowedCollider);
        }
        else
            _currentBackground.gameObject.SetActive(false);
    }
}
