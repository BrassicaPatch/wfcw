using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stateUI : MonoBehaviour
{
    [SerializeField] private GameObject GMO;
    private GameManager GM;
    private country playerC;
    private state stateSel;

    [Header("State UI")]
    public GameObject stateMenu;
    public GameObject stateName;
    public GameObject stateOwner;
    public GameObject stateOcc;
    public GameObject stateRes;

    [Space(10)]
    public GameObject actionButtonContainer;

    [Header("Prefabs")]
    public GameObject flrPrefab;

    //Istantiated Objects
    private GameObject flr;

    public void Update()
    {
        stateStats();
        actionMenu();
    }

    public void getInf()
    {
        GM = GMO.GetComponent<GameManager>();
        playerC = GM.player.GetComponent<country>();
    }

    public void stateMenuShow(GameObject state)
    {
        stateMenu.SetActive(true);
        stateSel = state.GetComponent<state>();
        stateName.GetComponent<Text>().text = stateSel.name;
        stateOwner.GetComponent<Text>().text = stateSel.owner.GetComponent<country>().name;
    }

    public void stateStats()
    {
        stateOcc.GetComponent<Text>().text = "Local Resistance: " + stateSel.occupyCost;
    }

    public void actionMenu()
    {
        if (playerC.money >= 5)
        {
            flr = Instantiate(flrPrefab);
            flr.transform.SetParent(actionButtonContainer.gameObject.transform);
        }
    }

    public void fundLocalResistance()
    {
        playerC.money -= 5;
        stateSel.occupyCost += 1;
    }




    public void exit()
    {
        Destroy(flr);
    }
}
