//Created by Dylan LeClair 09/08/21
//Last modified 09/08/21 (Dylan LeClair)
using System.Collections.Generic;
using UnityEngine;

public class S2_HitExplosionPooler : MonoBehaviour {
    public static S2_HitExplosionPooler Instance { get; private set; }

    [SerializeField] protected ParticleSystem explosionPrefab = null;
    [SerializeField] protected List<ParticleSystem> explosions = new List<ParticleSystem>();

    void Awake() { Instance = this; }

    public ParticleSystem GetHitExplosion(bool hitObstacle) {
        for (int i = 0; i < explosions.Count; i++) if (!explosions[i].gameObject.activeInHierarchy){
                if (hitObstacle) explosions[i].GetComponent<HitExplosionCleanup>().ToggleHitObstacle();
                return explosions[i];
            }


        ParticleSystem effect = Instantiate(explosionPrefab, transform);
        effect.gameObject.SetActive(false);
        explosions.Add(effect);
        return effect;
    }
}
