using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    [Header("Country UI")]
    public GameObject countryMenu;
    public GameObject countryName;
    public GameObject countryIdeology;
    public GameObject countryLeaning;

    [Header("State UI")]
    public GameObject stateMenu;
    public GameObject stateName;
    public GameObject stateOwner;

    public void stateMenuShow(GameObject state) 
    {
        stateMenu.SetActive(true);
        stateName.GetComponent<Text>().text = state.GetComponent<state>().name;
        stateOwner.GetComponent<Text>().text = state.GetComponent<state>().owner.GetComponent<country>().name;

    }

    public void countryMenuShow(GameObject country)
    {
        countryMenu.SetActive(true);
        countryName.GetComponent<Text>().text = country.GetComponent<country>().name;
        if (country.GetComponent<country>().ideology != null) { countryIdeology.GetComponent<Text>().text = "Ideology: " + country.GetComponent<country>().ideology.name; }
        if (country.GetComponent<country>().leaning != null) { countryLeaning.GetComponent<Text>().text = "Leaning: " + country.GetComponent<country>().leaning; }

    }
}
