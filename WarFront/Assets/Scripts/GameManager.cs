using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dev")]
    [SerializeField] public bool devMode;
    [SerializeField] public GameObject devCountry;

    [Header("Game")]
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
            if (country.leaning == "")
            {
                if (country.ideology.GetComponent<ideology>().name == "Democracy")
                {
                    if (country.lean > 5)
                    {
                        country.leaning = "Pro American Democracy";
                    }
                    else if (country.lean < -5)
                    {
                        country.leaning = "Anti American Democracy";
                    }
                    else { country.leaning = "Neutral Democracy"; }
                }
                else if (country.ideology.GetComponent<ideology>().name == "Communism")
                {
                    if (country.lean > 5)
                    {
                        country.leaning = "Pro Soviet Communism";
                    }
                    else if (country.lean < -5)
                    {
                        country.leaning = "Anti Soviet Communism";
                    }
                    else { country.leaning = "Neutral Communism"; }
                }
                else
                {
                    if (country.lean > 5)
                    {
                        country.leaning = "Pro Western State";
                    }
                    else if (country.lean < -5)
                    {
                        country.leaning = "Pro Eastern State";
                    }
                    else { country.leaning = "Neutral State"; }
                }
            }
        }
    }

}
