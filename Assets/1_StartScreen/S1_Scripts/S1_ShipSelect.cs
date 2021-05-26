//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_ShipSelect : MonoBehaviour {
    [SerializeField] protected Button selectButton = null;
    [SerializeField] protected Text selectText = null;
    [SerializeField] protected Text price = null;
    [SerializeField] protected Text coinCount = null;

    [SerializeField] protected GameObject[] ships = null;

    protected Vector3 center;
    protected Vector3 rotaion = new Vector3(0, 70, 0);

    protected readonly bool[] wasPurchased = { true, false, false, false };
    protected readonly int[] prices = { 0, 10000, 10000, 10000 };

    protected int index;
    protected int playerCoins;
    protected int shipsLength;

    void Start() {
        SPD data = PDSM.LoadData();
        if(data != null) {
            for (int i = 1; i < wasPurchased.Length; i++) wasPurchased[i] = data.wasPurchased[i];
            index = data.shipSelected;
        }

        playerCoins = GameManager.Instance.GetNumbers("Coins");
        shipsLength = ships.Length - 1;
    }

    void OnEnable() {
        if(!Cursor.visible) selectButton.Select();
        MenusManager.Instance.SetSelectedButton(selectButton, null);

        ships[index].SetActive(true);
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);
        ships[index].transform.position = center;

        if (index == 0) price.text = "";
        coinCount.text = "Coins: " + playerCoins;
    }

    void OnDisable() { foreach (GameObject item in ships) if(item) item.SetActive(false); }

    void Update() { foreach (GameObject item in ships) item.transform.Rotate(rotaion * Time.deltaTime); }

    public void SelectButton() {
        if (wasPurchased[index] == true) {
            ships[index].SetActive(false);

            S1_ButtonsController.Instance.ChangePanels("HyperDrive", new bool[] { true, false, false, false, false });

            PDSM.SaveData(this);
            S1_ButtonsController.Instance.SetPanelChange();
        }
        else if (wasPurchased[index] == false && playerCoins >= prices[index]) {
            selectText.text = "Purchsed";
            price.text = "";

            wasPurchased[index] = true;
            PDSM.SaveData(this);

            playerCoins -= prices[index];
            GameManager.Instance.SetNumbers("Coins", playerCoins);

            coinCount.color = Color.red;
            coinCount.text = "Coins: " + playerCoins + " - " + prices[index];
            Invoke(nameof(Purchused), 0.8f);
        }
        else AudioManager.Instance.PlayButtonSound(4);
    }

    public void Previous() {
        ships[index].SetActive(false);

        index--;
        if (index < 0) index = shipsLength;

        GoToNextShip();
    }

    public void Next() {
        ships[index].SetActive(false);

        index++;
        if (index > shipsLength) index = 0;

        GoToNextShip();
    }

    void GoToNextShip() {
        AudioManager.Instance.PlayButtonSound(1);

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

    public bool[] GetWasPurchsed() { return wasPurchased; }

    public int GetIndex() { return index; }
}
