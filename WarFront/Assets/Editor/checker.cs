using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Plastic.Newtonsoft.Json;

public class checker : EditorWindow
{
    TextAsset jsonFile;

    [MenuItem("Tools/jsonChecker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(checker));
    }

    [System.Obsolete]
    private void OnGUI()
    {
        GUILayout.Label("JSON Importer", EditorStyles.boldLabel);

        jsonFile = (TextAsset)EditorGUILayout.ObjectField("Countries File: ", jsonFile, typeof(TextAsset));

        if (GUILayout.Button("Check"))
        {
            check();
        }
    }

    private void check()
    {
        var dic = JsonConvert.DeserializeObject<SortedDictionary<string, int[]>>(jsonFile.text);
        var checkL = new List<string>();

        foreach (var c in dic)
        {
            checkL.Add(c.Key); 
        }

        int x = 0;
        while (x < checkL.Count)
        {
            Color32 check = new Color32((byte)dic[checkL[x]][0], (byte)dic[checkL[x]][1], (byte)dic[checkL[x]][2], 255);
            foreach (var c in dic)
            {
                Color32 check2 = new Color32((byte)c.Value[0], (byte)c.Value[1], (byte)c.Value[2], 255);
                if (check.Equals(check2) && !(checkL[x] == c.Key))
                {
                    Debug.Log(checkL[x] + "'s color is the same as: " + c.Key);
                    checkL.Remove(c.Key);
                }
            }
            x++;
        }


        //foreach (var c in dic)
        //{
        //    Color32 check = new Color32((byte)c.Value[0], (byte)c.Value[1], (byte)c.Value[2], 255);
        //    dic.Remove(c.Key);
        //    foreach (var c2 in dic)
        //    {
        //        Color32 check2 = new Color32((byte)c2.Value[0], (byte)c2.Value[1], (byte)c2.Value[2], 255);
        //        if (check.Equals(check2))
        //        {
        //            dic.Remove(c2.Key);
        //            Debug.Log(c.Key + "'s Color is the same as: " + c2.Key);
        //        }
        //    }
        //}
    }
}
