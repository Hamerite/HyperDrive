//Created by Kyle Ennis 13/09/21
//Last modified 22/09/21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class ShipDropDownSelector : MonoBehaviour {
    public static ShipDropDownSelector Instance { private set; get; }

    [SerializeField] protected GameObject shipPanel;
    [SerializeField] protected GameObject[] shipModels;
    [SerializeField] protected GameObject[] staticObjects;

    [SerializeField] protected Text[] shipNames;
    [SerializeField] protected Button[] tierSelectors;

    [SerializeField] protected Animator anim;

    protected int shipTier = 0;
    protected string[] shipNameStrings = { "MK-1 Space Ship", "Orcinus 2", "ENS Claire", "X Thrust", "Lumatlek", 
                                            "GNBB-1K", "Deluge", "GRN Goblin", "Predator", "Tranquility", 
                                            "UPC Endeavor", "Revelation", "Heron", "VAN", "Deleter" };

    void Awake() { Instance = this; }

    public void SetTierOne() {
        AudioManager.Instance.PlayInteractionSound(1);
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Close")) anim.SetTrigger("Open");

        shipTier = 0;

        for (int i = 0; i < shipNames.Length; i++) shipNames[i].text = shipNameStrings[i];
        for (int j = 0; j < shipModels.Length; j++) {
            if (j < 5) shipModels[j].SetActive(true);
            else shipModels[j].SetActive(false);
        }

        S5_ShipSelect.Instance.GetShipDropDownPanel().SetActive(true);
        if(!shipPanel.activeInHierarchy) shipPanel.SetActive(true);
        StartStatic();
    }

    public void SetTierTwo() {
        AudioManager.Instance.PlayInteractionSound(1);
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Close")) anim.SetTrigger("Open");

        shipTier = 1;

        for (int i = 0; i < shipNames.Length; i++) shipNames[i].text = shipNameStrings[i + 5];
        for (int j = 0; j < shipModels.Length; j++) {
            if (j < 10 && j >= 5) shipModels[j].SetActive(true);
            else shipModels[j].SetActive(false);
        }

        S5_ShipSelect.Instance.GetShipDropDownPanel().SetActive(true);
        if (!shipPanel.activeInHierarchy) shipPanel.SetActive(true);
        StartStatic();
    }

    public void SetTierThree() {
        AudioManager.Instance.PlayInteractionSound(1);
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Close")) anim.SetTrigger("Open");

        shipTier = 2;

        for (int i = 0; i < shipNames.Length; i++) shipNames[i].text = shipNameStrings[i + 10];
        for (int j = 0; j < shipModels.Length; j++) {
            if (j < 15 && j >= 10) shipModels[j].SetActive(true);
            else shipModels[j].SetActive(false);
        }

        S5_ShipSelect.Instance.GetShipDropDownPanel().SetActive(true);
        if (!shipPanel.activeInHierarchy) shipPanel.SetActive(true);
        StartStatic();
    }

    public void ClosePanel() {
        AudioManager.Instance.PlayInteractionSound(1);
        anim.SetTrigger("Close");
        StartStatic();
    }

    public void StartStatic() {
        for (int i = 0; i < staticObjects.Length; i++) staticObjects[i].SetActive(true);
        Invoke(nameof(StopStatic), 0.5f);
    }

    void StopStatic() { for (int i = 0; i < staticObjects.Length; i++) staticObjects[i].SetActive(false); }

    public Animator GetAnimator() { return anim; }

    public int ReturnTier() { return shipTier; }
}
