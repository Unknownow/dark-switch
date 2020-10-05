using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectState
{
    BLACK = 0,
    WHITE = 1
}

public class BaseObject : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private EventListener[] _eventListener = null;

    [SerializeField]
    private GameObject[] _stateObjectList;

    private ObjectState _currentState;


    // ========== Constructors ==========

    void Start()
    {
        InitObject();
        TransformState();
    }

    private void InitObject()
    {
        int stateCount = transform.childCount;
        _stateObjectList = new GameObject[stateCount];
        for (int i = 0; i < stateCount; i++)
        {
            _stateObjectList[i] = transform.GetChild(i).gameObject;
        }
        AddListeners();
    }

    private void ClearObject()
    {
        RemoveListeners();
    }

    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_TRANSFORM_TOUCH, this, OnTransformTouch);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    // ========== Methods ==========
    public virtual void TransformState(ObjectState state = ObjectState.WHITE)
    {
        _currentState = state;
        for (int i = 0; i < _stateObjectList.Length; i++)
            _stateObjectList[i].SetActive(i == (int)_currentState);
    }

    private void OnTransformTouch(object[] eventParam)
    {
        Touch touch = (Touch)eventParam[0];
        if (touch.phase == TouchPhase.Began)
        {
            TransformState((ObjectState)(((int)_currentState + 1) % 2));

        }
    }
}
