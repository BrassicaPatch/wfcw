using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

public class importer : EditorWindow
{
    TextAsset jsonCountries;
    TextAsset jsonStates;

    Texture2D colorMap;
    Texture2D countryMap;

    [MenuItem("Tools/Json Importer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(importer));
    }

    [System.Obsolete]
    private void OnGUI()
    {
        GUILayout.Label("JSON Importer", EditorStyles.boldLabel);

        jsonCountries = (TextAsset)EditorGUILayout.ObjectField("Countries File: ", jsonCountries, typeof(TextAsset));
        jsonStates = (TextAsset)EditorGUILayout.ObjectField("States File: ", jsonStates, typeof(TextAsset));
        colorMap = (Texture2D)EditorGUILayout.ObjectField("ColorMap: ", colorMap, typeof(Texture2D), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        countryMap = (Texture2D)EditorGUILayout.ObjectField("Country Map: ", countryMap, typeof(Texture2D), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));

        if (GUILayout.Button("Import"))
        {
            //check();
            import();
        }
    }

    //private void check()
    //{
    //    int conflict = 0;
    //    var conflicts = new List<string>();

    //    var dic = JsonConvert.DeserializeObject<SortedDictionary<string, int[]>>(jsonFile.text);
    //    var checkL = new List<string>();

    //    foreach (var c in dic)
    //    {
    //        checkL.Add(c.Key);
    //    }

    //    int x = 0;
    //    while (x < checkL.Count)
    //    {
    //        Color32 check = new Color32((byte)dic[checkL[x]][0], (byte)dic[checkL[x]][1], (byte)dic[checkL[x]][2], 255);
    //        foreach (var c in dic)
    //        {
    //            Color32 check2 = new Color32((byte)c.Value[0], (byte)c.Value[1], (byte)c.Value[2], 255);
    //            if (check.Equals(check2))
    //            {
    //                checkL.Remove(c.Key);
    //                conflicts.Add(c.Key);
    //                conflict++;
    //                //Debug.Log(checkL[x] + "'s color is the same as: " + c.Key);
    //            }
    //        }
    //        x++;
    //    }

    //    while (conflict > 0)
    //    {
    //        int y = 0;
    //        while (y < conflicts.Count)
    //        {
    //            Color32 old = new Color32((byte)dic[conflicts[y]][0], (byte)dic[conflicts[y]][1], (byte)dic[conflicts[y]][2], 255);
    //            int r = old.r + Random.Range(-2, 2); int g = old.g + Random.Range(-2, 2); int b = old.b + Random.Range(-2, 2);
    //            if (r > 255) { r = 255; } if (r < 0) { r = 0; }
    //            if (g > 255) { g = 255; } if (g < 0) { g = 0; }
    //            if (b > 255) { }
    //        }
    //    }
    //}

    //private void changeColor()
    //{

    //}


    private void import()
    {
        Debug.Log("Import Started");

        List<GameObject> statesList = new List<GameObject>();
        List<GameObject> countriesList = new List<GameObject>();

        List<Color32> stateColorsList = new List<Color32>();
        List<Color32> checkedColorList = new List<Color32>();

        string countryText = jsonCountries.text;
        string stateText = jsonStates.text;
        var countryDict = JsonConvert.DeserializeObject<SortedDictionary<string, int[]>>(countryText);
        var stateDict = JsonConvert.DeserializeObject<SortedDictionary<string, int[]>>(stateText);

        GameObject stateGameObj = new GameObject("States");
        foreach(var s in stateDict)
        {
            GameObject GO = new GameObject(s.Key, typeof(state));
            Color32 newColor = new Color32((byte)s.Value[0], (byte)s.Value[1], (byte)s.Value[2], 255);
            stateColorsList.Add(newColor);

            GO.GetComponent<state>().name = s.Key;
            GO.GetComponent<state>().color = newColor;
            GO.transform.parent = stateGameObj.transform;
            statesList.Add(GO);
        }

        GameObject countryGameObject = new GameObject("Countries");
        foreach(var c in countryDict)
        {
            GameObject GO = new GameObject(c.Key, typeof(country));
            Color32 newColor = new Color32((byte)c.Value[0], (byte)c.Value[1], (byte)c.Value[2], 255);

            GO.GetComponent<country>().name = c.Key;
            GO.GetComponent<country>().color = newColor;
            GO.transform.parent = countryGameObject.transform;
            countriesList.Add(GO);
        }

        for (int x = 0; x < colorMap.width; x++)
        {
            for (int y = 0; y < colorMap.height; y++)
            {
                Color32 checkColor = colorMap.GetPixel(x, y);
                if (checkedColorList.Contains(checkColor)) { continue; }
                foreach(var s in statesList)
                {
                    if (s.GetComponent<state>().color.Equals(checkColor))
                    {
                        Color32 countryCheck = countryMap.GetPixel(x, y);
                        foreach(var c in countriesList)
                        {
                            if (c.GetComponent<country>().color.Equals(countryCheck))
                            {
                                s.GetComponent<state>().owner = c;
                                //c.GetComponent<country>().ownedStates.Add(s);
                            }
                        }
                    }
                }
                checkedColorList.Add(checkColor);
            }
        }

        foreach (var state in statesList)
        {
            if (state.GetComponent<state>().owner == null) { Debug.Log(state.GetComponent<state>().name + " has no owner"); continue; }
            foreach (var country in countriesList)
            {
                if (state.GetComponent<state>().owner == country)
                {
                    country.GetComponent<country>().ownedStates.Add(state);
                }
            }
        }




        //return;
        ////Debug.Log("For Starting");
        //for(int x = 0; x < colorMap.width; x++)
        //{
        //    for(int y = 0; y < colorMap.height; y++)
        //    {
        //        Color32 checkColor = colorMap.GetPixel(x, y);
        //        if (stateColorsList.Contains(checkColor))
        //        {
        //            if (checkedColorList.Contains(checkColor)) { continue; }
        //            //Debug.Log("For Each state starting");
        //            foreach (var state in statesList)
        //            {
        //                if (state.GetComponent<state>().color.Equals(checkColor))
        //                {
        //                    foreach (var country in countriesList)
        //                    {
        //                        Color32 countryMapColor = countryMap.GetPixel(x, y);
        //                        Debug.Log("CountryMap Color Got: " + countryMapColor + " === Country Color Got: " + country.GetComponent<country>().color);
        //                        if (countryMapColor.Equals(country.GetComponent<country>().color)){
        //                            state.GetComponent<state>().owner = country;
        //                            country.GetComponent<country>().ownedStates.Add(state);
        //                           // Debug.Log("Country Owner Added");
        //                        }
        //                    }
        //                }
        //            }
        //            checkedColorList.Add(colorMap.GetPixel(x, y));
        //        }
        //    }
        //}
        //Debug.Log("Import Finished");
    }
}
