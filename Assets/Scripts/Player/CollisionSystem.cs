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
    private List<ColliderType> _allowedCollider = new List<ColliderType>();
    public List<ColliderType> allowedCollider
    {
        get
        {
            return _allowedCollider;
        }
    }
    private Collider2D _playerCollider;

    private float _deadCountdown = PlayerConfig.TIME_BEFORE_DEAD;
    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        _deadCountdown = PlayerConfig.TIME_BEFORE_DEAD;
    }

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

        if (CheckInstantDieCollision())
        {
            _deadCountdown = -1;
            EventSystem.instance.DispatchEvent(EventCode.ON_PLAYER_DIED);
            LogUtils.instance.Log(GetClassName(), "ON PLAYER DIED");
        }

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
        bool hasAllowedCollider = false;
        Collider2D[] collidersList = Physics2D.OverlapBoxAll(transform.position, _objectSize, 0, _objectMask);
        if (collidersList.Length > 0)
        {
            List<BaseObjectCollider> backgroundColliderList = new List<BaseObjectCollider>();
            foreach (Collider2D collider in collidersList)
            {
                BaseObjectCollider baseObjectCollider = collider.gameObject.GetComponent<BaseObjectCollider>();
                if (baseObjectCollider == null)
                    continue;

                BaseObject baseObject = collider.transform.parent.parent.GetComponent<BaseObject>();
                if (baseObject == null || baseObject.type != ObjectType.BACK_GROUND)
                    continue;

                hasAllowedCollider = CheckIfObjectColliderIsInAllowedCollider(baseObjectCollider);
                if (hasAllowedCollider)
                    break;
            }

            foreach (Collider2D collider in collidersList)
            {
                BaseObjectCollider baseObjectCollider = collider.gameObject.GetComponent<BaseObjectCollider>();
                if (baseObjectCollider == null)
                    continue;

                BaseObject baseObject = collider.transform.parent.parent.GetComponent<BaseObject>();
                if (baseObject == null || baseObject.type == ObjectType.BACK_GROUND)
                    continue;

                if (hasAllowedCollider)
                {
                    bool isInUnallowedCollider = !CheckIfObjectColliderIsInAllowedCollider(baseObjectCollider) && CheckIfColliderIsWithinAnotherCollider(_playerCollider, collider);
                    hasAllowedCollider = !isInUnallowedCollider;
                    if (!hasAllowedCollider)
                        break;
                }
                else
                {
                    hasAllowedCollider = CheckIfObjectColliderIsInAllowedCollider(baseObjectCollider);
                    if (hasAllowedCollider)
                        break;
                }
            }
        }

        return hasAllowedCollider;
    }

    private bool CheckInstantDieCollision()
    {
        bool isCollideWithSpike = false;
        Collider2D[] collidersList = Physics2D.OverlapBoxAll(transform.position, _objectSize, 0, _objectMask);
        if (collidersList.Length > 0)
        {
            foreach (Collider2D collider in collidersList)
            {
                BaseObjectCollider baseObjectCollider = collider.gameObject.GetComponent<BaseObjectCollider>();
                if (baseObjectCollider == null)
                    continue;

                isCollideWithSpike = (baseObjectCollider.type == ColliderType.INSTANT_DIE);
                if (isCollideWithSpike)
                    break;
            }
        }

        return isCollideWithSpike;
    }

    private bool CheckIfColliderIsWithinAnotherCollider(Collider2D checkCollider, Collider2D targetCollider)
    {
        if (targetCollider.bounds.Contains(checkCollider.bounds.min) && targetCollider.bounds.Contains(checkCollider.bounds.max))
            return true;
        return false;
    }

    private bool CheckIfObjectColliderIsInAllowedCollider(BaseObjectCollider baseObjectCollider)
    {
        foreach (ColliderType type in allowedCollider)
        {
            if (type == baseObjectCollider.type)
                return true;
        }
        return false;
    }
}
