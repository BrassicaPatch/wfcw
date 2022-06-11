using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class development : EditorWindow
{
    [MenuItem("Tools/dev2state")]

    public static void ShowWindow()
    {
        GetWindow(typeof(development));
    }
    private void OnGUI()
    {
        GUILayout.Label("Dev To States", EditorStyles.boldLabel);

        if (GUILayout.Button("Run"))
        {
            toStates();
        }
    }
    void toStates()
    {
        foreach(var state in GameObject.FindGameObjectsWithTag("state"))
        {
            state.GetComponent<state>().localDevelopment = state.GetComponent<state>().cores[0].GetComponent<country>().developmentIndex;
        }
    }
}
