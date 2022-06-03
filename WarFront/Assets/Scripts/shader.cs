using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shader : MonoBehaviour
{
    int width;
    int height;

    Material material;

    Texture2D mainTex;
    Texture2D remapTex;
    Texture2D paletteTex;

    Color32[] mainArr;
    Color32[] remapArr;
    Color32[] paletteArr;

    Dictionary<Color32, Color32> main2remap;
    [SerializeField] Dictionary<Color32, GameObject> stateRef = new Dictionary<Color32, GameObject>();
    Dictionary<Color32, Color32> remapStateColorRef = new Dictionary<Color32, Color32>();

    Color32 prevColor;
    bool selectAny = false;

    void Start()
    {
        foreach(GameObject state in GameObject.FindGameObjectsWithTag("state")) { stateRef.Add(state.GetComponent<state>().color, state); }

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
                //if (mainColor.Equals(new Color32 (255, 255, 255, 255))) { Debug.Log("White Reference Color: " + main2remap[mainColor]); }

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

        //reloadPalette();

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

        //Make Clicking on Ocean close menus. - To be worked on
        //if ( (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && (uM.countryMenu.activeInHierarchy || uM.stateMenu.activeInHierarchy))
        //{


        //    uM.countryMenu.SetActive(false);
        //    uM.stateMenu.SetActive(false);
        //}
    }

    void changeColor(Color32 remapColor, Color32 showColor)
    {
        int xp = remapColor[0]; 
        int yp = remapColor[1];

        paletteTex.SetPixel(xp, yp, showColor);
    }

    void reloadPalette()
    {
        var material = GetComponent<Renderer>().material;
        var mainTex = material.GetTexture("_MainTex") as Texture2D;
        var remapTex = material.GetTexture("_RemapTex") as Texture2D;

        var paletteArr = new Color32[256 * 256];
        List<Color32> checkColorList = new List<Color32>();

        for (int x = 0; x < mainTex.width; x++)
        {
            for (int y = 0; y < mainTex.height; y++)
            {
                Color32 checkColor = mainTex.GetPixel(x, y);
                if (checkColorList.Contains(checkColor)) { continue; }
                foreach (var state in GameObject.FindGameObjectsWithTag("state"))
                {
                    Color32 remapColor = remapTex.GetPixel(x, y);
                    Color32 ownerColor = state.GetComponent<state>().owner.GetComponent<country>().color;
                    paletteArr[remapColor[0] + remapColor[1] * paletteArr.Length] = ownerColor;
                }
            }
        }
        paletteTex.SetPixels32(paletteArr);
        paletteTex.Apply(false);


        //Debug.Log("Reloaded Color Palette");
        //var material = GetComponent<Renderer>().material;                                          
        //var mainTex = material.GetTexture("_MainTex") as Texture2D;
        //var remapTex = material.GetTexture("_RemapTex") as Texture2D;

        //List<Color32> checkColorList = new List<Color32>();

        //foreach (GameObject state in GameObject.FindGameObjectsWithTag("state"))
        //{
        //    for (int x = 0; x < mainTex.width; x++)
        //    {
        //        for (int y = 0; y < mainTex.height; y++)
        //        {
        //            Color32 checkColor = mainTex.GetPixel(x, y);
        //            //if (checkColorList.Contains(checkColor)) { continue; }
        //            //if (state.GetComponent<state>().color.Equals(checkColor))
        //            //{
        //            //    Color32 remapColor = remapTex.GetPixel(x, y);
        //            //    Color32 ownerColor = state.GetComponent<state>().owner.GetComponent<country>().color;
        //            //    paletteTex.SetPixel(remapColor[0], remapColor[1], ownerColor);
        //            //    paletteTex.Apply(false);
        //            //}
        //            //checkColorList.Add(checkColor);
        //        }
        //    }
        //}
    }
}
