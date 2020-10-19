using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectMovement : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private float _movementSpeed = 0;
    public float movementSpeed
    {
        get
        {
            return this._movementSpeed;
        }
    }
    private float _movementSpeedMultiplier = 1;
    public float movementSpeedMultiplier
    {
        get
        {
            return this._movementSpeedMultiplier;
        }
        set
        {
            this._movementSpeedMultiplier = value;
        }
    }

    [SerializeField]
    private float _angularSpeed = 0;
    public float angularSpeed
    {
        get
        {
            return this._angularSpeed;
        }
    }
    private float _angularSpeedMultiplier = 1;
    public float angularSpeedMultiplier
    {
        get
        {
            return this._angularSpeedMultiplier;
        }
        set
        {
            this._angularSpeedMultiplier = value;
        }
    }

    private Vector3 velocity;
    private Vector3 endPosition;

    // ========== MonoBehaviour Methods ==========
    protected virtual void Awake()
    {
        endPosition = transform.position;
        velocity = Vector3.zero;
    }

    protected virtual void Update()
    {
        MoveWithVelocity();
    }

    // ========== Public Methods ==========
    public float MoveTo(Vector3 endPosition)
    {
        velocity = endPosition - transform.position;
        velocity = velocity.normalized * movementSpeed * movementSpeedMultiplier;
        this.endPosition = endPosition;

        float travelTime = Vector3.Distance(transform.position, endPosition) / velocity.magnitude;
        return travelTime;
    }

    public float MoveBy(Vector3 distance)
    {
        endPosition = transform.position + distance;
        velocity = distance.normalized * movementSpeed * movementSpeedMultiplier;

        float travelTime = Vector3.Distance(transform.position, endPosition) / velocity.magnitude;
        return travelTime;
    }

    // ========== Protected Methods ==========
    protected void MoveWithVelocity()
    {
        if (transform.position == endPosition)
            return;
        if (Vector3.Distance(transform.position, endPosition) <= movementSpeed * movementSpeedMultiplier * Time.deltaTime)
        {
            transform.position = endPosition;
            return;
        }
        transform.position += velocity * Time.deltaTime;
    }
}