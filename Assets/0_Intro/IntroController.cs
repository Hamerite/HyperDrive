//Created by Dylan LeClair
//Last revised 23-08-21 (Dylan LeClair)
using System.IO;
using UnityEngine;

public class IntroController : MonoBehaviour {
    [SerializeField] protected AudioClip introAudio = null;
    [SerializeField] protected CanvasGroup canvasGroup = null;

    void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "HDSD"));
        print(Application.persistentDataPath);
    }

    void Start() {
        canvasGroup.LeanAlpha(1.0f, 7.0f).setOnComplete(() => GameManager.Instance.TraverseScenes("StartScreen"));
        Invoke(nameof(StartAudio), 0.5f);
    }

    void Update() { if (Input.anyKeyDown) GameManager.Instance.TraverseScenes("StartScreen"); }

    void StartAudio() { AudioManager.Instance.GetAudioSources()[2].PlayOneShot(introAudio); }
}
