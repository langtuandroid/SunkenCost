using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class Log : MonoBehaviour
{
    public static Log current;
    public GUIStyle guiStyle;
    
    // Private VARS
    private List<string> Eventlog = new List<string>();
    private string guiText = "";
 
    // Public VARS
    public int maxLines = 10;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - (Screen.height / 3), Screen.width / 3, Screen.height / 3), guiText, guiStyle);
    }
 
    public void AddEvent(string eventString)
    {
        Eventlog.Add(eventString);
 
        if (Eventlog.Count >= maxLines)
            Eventlog.RemoveAt(0);
 
        guiText = "";
 
        foreach (string logEvent in Eventlog)
        {
            guiText += logEvent;
            guiText += "\n";
        }
    }
}