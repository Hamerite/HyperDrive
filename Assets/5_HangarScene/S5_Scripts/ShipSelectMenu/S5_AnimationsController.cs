//Created by Dylan LeClair 23/09/21
//Last modified 23/09/21 (Dylan LeClair)
using UnityEngine;

public class S5_AnimationsController : MonoBehaviour {
    public static S5_AnimationsController Instance { get; private set; }

    [SerializeField] protected GameObject[] hangarAssets = null;
    [SerializeField] protected GameObject redLight = null;

    [SerializeField] protected Light hangarLight = null;
    [SerializeField] protected Animator shipAnim, hangarDoorAnim, UIAnim;

    protected bool flicker;
    protected int hangarIndex;

    void Awake() { Instance = this; }

    void OnEnable() {
        PlayShipAnimationOne();

        InvokeRepeating(nameof(ChangeHangarLight), 0.3f, 0.3f);
        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close")) UIAnim.SetTrigger("open");

        StopFlicker();
    }

    void Update() {
        if (!flicker) return;
        hangarLight.intensity = Mathf.PingPong(Time.time * Random.Range(1, 1.5f), Random.Range(15, 15.1f)) + Random.Range(14.9f, 15);
    }

    void FlickerLight() { 
        flicker = true;
        Invoke(nameof(StopFlicker), Random.Range(0.25f, 1));
    }

    void StopFlicker() { 
        flicker = false;
        Invoke(nameof(FlickerLight), Random.Range(2, 5));
    }

    public void PlayShipAnimationOne() {
        PlayHangarDoorAnimation();
        shipAnim.SetTrigger("ShipIn");
        Invoke(nameof(TurnOffWarningLight), 3);
    }

    void TurnOffWarningLight() { redLight.SetActive(false); }

    public void PlayShipAnimationTwo() {
        redLight.SetActive(true);
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter")) {
            Invoke(nameof(PlayHangarDoorAnimation), 3.3f);
            shipAnim.SetTrigger("ShipOut");
        }
    }

    void PlayHangarDoorAnimation() { hangarDoorAnim.SetTrigger("triggerDoor"); }

    void ChangeHangarLight() {
        hangarIndex++;

        if (hangarIndex > hangarAssets.Length - 1) hangarIndex = 0;
        for (int i = 0; i < hangarAssets.Length; i++) {
            if (i == hangarIndex) hangarAssets[i].SetActive(true);
            else hangarAssets[i].SetActive(false);
        }
    }

    public void ToggleUIAnimation() {
        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("open")) UIAnim.SetTrigger("close");
        else if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close")) UIAnim.SetTrigger("open");
    }

    public string GetShipAnimationState() {
        string state = null;

        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle")) state = "shipIdle";
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter")) state = "ShipEnter";
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipExit")) state = "shipExit";

        return state;
    }
}
