using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarAudioManager : MonoBehaviour
{
    public static HangarAudioManager Instance { private get; set; }

    [SerializeField] AudioClip[] interactSounds;
    [SerializeField] AudioSource source;
    private void Start()
    {
        Instance = this;
    }

    public void PlayOneShotByIndex(int x)
    {
        source.PlayOneShot(interactSounds[x]);
    }
}
