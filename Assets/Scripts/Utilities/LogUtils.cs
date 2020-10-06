﻿using UnityEngine;
public class LogUtils
{
    public string getClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static LogUtils _instance;

    public static LogUtils instance
    {
        get
        {
            if (_instance == null)
                _instance = new LogUtils();
            return _instance;
        }
    }
    // ========== Constructors ==========
    private LogUtils()
    {
        
    }

    // ========== Public Methods ==========
    public void Log(params string[] logs)
    {
        string logString = "";
        for (int i = 0; i < logs.Length; i++)
        {
            logString += logs[i];
            if (i < logs.Length - 1)
                logString += " ";
        }
        Debug.Log(getClassName() + " LOG: " + logString);
    }
}
