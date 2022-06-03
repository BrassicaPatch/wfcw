using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class country : MonoBehaviour
{
    //Country Properties
    public Color32 color;
    public string name;

    //Country Ideolological Standing
    public GameObject ideology;
    public string leaning;
    public int lean;

    //Country Stuffs
    public float money;
    public float income;
    public float unrest;
    public int corruption;

    public Dictionary<GameObject, int> resourcePro = new Dictionary<GameObject, int>();
    public Dictionary<GameObject, int> resourceReq = new Dictionary<GameObject, int>();

    public Dictionary<GameObject, int> relations = new Dictionary<GameObject, int>();

    public List<GameObject> ownedStates = new List<GameObject>();
}
