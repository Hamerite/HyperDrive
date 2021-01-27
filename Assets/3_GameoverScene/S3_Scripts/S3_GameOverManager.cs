//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class S3_GameOverManager : MonoBehaviour
{
    public static S3_GameOverManager Instance { get; private set; }

    [SerializeField] GameObject[] textElements = null;
    [SerializeField] Text coinsGain = null;

    bool startAdd;
    float preAddedCoins;
    float addedCoins;
    float gameTime;
    float addRate = 300.0f;
    string champName;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.GetString("ChampName") == null)
            champName = "";
        else
            champName = PlayerPrefs.GetString("ChampName");

        textElements[0].GetComponent<Text>().text = "Score: " + GameManager.Instance.GetNumbers("Score").ToString();
        textElements[2].GetComponent<Text>().text = "High Score: " + champName + " : " + PlayerPrefs.GetInt("HighScore").ToString();

        if (PlayerPrefs.GetInt("HighScore") < GameManager.Instance.GetNumbers("Score"))
        {
            PlayerPrefs.SetInt("HighScore", GameManager.Instance.GetNumbers("Score"));

            textElements[1].SetActive(true);
            InvokeRepeating(nameof(FlashingLetters), 0.15f, 0.15f);
            S3_ButtonsController.Instance.ButtonsInteractability();
            S3_ButtonsController.Instance.HighScoreAcheived();
        }

        preAddedCoins = GameManager.Instance.GetNumbers("Coins");
        coinsGain.text = "Coins: " + preAddedCoins;
        GameManager.Instance.SetNumbers("Coins", 0);
        addedCoins = GameManager.Instance.GetNumbers("Coins") - preAddedCoins;

        gameTime = Time.time;

        Invoke(nameof(AddCoins), 0.7f);
    }

    private void Update()
    {
        if (startAdd)
        {
            AudioManager.Instance.PlayLoopingAudio(5);

            if (Time.time > gameTime + 1.0f)
            {
                gameTime = Time.time;
                addRate += 25.0f;
            }

            if (preAddedCoins < GameManager.Instance.GetNumbers("Coins"))
                preAddedCoins += Mathf.Floor(addRate * Time.deltaTime);      

            if (addedCoins > 0)
            {
                addedCoins -= Mathf.Floor(addRate * Time.deltaTime);
                coinsGain.text = "Coins: " + preAddedCoins + " + " + addedCoins;
            }
            else
            {
                coinsGain.text = "Coins: " + preAddedCoins;
                AudioManager.Instance.StopLoopingAudio();
                startAdd = false;
            }
        }
    }

    void AddCoins()
    {
        if (addedCoins > 0)
            coinsGain.text = "Coins: " + preAddedCoins + " + " + addedCoins;
        else
            coinsGain.text = "Coins: " + preAddedCoins;

        startAdd = true;
    }

    void FlashingLetters()
    {
        if(textElements[1].GetComponent<Text>().color == Color.red)
            textElements[1].GetComponent<Text>().color = Color.white;
        else
            textElements[1].GetComponent<Text>().color = Color.red;
    }

    public void SetChampName(string name)
    {
        textElements[1].SetActive(false);
        textElements[2].GetComponent<Text>().text = "High Score: " + name + "  " + PlayerPrefs.GetInt("HighScore").ToString();
    }
}