//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource[] audioSource = null; // { Music, SFX, MenuSounds }
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] AudioClip[] buttonSoundClips = null; // { MousedOver, Pressed, saveHighScore, deleteHighScore, DenySelection, Coins }

    bool[] mutes = { false, false }; // { All, Menues }
    float[] volumes = { 0, 0, 0 }; // { Master, Music, SFX }

    void Awake() { 
        Instance = this;

        SAD data = ADSM.LoadData();
        if (data != null)
        {
            mutes = data.mutes;
            volumes = data.volumes;
        }
    }

    void Start() {
        AudioListener.pause = mutes[0];
        if (mutes[1]) audioMixer.SetFloat("MixerMenu", 0);

        audioMixer.SetFloat("MixerMaster", Mathf.Log10(volumes[0]) * 20);
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(volumes[1]) * 20);
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(volumes[2]) * 20);
    }

    public void SetMute(bool value) {
        AudioListener.pause = value;
        mutes[0] = value;
        ADSM.SaveData(this);
    }

    public void SetMenuMute(bool value){
        if(value) audioMixer.SetFloat("MixerMenu", 0);
        else audioMixer.SetFloat("MixerMenu", 1);

        mutes[1] = value;
        ADSM.SaveData(this);
    }

    public void SetMasterVolume(float value) {
        audioMixer.SetFloat("MixerMaster", Mathf.Log10(value) * 20);
        volumes[0] = value;
        ADSM.SaveData(this);
    }

    public void SetMusicVolume(float value) {
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(value) * 20);
        volumes[1] = value;
        ADSM.SaveData(this);
    }

    public void SetSFXVolume(float value) {
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(value) * 20);
        volumes[2] = value;
        ADSM.SaveData(this);
    }

    public void PlayButtonSound(int index) { audioSource[2].PlayOneShot(buttonSoundClips[index]); }

    public void PlayLoopingAudio(int index) {
        audioSource[1].loop = true;
        audioSource[1].clip = buttonSoundClips[index];
        audioSource[1].Play();
    }

    public void StopLoopingAudio() {
        audioSource[1].loop = false;
        audioSource[1].clip = null;
        audioSource[1].Stop();
    }

    public AudioSource[] GetAudioSources() { return audioSource; }

    public bool[] GetMutes() { return mutes; }

    public float[] GetVolumes() { return volumes; }
}
