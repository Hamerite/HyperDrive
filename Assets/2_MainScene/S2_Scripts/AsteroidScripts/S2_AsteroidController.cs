//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using System.Collections.Generic;
using UnityEngine;

public class S2_AsteroidController : MonoBehaviour {
    [SerializeField] protected Transform[] asteroids;

    protected readonly List<Vector3> rotateDir =  new List<Vector3>();
    protected readonly List<float> rotateSpeed =  new List<float>();
    protected Quaternion startRotation = new Quaternion(0, 0, 0, 0);

    void Start() {
        for (int i = 0; i < asteroids.Length - 1; i++) {
            rotateDir.Add(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), 0));
            rotateSpeed.Add(Random.Range(0.001f, 0.003f));
        }

        rotateDir.TrimExcess();
        rotateSpeed.TrimExcess();
    }

    void Update() {
        if (!isActiveAndEnabled) return;
        
        transform.position = Vector3.MoveTowards(transform.position, DestroyerSingleton.Instance.gameObject.transform.position, 1.0f * S2_PoolController.Instance.GetSpeed() * Time.deltaTime);
        for (int i = 1; i < asteroids.Length - 1; i++) asteroids[i].Rotate(rotateDir[i] * rotateSpeed[i]);

        if (transform.position == DestroyerSingleton.Instance.gameObject.transform.position) {
            gameObject.transform.rotation = startRotation;
            gameObject.SetActive(false);

            if (!S2_PlayerCollisionController.Instance.GetIsAlive()) return;

            S2_HUDUI.Instance.SetObstacleCounter();
            S2_PoolController.Instance.CheckForBehaviourChange();
        }
    }
}