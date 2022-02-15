//Created by Dylan LeClair 11/08/21
//Last modified 11/08/21 (Dylan LeClair)
using UnityEngine;

public class SparksCleanup : MonoBehaviour {
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected new ParticleSystem particleSystem;

    void Start() {
        ParticleSystem.MainModule main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnEnable() { audioSource.Play(); }

    void OnParticleSystemStopped() { gameObject.SetActive(false); }
}
