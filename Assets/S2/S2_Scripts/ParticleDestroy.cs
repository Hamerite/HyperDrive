//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)

using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    [SerializeField] new ParticleSystem particleSystem;

    private void Update()
    {
        if (!particleSystem.IsAlive())
            Invoke(nameof(DelaySceneChange), 1.0f);
    }

    void DelaySceneChange()
    {
        GameManager.Instance.TraverseScenes(2, 3);
    }
}
