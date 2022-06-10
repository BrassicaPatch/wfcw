using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class state : MonoBehaviour
{
    public Color32 color;

    public string name;
    public int population;

    public GameObject owner;
    public int occupyCost;
    public List<GameObject> cores = new List<GameObject>();

    [Header("Resources")]

    public int oil = 0;
    public int food = 0;
    public int rawMaterial = 0;
    public int consumerGoods = 0;

    //[CustomPropertyDrawer(typeof(resource))]
    //public Dictionary<GameObject, int> resource = new Dictionary<GameObject, int>();

    public GameObject location;
}
