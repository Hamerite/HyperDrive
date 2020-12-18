//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class S1_ShipSelect : MonoBehaviour
{
    [SerializeField] Button selectButton = null;
    [SerializeField] Text selectText = null;
    [SerializeField] Text price = null;
    [SerializeField] Text coinCount = null;

    [SerializeField] GameObject[] ships = null;

    Vector3 center;
    Vector3 rotaion = new Vector3(0, 70, 0);

    readonly bool[] wasPurchased = { true, false, false, false };
    readonly int[] prices = { 0, 10000, 10000, 10000 };

    int index;
    int playerCoins;
    int shipsLength;

    void Start()
    {
        wasPurchased[1] = (PlayerPrefs.GetInt("SharkPurchased") != 0);
        wasPurchased[2] = (PlayerPrefs.GetInt("BattlePurchased") != 0);
        wasPurchased[3] = (PlayerPrefs.GetInt("XPurchased") != 0);

        shipsLength = ships.Length - 1;
    }

    void OnEnable()
    {
        if(!Cursor.visible)
            selectButton.Select();
        MenusManager.Instance.SetSelectedButton(selectButton, null);

        ships[PlayerPrefs.GetInt("Selection")].SetActive(true);
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);
        ships[PlayerPrefs.GetInt("Selection")].transform.position = center;
        index = PlayerPrefs.GetInt("Selection");

        if (index == 0)
            price.text = "";

        playerCoins = GameManager.Instance.GetNumbers("Coins");
        coinCount.text = "Coins: " + playerCoins;
    }

    private void OnDisable()
    {
        foreach (GameObject item in ships)
            if(item)
                item.SetActive(false);
    }

    void Update()
    {
        foreach (GameObject item in ships)
            item.transform.Rotate(rotaion * Time.deltaTime);
    }

    public void SelectButton()
    {
        if (wasPurchased[index] == true)
        {
            ships[index].SetActive(false);

            S1_ButtonsController.Instance.ChangePanels("HyperDrive", true, false, false, false, false);

            PlayerPrefs.SetInt("Selection", index);
            PlayerPrefs.Save();

            S1_ButtonsController.Instance.SetPanelChange();
        }
        else if (wasPurchased[index] == false && playerCoins >= prices[index])
        {
            selectText.text = "Purchsed";
            price.text = "";

            wasPurchased[index] = true;
            coinCount.color = Color.red;
            coinCount.text = "Coins: " + playerCoins + " - " + prices[index];
            Invoke(nameof(Purchused), 0.8f);
            SavePurchase();
        }
        else
        {
            AudioManager.Instance.PlayButtonSound(4);
        }
    }

    public void Previous()
    {
        ships[index].SetActive(false);

        index--;
        if (index < 0)
            index = shipsLength;

        GoToNextShip();
    }

    public void Next()
    {
        ships[index].SetActive(false);

        index++;
        if (index > shipsLength)
            index = 0;

        GoToNextShip();
    }
    void GoToNextShip()
    {
        AudioManager.Instance.PlayButtonSound(1);

        if (wasPurchased[index] == false)
        {
            selectText.text = "BUY";
            price.text = "Costs " + prices[index] + " Coins";
        }
        else
        {
            selectText.text = "SELECT";
            price.text = "";
        }

        ships[index].SetActive(true);
        ships[index].transform.position = center;
    }

    void SavePurchase()
    {
        if (index == 1)
            PlayerPrefs.SetInt("SharkPurchased", (wasPurchased[index] ? 1 : 0));
        if (index == 2)
            PlayerPrefs.SetInt("BattlePurchased", (wasPurchased[index] ? 1 : 0));
        if (index == 3)
            PlayerPrefs.SetInt("XPurchased", (wasPurchased[index] ? 1 : 0));

        playerCoins -= prices[index];
        GameManager.Instance.SetNumbers("Coins", playerCoins);
        PlayerPrefs.SetInt("Coins", playerCoins);
        PlayerPrefs.Save();
    }

    void Purchused()
    {
        coinCount.color = Color.yellow;
        coinCount.text = "Coins: " + playerCoins;
    }
}
