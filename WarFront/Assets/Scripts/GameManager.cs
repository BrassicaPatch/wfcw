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

        debugCheck();
    }

    public void debugCheck()
    {
        Debug.Log("Debug Color Check Called");
        var dic = new Dictionary<string, Color32>();
        var checkL = new List<string>();

        foreach(var c in countries)
        {
            dic.Add(c.GetComponent<country>().name, c.GetComponent<country>().color);
        }

        foreach (var c in dic)
        {
            checkL.Add(c.Key);
        }

        int x = 0;
        while (x < checkL.Count)
        {
            Color32 check = dic[checkL[x]];

            foreach (var c in dic)
            {
                Color32 check2 = c.Value; //new Color32((byte)c.Value[0], (byte)c.Value[1], (byte)c.Value[2], 255);
                if (check.Equals(check2) && !(checkL[x] == c.Key))
                {
                    Debug.Log(checkL[x] + "'s color is the same as: " + c.Key);
                    checkL.Remove(c.Key);
                }
            }
            x++;
        }
    }

}
