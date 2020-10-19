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
    private GameObject _currentBackground;
    private GameObject _lastBackground;
    private GameObject _currentPlayer;
    public GameObject currentPlayer
    {
        get
        {
            if (_currentPlayer == null)
                _currentPlayer = GameObject.FindObjectOfType<Player>().gameObject;
            return _currentPlayer;
        }
    }

    private bool _isBackgroundMoving = false;

    // ========== MonoBehaviour Methods ==========
    private void Start()
    {
        InitBackground();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SpawnNextBackground();
        // }
    }

    // ========== Public Methods ==========

    // ========== Private Methods ==========
    private void InitBackground()
    {
        _currentBackground = ObjectPool.instance.GetObject(ObjectType.BACK_GROUND);
        _currentBackground.transform.position = Vector3.zero;
        if (currentPlayer)
        {
            List<ColliderType> allowedCollider = currentPlayer.GetComponent<CollisionSystem>().allowedCollider;
            int randomAllowedStateIndex = Mathf.FloorToInt(Random.Range(0, allowedCollider.Count));
            _currentBackground.GetComponent<BackgroundObject>().TransformRandomlyToStateWithAllowedCollider(allowedCollider);
        }
        else
            _currentBackground.gameObject.SetActive(false);
        _isBackgroundMoving = false;
    }

    private void SpawnNextBackground(bool isBackgroundAllowed = false)
    {
        if (_isBackgroundMoving)
            return;
        _isBackgroundMoving = true;

        GameObject newBackground = ObjectPool.instance.GetObject(ObjectType.BACK_GROUND);

        newBackground.transform.position = new Vector3(0, Camera.main.orthographicSize * 2f - 0.2f, 0);
        newBackground.GetComponent<BaseObjectMovement>().MoveTo(Vector3.zero);
        float movingTime = _currentBackground.GetComponent<BaseObjectMovement>().MoveTo(new Vector3(0, -Camera.main.orthographicSize * 2f, 0));
        _lastBackground = _currentBackground;
        _currentBackground = newBackground;

        if (currentPlayer)
        {
            List<ColliderType> allowedCollider = currentPlayer.GetComponent<CollisionSystem>().allowedCollider;
            int randomAllowedStateIndex = Mathf.FloorToInt(Random.Range(0, allowedCollider.Count));
            if (isBackgroundAllowed)
                _currentBackground.GetComponent<BackgroundObject>().TransformRandomlyToStateWithAllowedCollider(allowedCollider);
            else
                _currentBackground.GetComponent<BackgroundObject>().TransformRandomlyToStateWithUnallowedCollider(allowedCollider);
        }
        else
            _currentBackground.gameObject.SetActive(false);

        Invoke("OnBackgroundMovingDone", movingTime);
    }

    private void OnBackgroundMovingDone()
    {
        _isBackgroundMoving = false;
        _lastBackground.SetActive(false);
    }
}
