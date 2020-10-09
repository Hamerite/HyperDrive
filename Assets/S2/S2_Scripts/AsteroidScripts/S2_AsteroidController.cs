//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)

using System.Collections.Generic;
using UnityEngine;

public class S2_AsteroidController : MonoBehaviour
{
    Transform[] asteroids;
    GameObject destroyer;

    readonly List<Vector3> rotateDir =  new List<Vector3>();
    readonly List<float> rotateSpeed =  new List<float>();

    [SerializeField]
    int pointsPlanesOffset;

    void Awake()
    {
        asteroids = GetComponentsInChildren<Transform>();
        destroyer = GameObject.FindGameObjectWithTag("Destroyer");
    }

    void Start()
    {
        for (int i = 0; i < asteroids.Length - pointsPlanesOffset; i++)
        {
            rotateDir.Add(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), 0));
            rotateSpeed.Add(Random.Range(0.01f, 0.03f));
        }

        rotateDir.TrimExcess();
        rotateSpeed.TrimExcess();
    }

    void Update()
    {
        if (isActiveAndEnabled)
            transform.position = Vector3.MoveTowards(transform.position, destroyer.transform.position, 1.0f * S2_PoolController.Instance.GetSpeed() * Time.deltaTime);

        for (int i = 1; i < asteroids.Length - 4; i++)
             asteroids[i].Rotate(rotateDir[i] * rotateSpeed[i]);

        if (transform.position == destroyer.transform.position)
        {
            gameObject.SetActive(false);

            if(S2_PlayerCollisionController.Instance.GetIsAlive())
            {
                GameManager.Instance.SetNumbers("Counter", 0);
                S2_PoolController.Instance.CheckForBehaviourChange();
            }
        }
    }
}