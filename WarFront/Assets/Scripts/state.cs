using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state : MonoBehaviour
{
    public Color32 color;

    public string name;
    public int population;

    public GameObject owner;
    public int occupyValue;
    public List<GameObject> cores = new List<GameObject>();

    public List<GameObject> resources = new List<GameObject>();
    public GameObject location;
}
