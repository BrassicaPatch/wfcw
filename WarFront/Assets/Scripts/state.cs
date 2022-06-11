using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class state : MonoBehaviour
{
    public Color32 color;

    public string name;
    public float population;
    public int localDevelopment;

    public GameObject owner;
    public int occupyCost;
    public int occupyResistance;
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
