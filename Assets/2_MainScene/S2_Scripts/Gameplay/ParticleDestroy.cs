//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)
using UnityEngine;

public class ParticleDestroy : MonoBehaviour{
    [SerializeField] protected new ParticleSystem particleSystem = null;

    void Update() {
        if (particleSystem.IsAlive()) return;
        Invoke(nameof(DelaySceneChange), 1.0f); 
    }

    void DelaySceneChange() { GameManager.Instance.TraverseScenes("Gameover"); }
}
