using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObject : BaseObject
{
    [SerializeField]
    private ObjectState _defaultState = ObjectState.STATE_0;
    private void Start() {
        this._currentState = _defaultState;
        this._type = ObjectType.BACK_GROUND;
    }
}
