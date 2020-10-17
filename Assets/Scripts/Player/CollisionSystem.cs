using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private LayerMask _objectMask;

    [SerializeField]
    private Vector2 _objectSize;

    [SerializeField]
    private List<ObjectState> _allowedState = new List<ObjectState>();
    private Collider2D _playerCollider;

    private float _deadCountdown = 0;
    // ========== MonoBehaviour Functions ==========
    private void Start()
    {
        _playerCollider = gameObject.GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckPlayerCollision();
    }

    // ========== Private Methods ==========
    private void CheckPlayerCollision()
    {
        if (_deadCountdown <= 0)
            return;

        bool isAllowedStateAvailable = IsAllowedStateAvailable();
        if (!isAllowedStateAvailable)
        {
            LogUtils.instance.Log(GetClassName(), "NO ALLOWED STATE AVAILABLE");
            _deadCountdown -= Time.deltaTime;
        }
        else
        {
            LogUtils.instance.Log(GetClassName(), "ALLOWED STATE AVAILABLE");
            _deadCountdown = PlayerConfig.TIME_BEFORE_DEAD;
        }

        if (_deadCountdown <= 0)
        {
            EventSystem.instance.DispatchEvent(EventCode.ON_PLAYER_DIED);
            LogUtils.instance.Log(GetClassName(), "ON PLAYER DIED");
        }
    }

    private bool IsAllowedStateAvailable()
    {
        bool hasAllowedState = true;
        Collider2D[] collidersList = Physics2D.OverlapBoxAll(transform.position, _objectSize, 0, _objectMask);

        if (collidersList.Length > 0)
        {
            BaseObject background = null;
            foreach (Collider2D collider in collidersList)
            {
                BaseObjectState baseObjectState = collider.transform.parent.GetComponent<BaseObjectState>();
                BaseObject baseObject = baseObjectState.parentObject;
                if (baseObject && baseObject.type == ObjectType.BACK_GROUND)
                {
                    background = baseObject;
                    hasAllowedState = CheckIfObjectIsInAllowedState(background);
                    break;
                }
            }

            if (background != null)
            {
                foreach (Collider2D collider in collidersList)
                {
                    BaseObjectState baseObjectState = collider.transform.parent.GetComponent<BaseObjectState>();
                    BaseObject baseObject = baseObjectState.parentObject;
                    if (baseObject.type == ObjectType.BACK_GROUND)
                        continue;
                    if (hasAllowedState)
                    {
                        if (!CheckIfObjectIsInAllowedState(baseObject) && CheckIfColliderIsWithinAnotherCollider(_playerCollider, collider))
                        {
                            hasAllowedState = false;
                            break;
                        }
                    }
                    else
                        hasAllowedState = CheckIfObjectIsInAllowedState(baseObject);
                }
            }
        }
        return hasAllowedState;
    }

    private bool CheckIfColliderIsWithinAnotherCollider(Collider2D checkCollider, Collider2D targetCollider)
    {
        if (targetCollider.bounds.Contains(checkCollider.bounds.min) && targetCollider.bounds.Contains(checkCollider.bounds.max))
            return true;
        return false;
    }

    private bool CheckIfObjectIsInAllowedState(BaseObject baseObject)
    {
        foreach (ObjectState state in _allowedState)
        {
            if (state != baseObject.currentState)
                return false;
        }
        return true;
    }
}
