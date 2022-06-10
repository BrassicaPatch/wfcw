using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dev")]
    [SerializeField] public bool devMode;
    [SerializeField] public GameObject devCountry;

    [Header("Game")]
    public int mapView = 0;
    public int turn = 0;
    public GameObject player;

    public List<GameObject> states = new List<GameObject>();
    public List<GameObject> countries = new List<GameObject>();

    public void Start()
    {
        foreach ( var sI in GameObject.FindGameObjectsWithTag("state")) { states.Add(sI); }
        foreach ( var cI in GameObject.FindGameObjectsWithTag("country")) { countries.Add(cI);  }

        foreach (var c in countries)
        {
            var country = c.GetComponent<country>();
            if (country.lean > 2) { country.leaning = "Slightly Pro-West"; }
            else if (country.lean < -2) { country.leaning = "Slightly Pro-East"; }
            else if (country.lean >= 5) { country.leaning = "Pro-West"; }
            else if (country.lean <= -5) { country.leaning = "Pro-East"; }
            else if (country.lean >= 8) { country.leaning = "Strong Pro-West"; }
            else if (country.lean <= -8) { country.leaning = "Strong Pro-East"; }
            else { country.leaning = "Neutral"; }
        }
    }

}
