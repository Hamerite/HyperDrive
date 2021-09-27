using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossDisruptorBase : MonoBehaviour
{
    [SerializeField]
    float timeOnScreen;
    [SerializeField]
    float warningTime;

    [SerializeField]
    GameObject damageObject;
    [SerializeField]
    GameObject warningObject;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ShowWarning();
        Invoke(nameof(ActivateBullets), warningTime);
        Invoke(nameof(DeactivateBullets), warningTime + timeOnScreen);
    }


    public void ShowWarning()
    {
        warningObject.SetActive(true);
        damageObject.SetActive(false);
    }

    public void ActivateBullets()
    {
        warningObject.SetActive(false);
        damageObject.SetActive(true);
    }

    public void DeactivateBullets()
    {
        gameObject.SetActive(false);
    }

}
