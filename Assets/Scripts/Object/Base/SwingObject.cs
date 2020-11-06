using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SwingSide
{
    LEFT,
    RIGHT
}

public class SwingObject : BaseObject
{
    // ========== Fields and properties ==========
    [SerializeField]
    private float LeftSideXPosition = -3.75f;

    private SwingSide _currentSide;

    [SerializeField]
    private float _rotateUpSpeed;
    public float rotateUpSpeed { get { return this._rotateUpSpeed; } set { this._rotateUpSpeed = value; } }

    [SerializeField]
    private float _rotateDownSpeed;
    public float rotateDownSpeed { get { return this._rotateDownSpeed; } set { this._rotateDownSpeed = value; } }

    [SerializeField]
    private float _leftSideEndAngle;

    [SerializeField]
    private float _rightSideEndAngle;

    public bool _rotatePhase = true; // true: rotate down, false: rotate up

    // ========== MonoBehaviour Methods ==========
    protected override void Awake()
    {
        base.Awake();
        _type = ObjectType.OBSTACLE_SWING;
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
            return;
        RotateUpdate();
    }

    // ========== Public Methods ==========
    public override void ResetObject()
    {
        base.ResetObject();
        float XPosition;
        SwingSide[] swingSideList = (SwingSide[])System.Enum.GetValues(typeof(SwingSide));
        SwingSide currentSide = swingSideList[Random.Range(0, swingSideList.Length)];
        _currentSide = currentSide;
        switch (currentSide)
        {
            case SwingSide.LEFT:
                XPosition = LeftSideXPosition;
                break;
            case SwingSide.RIGHT:
                XPosition = -LeftSideXPosition;
                break;
            default:
                XPosition = LeftSideXPosition;
                break;
        }
        transform.position = new Vector3(XPosition, transform.position.y, transform.position.z);
    }
    // ========== Protected Methods ==========
    protected override void OnObjectTriggered()
    {
        switch (_currentSide)
        {
            case SwingSide.LEFT:
                transform.rotation = Quaternion.Euler(0, 0, _leftSideEndAngle);
                break;
            case SwingSide.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, _rightSideEndAngle);
                break;
            default:
                transform.rotation = Quaternion.Euler(0, 0, _leftSideEndAngle);
                break;
        }
        _rotatePhase = true;
    }
    // ========== Private Methods ==========
    private void RotateUpdate()
    {
        switch (_currentSide)
        {
            case SwingSide.LEFT:
                LeftSideRotate();
                break;
            case SwingSide.RIGHT:
                RightSideRotate();
                break;
            default:
                LeftSideRotate();
                break;
        }
    }

    private void LeftSideRotate()
    {
        if (_rotatePhase)
        {
            if (transform.eulerAngles.z > 90 || transform.eulerAngles.z < _leftSideEndAngle)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _leftSideEndAngle);
                _rotatePhase = false;
                return;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - Time.deltaTime * rotateDownSpeed);
        }
        else
        {
            if (transform.eulerAngles.z > 90 || transform.eulerAngles.z < _leftSideEndAngle)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
                _rotatePhase = true;
                return;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Time.deltaTime * rotateUpSpeed);
        }
    }

    private void RightSideRotate()
    {
        if (_rotatePhase)
        {
            if (transform.eulerAngles.z < 90 || transform.eulerAngles.z > _rightSideEndAngle)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _rightSideEndAngle);
                _rotatePhase = false;
                return;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Time.deltaTime * rotateDownSpeed);
        }
        else
        {
            if (transform.eulerAngles.z < 90 || transform.eulerAngles.z > _rightSideEndAngle)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
                _rotatePhase = true;
                return;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - Time.deltaTime * rotateUpSpeed);
        }
    }

}
