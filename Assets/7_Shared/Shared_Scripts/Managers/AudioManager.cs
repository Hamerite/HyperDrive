//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    [SerializeField] protected AudioSource[] audioSource; // { Music, SFX, MenuSounds }
    [SerializeField] protected AudioMixer audioMixer;
    [SerializeField] protected AudioClip[] interactionSoundClips; // { MousedOver, Pressed, saveHighScore, deleteHighScore, DenySelection, Coins, Purchased, GoBack }

    public AudioSource[] GetAudioSources() { return audioSource; }

    public bool[] Mutes { get; protected set; }
    public float[] Volumes { get; protected set; }

    void Awake() { 
        Instance = this;

        Mutes = new bool[2];
        Volumes = new float[3] { .5f, .5f, .5f };
        SAD data = ADSM.LoadData();
        if (data == null) return;

        Mutes = data.mutes;
        Volumes = data.volumes;
    }

    void Start() {
        AudioListener.pause = Mutes[0];
        audioMixer.SetFloat("MixerMenu", Mutes[1] ? 0 : 1);

        audioMixer.SetFloat("MixerMaster", Mathf.Log10(Volumes[0] > 0 ? Volumes[0] : 0.5f) * 20);
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(Volumes[1] > 0 ? Volumes[1] : 0.5f) * 20);
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(Volumes[2] > 0 ? Volumes[2] : 0.5f) * 20);
    }

    public void SetMutes(string key, bool status, int index) {
        Mutes[index] = status;

        if(key == null) { AudioListener.pause = status; }
        else { audioMixer.SetFloat(key, status ? 0 : 1); }
    }

    public void SetVolume(string key, float value, int index) {
        Volumes[index] = value;
        audioMixer.SetFloat(key, Mathf.Log10(value) * 20);
    }

    public void SaveAudioSettings() { ADSM.SaveData(this); }

    public void PlayInteractionSound(int index) { audioSource[2].PlayOneShot(interactionSoundClips[index]); }

    public void ToggleLoopingAudio(bool status, int index) {
        audioSource[1].loop = status;
        if (status) {
            audioSource[1].clip = interactionSoundClips[index];
            audioSource[1].Play();
        }
        else {
            audioSource[1].Stop();
            audioSource[1].clip = null;
        }
    }
}
