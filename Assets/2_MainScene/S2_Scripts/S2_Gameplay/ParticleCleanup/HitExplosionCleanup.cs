//Created by Dylan LeClair 09/08/21
//Last modified 09/08/21 (Dylan LeClair)
using UnityEngine;

public class HitExplosionCleanup : MonoBehaviour {
    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected new ParticleSystem particleSystem = null;

    protected bool hitObstacle;

    void Start() {
        ParticleSystem.MainModule main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnEnable() { audioSource.Play(); }

    void Update() { 
        if (!hitObstacle) return; 
        transform.Translate(Vector3.back * S2_PoolController.Instance.GetSpeed() * Time.deltaTime); 
    }

    void OnParticleSystemStopped() { gameObject.SetActive(false); }

    public void ToggleHitObstacle() { hitObstacle = !hitObstacle; }
}
