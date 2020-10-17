using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static PlayerManager _instance;
    public static PlayerManager instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private GameObject _currentPlayer;
    public GameObject currentPlayer
    {
        get
        {
            if (_currentPlayer == null)
                _currentPlayer = GameObject.FindObjectOfType<Player>().gameObject;
            return this._currentPlayer;
        }
    }

    // ========== Constructors ==========
    private PlayerManager()
    {
        if (_instance == null)
            _instance = this;
    }
}
