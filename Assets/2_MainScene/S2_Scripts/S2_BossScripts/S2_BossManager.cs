//created 22/09/21 by AT
//Last modified 15/02/22 ~Dylan LeClair
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S2_BossManager : MonoBehaviour {
    public static S2_BossManager Instance { get; protected set;}
       
    [SerializeField] protected S2_BossBaseClass[] bossClasses;

    [SerializeField] protected GameObject bulletPooler, bossCanvas, warningObject, warningText;
    [SerializeField] protected GameObject[] bosses;

    [SerializeField] protected Text healthText, shieldTest;
    [SerializeField] protected Slider healthBar, shieldBar;

    [SerializeField] protected int bossNum;

    protected S2_BossBaseClass activeBoss;
    protected Coroutine loadBar;
    protected WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    protected int flashCounter;
    protected float t = 0, maxHealth, maxShields, targetHealthValue, targetShieldValue;

    public int GetBossNum(){ return bossNum; }

    void Awake() {
        Instance = this;
        bossNum = 0;
    }

    public void StartBoss(int i) {
        if (i < bosses.Length) { bossNum = i; }
        else { bossNum = bosses.Length - 1; }
            
        Invoke(nameof(BossWait), 0);
        bulletPooler.SetActive(true);
    }

    public void BossWait() {
        bosses[bossNum].SetActive(true);
        activeBoss = bossClasses[bossNum];//.GetComponentInChildren<S2_BossBaseClass>();
        activeBoss.ResetBoss();
        LoadHealthBars();
    }

    public void EndBoss() {
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

    public void RestartParticles() { S2_PoolController.Instance.StartParicles(); }

    public void RestartAsteroids() { S2_PoolController.Instance.StartUpAsteroids(); }
    
    public void LoadHealthBars() {
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

    IEnumerator FillHealthBars() {
        while(t < 1) {
            t += Time.deltaTime * 0.01f;
            healthBar.value += t;
            shieldBar.value += t;
            yield return endOfFrame;
        }

        t = 1;
        healthBar.value = 1;
        shieldBar.value = 1;
    }

    public void StopCoroutine() { StopCoroutine(loadBar); }

    public void UpdateHealthBars(float health, float shield) {
        healthBar.value = health / maxHealth;
        shieldBar.value = shield / maxShields;
    }

    public void UpdateText(float health, float shield) { //update this to healbars once assets come in
        healthText.text = health.ToString();
        shieldTest.text = shield.ToString();
    }

    public void DisplayWarning() {
        bossCanvas.SetActive(true);
        healthBar.gameObject.SetActive(false);
        shieldBar.gameObject.SetActive(false);
        warningObject.SetActive(true);
        warningText.SetActive(true);
        flashCounter = 0;
        InvokeRepeating(nameof(Flash), 0, 0.5f);
    }

    public void Flash() {
        if (flashCounter >= 7) {
            CancelInvoke(nameof(Flash));
            return;
        }

        flashCounter++;
        warningObject.SetActive(!warningObject.activeSelf);
        warningText.SetActive(!warningText.activeSelf);
    }
}
