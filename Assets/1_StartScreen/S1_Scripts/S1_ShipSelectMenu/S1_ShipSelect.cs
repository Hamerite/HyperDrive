//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_ShipSelect : MonoBehaviour {
    [SerializeField] protected Button selectButton = null;
    [SerializeField] protected Text selectText = null, price = null, coinCount = null;

    [SerializeField] protected GameObject[] ships = null;

    protected Vector3 center, rotaion = new Vector3(0, 70, 0);

    protected bool[] wasPurchased = { true, false, false, false, false,false,false,false };
    protected readonly int[] prices = { 0, 10000, 10000, 10000, 0,0,0,0 };

    protected int index, playerCoins;

    void Awake() {
        OnDisable();
        SPD data = PDSM.LoadData();
        if (data == null) return;

        wasPurchased = data.wasPurchased;
        index = data.shipSelected;

        //Validate data
        PDSM.DeleteData();
        data.wasPurchased = wasPurchased;
        data.shipSelected = index;
    }

    void OnEnable() {
        if(!Cursor.visible) selectButton.Select();
        MenusManager.Instance.SetSelectedButton(selectButton, null);

        ships[index].SetActive(true);
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);
        ships[index].transform.position = center;

        if (index == 0) price.text = "";

        playerCoins = GameManager.Instance.GetCoinAmount();
        coinCount.text = "Coins: " + playerCoins.ToString();
    }

    void OnDisable() {
        foreach (GameObject item in ships) {
            if (!item) return;
            item.SetActive(false); 
        }
    }

    void Update() { ships[index].transform.Rotate(rotaion * Time.deltaTime); }

    public void SelectButton() {
        if (wasPurchased[index] == true) {
            AudioManager.Instance.PlayInteractionSound(1);
            ships[index].SetActive(false);

            S1_ButtonsController.Instance.ChangePanels("HyperDrive", new bool[] { true, false, false, false, false });

            SavePurchaseData();
            S1_ButtonsController.Instance.SetPanelChange();
        }
        else if (wasPurchased[index] == false && playerCoins >= prices[index]) {
            AudioManager.Instance.PlayInteractionSound(6);
            selectText.text = "Purchsed";
            price.text = "";

            wasPurchased[index] = true;
            playerCoins -= prices[index];
            GameManager.Instance.SetCoinAmount(playerCoins);
            SavePurchaseData();

            coinCount.color = Color.red;
            coinCount.text = "Coins: " + playerCoins + " - " + prices[index];
            Invoke(nameof(Purchused), 0.8f);
        }
        else AudioManager.Instance.PlayInteractionSound(4);
    }

    public void Previous() {
        ships[index].SetActive(false);

        index--;
        if (index < 0) index = ships.Length - 1;

        GoToNextShip();
    }

    public void Next() {
        ships[index].SetActive(false);

        index++;
        if (index > ships.Length - 1) index = 0;

        GoToNextShip();
    }

    void GoToNextShip() {
        S1_ShipSelectController.Instance.GetNewValues();
        MenusManager.Instance.SetSelectedButton(null, null);
        AudioManager.Instance.PlayInteractionSound(1);

        if (wasPurchased[index] == false) {
            selectText.text = "BUY";
            price.text = "Costs " + prices[index] + " Coins";
        }
        else {
            selectText.text = "SELECT";
            price.text = "";
        }

        ships[index].SetActive(true);
        ships[index].transform.position = center;
    }

    void Purchused() {
        coinCount.color = Color.yellow;
        coinCount.text = "Coins: " + playerCoins;
    }

    void SavePurchaseData() { PDSM.SaveData(this); }

    public bool[] GetWasPurchsed() { return wasPurchased; }

    public int GetIndex() { return index; }
}