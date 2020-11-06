using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    BASE,
    BACK_GROUND,
    OBSTACLE_LINE,
    OBSTACLE_SWING,
}

public enum ObjectState
{
    STATE_0 = 0,
    STATE_1 = 1
}

public class BaseObject : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    protected EventListener[] _eventListener = null;

    [SerializeField]
    protected GameObject[] _stateObjectList;

    [SerializeField]
    protected ObjectState _currentState = ObjectState.STATE_0;
    public ObjectState currentState
    {
        get
        {
            return this._currentState;
        }
        set
        {
            this._currentState = value;
        }
    }

    [SerializeField]
    protected ObjectType _type = ObjectType.BASE;
    public ObjectType type
    {
        get
        {
            return this._type;
        }
    }

    // ========== MonoBehaviour Methods ==========
    protected virtual void Awake()
    {
        InitObject();
        TransformState(_currentState);
    }

    protected void InitObject()
    {
        int stateCount = transform.childCount;
        _stateObjectList = new GameObject[stateCount];
        for (int i = 0; i < stateCount; i++)
            _stateObjectList[i] = transform.GetChild(i).gameObject;
        AddListeners();
    }

    protected void ClearObject()
    {
        RemoveListeners();
    }

    protected void OnDestroy()
    {
        RemoveListeners();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ObjectTrigger"))
            OnObjectTriggered();
        if (other.CompareTag("ObjectDestroyer"))
            OnObjectDestroyed();
    }

    protected void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_TRANSFORM_TOUCH, this, OnTransformTouch);
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_TRANSFORM_CLICK, this, OnTransformClick);
    }

    protected void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    // ========== Public Methods ==========
    public virtual void ResetObject()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float cameraHeight = Camera.main.orthographicSize * 2f;
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y + cameraHeight, 0);
    }

    public virtual void TransformState(ObjectState state = ObjectState.STATE_0)
    {
        LogUtils.instance.Log(GetClassName(), gameObject.name, "TransformState", state.ToString());
        _currentState = state;
        for (int i = 0; i < _stateObjectList.Length; i++)
        {
            _stateObjectList[i].SetActive(i == (int)_currentState);
        }
    }

    // ========== Protected Methods ==========
    protected void OnTransformTouch(object[] eventParam)
    {
        Touch touch = (Touch)eventParam[0];
        if (touch.phase == TouchPhase.Began)
        {
            TransformState((ObjectState)(((int)_currentState + 1) % 2));
        }
    }

    protected void OnTransformClick(object[] eventParam)
    {
        TransformState((ObjectState)(((int)_currentState + 1) % 2));
    }

    protected virtual void OnObjectTriggered()
    {
        LogUtils.instance.Log(GetClassName(), "OnTriggerObject", "OVERRIDE_ME");
    }

    protected virtual void OnObjectDestroyed()
    {
        LogUtils.instance.Log(GetClassName(), "OnObjectDestroyed", "OVERRIDE_ME", "REMEMBER_USE_BASE");
        gameObject.SetActive(false);
    }

    // ========== Protected Methods ==========
}
