using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformEffectController : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Vector2 _maxScale;
    [SerializeField]
    private float _effectSpeed;
    private EventListener[] _eventListener;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        AddListeners();
    }

    private void Update()
    {
        transform.position = _player.position;

        Vector3 currentScale = transform.localScale;

        Debug.Log(currentScale);
        if (currentScale.x < _maxScale.x)
            currentScale.x += Time.deltaTime * _effectSpeed;
        else
            currentScale.x = _maxScale.x;

        if (currentScale.y < _maxScale.y)
            currentScale.y += Time.deltaTime * _effectSpeed;
        else
            currentScale.y = _maxScale.y;

        transform.localScale = currentScale;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // ========== Initialization Methods ==========
    private void AddListeners()
    {
        _eventListener = new EventListener[2];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_TRANSFORM_TOUCH, this, OnTransformTouch);
        _eventListener[1] = EventSystem.instance.AddListener(EventCode.ON_TRANSFORM_CLICK, this, OnTransformClick);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    // ========== Private Methods ==========
    private void OnTransformTouch(object[] eventParam)
    {
        LogUtils.instance.Log(GetClassName(), "OnTransformTouch");
        DoTransformEffect();
    }

    private void OnTransformClick(object[] eventParam)
    {
        LogUtils.instance.Log(GetClassName(), "OnTransformClick");
        DoTransformEffect();
    }
    private void DoTransformEffect()
    {
        transform.localScale = Vector3.zero;
    }
}
