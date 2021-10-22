using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossBulletPooler : MonoBehaviour
{
    public static S2_BossBulletPooler Instance { get; private set; }

    [SerializeField] protected GameObject lockOnPrefab = null; //each type of prefab will have mutiple textures and/or ps that will be chosen from when fired
    [SerializeField] protected List<GameObject> lockOnBullets = new List<GameObject>();

    [SerializeField] protected GameObject chasePrefab = null; 
    [SerializeField] protected List<GameObject> chaseBullets = new List<GameObject>();

    [SerializeField] protected GameObject disruptorPrefab = null;
    [SerializeField] protected List<GameObject> disruptorBullets = new List<GameObject>();

    [SerializeField] protected GameObject coveragePrefab = null;
    [SerializeField] protected List<GameObject> coverageBullets = new List<GameObject>();

    void Start() 
    {       
        Instance = this; 
        //for(int i =0; i < 40; i++)
        //{
        //    GameObject chaser = Instantiate(chasePrefab, transform);
        //    chaser.SetActive(false);
        //    chaseBullets.Add(chaser);            
        //}

        //for(int i = 0; i < 10; i++)
        //{
        //    GameObject homer = Instantiate(lockOnPrefab, transform);
        //    homer.SetActive(false);
        //    lockOnBullets.Add(homer);
        //}

        //for (int i = 0; i < 10; i++)
        //{
        //    GameObject disruptor = Instantiate(disruptorPrefab, transform);
        //    disruptor.SetActive(false);
        //    disruptorBullets.Add(disruptor);
        //}

        //for(int i = 0; i < 10; i++)
        //{
        //    GameObject coverage = Instantiate(coveragePrefab, transform);
        //    coverage.SetActive(false);
        //    coverageBullets.Add(coverage);
        //}
    }

    public GameObject GetLockOnBullet()
    {
        for (int i = 0; i < lockOnBullets.Count; i++)
        {
            if (!lockOnBullets[i].activeInHierarchy)
            {
                return lockOnBullets[i];
            }
        }

        GameObject homer = Instantiate(lockOnPrefab, transform);
        homer.SetActive(false);
        lockOnBullets.Add(homer);
        return homer;
    }

    public GameObject GetChaseBullet()
    {
        for (int i = 0; i < chaseBullets.Count; i++)
        {
            if (!chaseBullets[i].activeInHierarchy)
            {
                return chaseBullets[i];
            }
        }

        GameObject chaser = Instantiate(chasePrefab, transform);
        chaser.SetActive(false);
        chaseBullets.Add(chaser);      
        return chaser;
    }

    public GameObject GetDisruptorBullet()
    {
        for (int i = 0; i < disruptorBullets.Count; i++)
        {
            if (!disruptorBullets[i].activeInHierarchy)
            {
                return disruptorBullets[i];
            }
        }
        GameObject disruptor = Instantiate(disruptorPrefab, transform);
        disruptor.SetActive(false);
        disruptorBullets.Add(disruptor);
        return disruptor;
    }

    public GameObject GetCoverageBullet()
    {
        for (int i = 0; i < coverageBullets.Count; i++)
        {
            if (!coverageBullets[i].activeInHierarchy)
            {
                return coverageBullets[i];
            }
        }
        GameObject coverage = Instantiate(coveragePrefab, transform);
        coverage.SetActive(false);
        coverageBullets.Add(coverage);
        return null;
    }

    private void OnEnable()
    {
        foreach(GameObject bullet in coverageBullets)
        {
            bullet.SetActive(false);
        }
    }
}
