using System.Collections.Generic;
using UnityEngine;



[ExecuteInEditMode]
public class DrawBounds : MonoBehaviour
{
    enum DrawType
    {
        Collider,
        Collider2D,
        Renderer,
        SpriteRenderer,
    }

    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private Collider _objectCollider = null;
    private Collider2D _objectCollider2d = null;
    private Renderer _objectRenderer = null;
    private SpriteRenderer _objectSpriteRenderer = null;
    private List<KeyValuePair<DrawType, Object>> _drawableList = new List<KeyValuePair<DrawType, Object>>();

    [SerializeField]
    private List<DrawType> _allowedBounds = new List<DrawType>();
    [SerializeField]
    private bool _isDrawOnSelected = false;
    [SerializeField]
    private bool _isDrawing = false;

    // ========== MonoBehaviour Methods ==========
    private void OnDrawGizmos()
    {
        if (!_isDrawing || _isDrawOnSelected)
            return;
        Init();
        Draw();
    }

    private void OnDrawGizmosSelected()
    {
        if (!_isDrawing || !_isDrawOnSelected)
            return;
        Init();
        Draw();
    }

    private void Draw()
    {
        foreach (DrawType type in _allowedBounds)
        {
            if (_drawableList.FindIndex(item => item.Key == type) == -1)
            {
                LogUtils.instance.Log(GetClassName(), System.Enum.GetName(typeof(DrawType), type), "NOT AVAILABLE");
                continue;
            }

            Bounds objectBounds = new Bounds(Vector3.zero, Vector3.zero);
            Color drawColor = Color.red;
            switch (type)
            {
                case DrawType.Collider:
                    objectBounds = _objectCollider.bounds;
                    drawColor = Color.green;
                    break;
                case DrawType.Collider2D:
                    objectBounds = _objectCollider2d.bounds;
                    drawColor = Color.green;
                    break;
                case DrawType.Renderer:
                    objectBounds = _objectRenderer.bounds;
                    drawColor = Color.blue;
                    break;
                case DrawType.SpriteRenderer:
                    objectBounds = _objectSpriteRenderer.bounds;
                    drawColor = Color.red;
                    break;
            }
            if (objectBounds.size.Equals(Vector3.zero))
                continue;
            Gizmos.color = drawColor;
            Gizmos.DrawWireCube(objectBounds.center, objectBounds.size);
        }
    }

    private void Init()
    {
        _objectCollider = gameObject.GetComponent<Collider>();
        _objectCollider2d = gameObject.GetComponent<Collider2D>();
        _objectRenderer = gameObject.GetComponent<Renderer>();
        _objectSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (_objectCollider != null)
            _drawableList.Add(new KeyValuePair<DrawType, Object>(DrawType.Collider, _objectCollider));
        if (_objectCollider2d != null)
            _drawableList.Add(new KeyValuePair<DrawType, Object>(DrawType.Collider2D, _objectCollider2d));
        if (_objectRenderer != null)
            _drawableList.Add(new KeyValuePair<DrawType, Object>(DrawType.Renderer, _objectRenderer));
        if (_objectSpriteRenderer != null)
            _drawableList.Add(new KeyValuePair<DrawType, Object>(DrawType.SpriteRenderer, _objectSpriteRenderer));
    }
}
