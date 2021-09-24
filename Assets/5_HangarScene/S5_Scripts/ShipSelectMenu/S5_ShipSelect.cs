//Created by Dylan LeClair
//Last modified 22-09-21 (Kyle Ennis)
using UnityEngine;
using UnityEngine.UI;

public class S5_ShipSelect : MonoBehaviour {
    public static S5_ShipSelect Instance { get; private set; }

    [SerializeField] protected Text selectText = null, price = null, coinCount = null, shipInformation = null;

    [SerializeField] protected GameObject[] ships = null;
    [SerializeField] protected GameObject shipDropDownPanel, shipInformationPanel;

    protected bool[] wasPurchased = { true, false, false, false, false,
                                        false, false, false, false, false,
                                        false, false, false, false, false };
    protected readonly int[] prices = { 0, 10000, 10000, 10000, 10000,
                                        20000, 20000, 20000, 20000, 20000,
                                        30000, 30000, 30000, 30000, 30000 };

    protected int index, playerCoins;

    void Awake() {
        Instance = this;

        SPD data = PDSM.LoadData();
        if (data == null) return;

        wasPurchased = data.wasPurchased;
        index = data.shipSelected;
    }

    void OnEnable() {
        ships[index].SetActive(true);

        if (index == 0) price.text = "";

        playerCoins = GameManager.Instance.GetCoinAmount();
        coinCount.text = "Coins: " + playerCoins.ToString();

        S5_ButtonsController.Instance.Invoke(nameof(S5_ButtonsController.Instance.SetButtonsInteractable), 3);
    }

    public void GoToNextShip(int direction) {
        index += direction;
        if (index < 0) index = ships.Length - 1;
        else if (index > ships.Length - 1) index = 0;

        S5_ShipSelectController.Instance.GetNewValues();
        AudioManager.Instance.PlayInteractionSound(1);

        if (wasPurchased[index] == false) {
            selectText.text = "BUY";
            price.text = "Costs " + prices[index] + " Coins";
        }
        else {
            selectText.text = "START GAME";
            price.text = "";
        }

        SwitchShipModel();
    }

    void SwitchShipModel() {
        ShipDropDownSelector.Instance.GetAnimator().SetTrigger("Close");
        S5_AnimationsController.Instance.ToggleUIAnimation();

        if (S5_AnimationsController.Instance.GetShipAnimationState() == "shipIdle") {
            S5_AnimationsController.Instance.PlayShipAnimationOne();
            S5_ButtonsController.Instance.Invoke(nameof(S5_ButtonsController.Instance.SetButtonsInteractable), 3);

            for (int i = 0; i < ships.Length; i++) {
                if (i == index) ships[i].SetActive(true);
                else if (ships[i].activeInHierarchy) ships[i].SetActive(false);
            }

            S5_AnimationsController.Instance.ToggleUIAnimation();
        }
        else {
            S5_AnimationsController.Instance.PlayShipAnimationTwo();
            S5_ButtonsController.Instance.SetButtonsInteractable();
            Invoke(nameof(SwitchShipModel), 5.5f);
        }
    }

    public void PurchaseShip() {
        selectText.text = "START GAME";
        price.text = "";

        wasPurchased[index] = true;
        playerCoins -= prices[index];
        GameManager.Instance.SetCoinAmount(playerCoins);
        SavePurchaseData();

        coinCount.color = Color.red;
        coinCount.text = "Coins: " + playerCoins + " - " + prices[index];
        Invoke(nameof(Purchased), 0.8f);
    }

    void Purchased() {
        coinCount.color = Color.yellow;
        coinCount.text = "Coins: " + playerCoins;
    }

    public void SavePurchaseData() { PDSM.SaveData(this); }

    public void SetShipInformation(string x) { shipInformation.text = x; }

    public void SetInformationPanel(bool state) { shipInformationPanel.SetActive(state); }

    public void SetIndex(int value) { index = value; }

    public GameObject GetShipDropDownPanel() { return shipDropDownPanel; }

    public GameObject[] GetShips() { return ships; }

    public bool[] GetWasPurchsed() { return wasPurchased; }

    public bool CheckPlayerWealth() { return playerCoins >= prices[index]; }

    public bool GetInformationPanelState() { return shipInformationPanel.activeInHierarchy; }

    public int GetIndex() { return index; }

    public bool GetCurrentMenuShip() { return wasPurchased[index]; }
}
