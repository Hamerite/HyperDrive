using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossCoverageBase : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    GameObject[] waves;

    GameObject activeWave;   

    private void OnEnable()
    {
        for(int i = 0; i < waves.Length; i++)
        { waves[i].SetActive(false); }

        if (activeWave)
            activeWave.SetActive(false);

        Invoke(nameof(Deactivate), 3);
        int r = Random.Range(0, waves.Length);       
        activeWave = waves[r];
        activeWave.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void Deactivate()
    {     
        if(gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }

    
}
