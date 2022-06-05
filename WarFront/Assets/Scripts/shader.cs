using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class shader : MonoBehaviour
{
    int width;
    int height;

    [SerializeField] Material material;

    [SerializeField] Texture2D mainTex;
    [SerializeField] Texture2D remapTex;
    [SerializeField] Texture2D paletteTex;

    [SerializeField] Color32[] mainArr;
    [SerializeField] Color32[] remapArr;
    [SerializeField] Color32[] paletteArr;

    [SerializeField] Dictionary<Color32, Color32> main2remap;
    [SerializeField] Dictionary<Color32, GameObject> stateRef = new Dictionary<Color32, GameObject>();
    [SerializeField] Dictionary<Color32, Color32> remapStateColorRef = new Dictionary<Color32, Color32>();

    Color32 prevColor;
    bool selectAny = false;

    void originalStart()
    {
        foreach (GameObject state in GameObject.FindGameObjectsWithTag("state")) { stateRef.Add(state.GetComponent<state>().color, state); }

        material = GetComponent<Renderer>().material;
        mainTex = material.GetTexture("_MainTex") as Texture2D;
        mainArr = mainTex.GetPixels32();

        width = mainTex.width;
        height = mainTex.height;

        main2remap = new Dictionary<Color32, Color32>();
        remapArr = new Color32[mainArr.Length];
        int idx = 0;
        for (int i = 0; i < mainArr.Length; i++)
        {
            var mainColor = mainArr[i];
            if (!main2remap.ContainsKey(mainColor))
            {
                var low = (byte)(idx % 256);
                var high = (byte)(idx / 256);
                main2remap[mainColor] = new Color32(low, high, 0, 255);

                if (stateRef.ContainsKey(mainColor))
                {
                    remapStateColorRef.Add(main2remap[mainColor], mainColor);
                }

                idx++;
            }
            var remapColor = main2remap[mainColor];
            remapArr[i] = remapColor;
        }

        paletteArr = new Color32[256 * 256];
        for (int i = 0; i < paletteArr.Length; i++)
        {
            var x = (byte)(i % 256); var y = (byte)(i / 256);


            var checkRemap = new Color32(x, y, 0, 255); //remapTex.GetPixel(x, y);
            if (remapStateColorRef.ContainsKey(checkRemap))
            {
                paletteArr[i] = stateRef[remapStateColorRef[checkRemap]].GetComponent<state>().owner.GetComponent<country>().color;
            }
            else
            {
                paletteArr[i] = new Color32(255, 255, 255, 255);
            }
        }

        remapTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        remapTex.filterMode = FilterMode.Point;
        remapTex.SetPixels32(remapArr);
        remapTex.Apply(false);
        material.SetTexture("_RemapTex", remapTex);

        paletteTex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        paletteTex.filterMode = FilterMode.Point;
        paletteTex.SetPixels32(paletteArr);
        paletteTex.Apply(false);
        material.SetTexture("_PaletteTex", paletteTex);

    }


    void Update()
    {
        var uM = GameObject.Find("UI Canvas").GetComponent<uiManager>();
        if (Input.GetMouseButtonDown(0) && !uM.countryMenu.activeInHierarchy)
        {
            var mousePos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var p = hitInfo.point;
                int x = (int)Mathf.Floor(p.x) + width / 2;
                int y = (int)Mathf.Floor(p.y) + height / 2;

                var remapColor = remapArr[x + y * width];
                if (remapColor.Equals(new Color32(0, 0, 0, 255))) { return; }

                var state = stateRef[remapStateColorRef[remapColor]];

                uM.stateMenuShow(state);
            }
        }
        if (Input.GetMouseButtonDown(1) && !uM.countryMenu.activeInHierarchy)
        {
            var mousePos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var p = hitInfo.point;
                int x = (int)Mathf.Floor(p.x) + width / 2;
                int y = (int)Mathf.Floor(p.y) + height / 2;

                var remapColor = remapArr[x + y * width];
                if (remapColor.Equals(new Color32(0, 0, 0, 255))) { return; }

                var state = stateRef[remapStateColorRef[remapColor]];
                var country = state.GetComponent<state>().owner;

                uM.countryMenuShow(country);
            }
        }

        if (Application.isEditor)
        {
            if (material == null || mainTex == null || mainArr == null || remapTex == null || paletteTex == null || stateRef.Count == 0)
            {
                cvars(); //Debug.Log("Create Variables Called");
            }
            
            if (material.GetTexture("_RemapTex") == null) { material.SetTexture("_RemapTex", remapTex); }

            uPalette(); //Debug.Log("Update Palette Called");
        }
    }

    void cvars()
    {
        foreach (GameObject state in GameObject.FindGameObjectsWithTag("state")) { stateRef.Add(state.GetComponent<state>().color, state); }

        material = GetComponent<Renderer>().sharedMaterial;
        mainTex = material.GetTexture("_MainTex") as Texture2D;
        mainArr = mainTex.GetPixels32();

        width = mainTex.width;
        height = mainTex.height;

        main2remap = new Dictionary<Color32, Color32>();
        remapArr = new Color32[mainArr.Length];
        int idx = 0;
        for (int i = 0; i < mainArr.Length; i++)
        {
            var mainColor = mainArr[i];
            if (!main2remap.ContainsKey(mainColor))
            {
                var low = (byte)(idx % 256);
                var high = (byte)(idx / 256);
                main2remap[mainColor] = new Color32(low, high, 0, 255);

                if (stateRef.ContainsKey(mainColor))
                {
                    remapStateColorRef.Add(main2remap[mainColor], mainColor);
                }

                idx++;
            }
            var remapColor = main2remap[mainColor];
            remapArr[i] = remapColor;
        }

        remapTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        remapTex.filterMode = FilterMode.Point;
        remapTex.SetPixels32(remapArr);
        remapTex.Apply(false);
        material.SetTexture("_RemapTex", remapTex);
    }

    void uPalette()
    {
        paletteArr = new Color32[256 * 256];
        for (int i = 0; i < paletteArr.Length; i++)
        {
            var x = (byte)(i % 256); var y = (byte)(i / 256);


            var checkRemap = new Color32(x, y, 0, 255); //remapTex.GetPixel(x, y);
            if (remapStateColorRef.ContainsKey(checkRemap))
            {
                paletteArr[i] = stateRef[remapStateColorRef[checkRemap]].GetComponent<state>().owner.GetComponent<country>().color;
            }
            else
            {
                paletteArr[i] = new Color32(255, 255, 255, 255);
            }
        }

        paletteTex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        paletteTex.filterMode = FilterMode.Point;
        paletteTex.SetPixels32(paletteArr);
        paletteTex.Apply(false);
        material.SetTexture("_PaletteTex", paletteTex);
    }
}
