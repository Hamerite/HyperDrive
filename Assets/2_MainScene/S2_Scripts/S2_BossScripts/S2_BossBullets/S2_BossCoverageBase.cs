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

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject w in waves)
        {
            w.SetActive(false);
        }
    }

    private void OnEnable()
    {
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
        activeWave.SetActive(false);
        gameObject.SetActive(false);
    }

    
}
