using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipDropDownSelector : MonoBehaviour
{
    public static ShipDropDownSelector Instance { private set; get; }
    [SerializeField] Text[] shipNames;
    [SerializeField] Button[] tierSelectors;
    [SerializeField] Button[] selectShipButtons;
    [SerializeField] Animator anim;
    [SerializeField] GameObject[] shipModels;
    [SerializeField] GameObject shipPanel;
    private int shipTier = 0;

    public Animator GetAnimator()
    {
        return anim;
    }
    public int ReturnTier()
    {
        return shipTier;
    }


    string[] shipNameStrings = { "MK-1 Space Ship", "Orcinus 2", "ENS Claire", "X Thrust", "Lumatlek", "GNBB-1K", "Deluge", "GRN Goblin", "Predator", "Tranquility", "UPC Endeavor", "Revelation", "Heron", "VAN", "Deleter" };

    private void Start()
    {
        Instance = this;
        shipPanel.SetActive(false);
    }
    public void SetTierOne()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Close"))
            anim.SetTrigger("Open");
        shipTier = 0;
        for (int i = 0; i < shipNames.Length; i++)
        {
            shipNames[i].text = shipNameStrings[i];
        }
        for (int j = 0; j < shipModels.Length; j++)
        {
            if (j < 5) shipModels[j].SetActive(true);
            else
                shipModels[j].SetActive(false);
        }
        HangarScript.Instance.GetShipDropDownPanel().SetActive(true);
        if(!shipPanel.activeInHierarchy)
            shipPanel.SetActive(true);

    }

    public void SetTierTwo()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Close"))
            anim.SetTrigger("Open");
        shipTier = 1;
        for (int i = 0; i < shipNames.Length; i++)
        {
            shipNames[i].text = shipNameStrings[i + 5];
        }
        for (int j = 0; j < shipModels.Length; j++)
        {
            if (j < 10 && j >= 5) shipModels[j].SetActive(true);
            else
                shipModels[j].SetActive(false);
        }
        HangarScript.Instance.GetShipDropDownPanel().SetActive(true);
        if (!shipPanel.activeInHierarchy)
            shipPanel.SetActive(true);
    }

    public void SetTierThree()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Close"))
            anim.SetTrigger("Open");
        shipTier = 2;
        for (int i = 0; i < shipNames.Length; i++)
        {
            shipNames[i].text = shipNameStrings[i + 10];
        }
        for (int j = 0; j < shipModels.Length; j++)
        {
            if (j < 15 && j >= 10) shipModels[j].SetActive(true);
            else
                shipModels[j].SetActive(false);
        }
        HangarScript.Instance.GetShipDropDownPanel().SetActive(true);
        if (!shipPanel.activeInHierarchy)
            shipPanel.SetActive(true);
    }
    public void ClosePanel()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Open"))
            anim.SetTrigger("Close");
        //HangarScript.Instance.GetShipDropDownPanel().SetActive(false);
    }

}
