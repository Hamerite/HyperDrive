using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_BossManager : MonoBehaviour
{
    public static S2_BossManager Instance { get; private set;}
       
    [SerializeField]
    GameObject[] bosses;
    [SerializeField] S2_BossBaseClass[] bossClasses;
    S2_BossBaseClass activeBoss;
    [SerializeField] int bossNum;
    public int GetBossNum(){ return bossNum; }

    [SerializeField] GameObject bulletPooler;

    //these will be bars for the furture
    [SerializeField]
    GameObject bossCanvas;
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text shieldTest;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider shieldBar;
    [SerializeField] GameObject warningObject;
    [SerializeField] GameObject warningText;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        bossNum = 0;
        bossCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void StartBoss(int i)
    {
        if (i < bosses.Length)
            bossNum = i;
        else
            bossNum = bosses.Length - 1;
        //S2_PoolController.Instance.StopAsteroids();
        Invoke(nameof(BossWait), 0);
        bulletPooler.SetActive(true);
    }

    public void BossWait()
    {
        bosses[bossNum].SetActive(true);
        activeBoss = bossClasses[bossNum];//.GetComponentInChildren<S2_BossBaseClass>();
        activeBoss.ResetBoss();
        //bossCanvas.SetActive(true);
        LoadHealthBars();
    }

    public void EndBoss()
    {
        //play destruction anims here
        activeBoss.AddScore();
        bosses[bossNum].SetActive(false);
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        bossCanvas.SetActive(false);
        bulletPooler.SetActive(false);
        Invoke(nameof(RestartParticles), 3);
        Invoke(nameof(RestartAsteroids), 5);
    }

    public void RestartParticles()
    {
        S2_PoolController.Instance.StartParicles();
    }

    public void RestartAsteroids()
    {
        S2_PoolController.Instance.StartUpAsteroids();
    }

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    Coroutine loadBar;
    float t = 0;
    float maxHealth;
    float maxShields;
    float targetHealthValue;
    float targetShieldValue;
    
    public void LoadHealthBars()
    {
        warningText.SetActive(false);
        warningObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        shieldBar.gameObject.SetActive(true);
        healthBar.value = 0;
        shieldBar.value = 0;
        t = 0;
        maxHealth = activeBoss.GetMaxHealth();
        maxShields = activeBoss.GetMaxShields();
        loadBar = StartCoroutine(FillHealthBars());
        Invoke(nameof(StopCoroutine), 3.5f);
    }

    IEnumerator FillHealthBars()
    {
        while(t < 1)
        {
            t += Time.deltaTime * 0.01f;
            healthBar.value += t;
            shieldBar.value += t;
            yield return endOfFrame;
        }

        t = 1;
        healthBar.value = 1;
        shieldBar.value = 1;
    }

    public void StopCoroutine()
    {
        StopCoroutine(loadBar);
    }

    public void UpdateHealthBars(float health, float shield)
    {
        healthBar.value = health / maxHealth;
        shieldBar.value = shield / maxShields;
    }

    public void UpdateText(float health, float shield) //update this to healbars once assets come in
    {
        healthText.text = health.ToString();
        shieldTest.text = shield.ToString();
    }

    public void DisplayWarning()
    {
        bossCanvas.SetActive(true);
        healthBar.gameObject.SetActive(false);
        shieldBar.gameObject.SetActive(false);
        warningObject.SetActive(true);
        warningText.SetActive(true);
        flashCounter = 0;
        InvokeRepeating(nameof(Flash), 0, 0.5f);
    }

    int flashCounter;

    public void Flash()
    {
        if (flashCounter >= 7)
        {
            CancelInvoke(nameof(Flash));
            return;
        }

        flashCounter++;
        warningObject.SetActive(!warningObject.activeSelf);
        warningText.SetActive(!warningText.activeSelf);
    }  

}
