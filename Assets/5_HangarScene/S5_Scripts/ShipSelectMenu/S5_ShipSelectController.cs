//Created by Dylan LeClair 24/06/21
//Last Modified 22/09/21 (Kyle Ennis)
using UnityEngine;
using UnityEngine.UI;

public class S5_ShipSelectController : MonoBehaviour {
    public static S5_ShipSelectController Instance { get; private set; }

    [SerializeField] protected Slider[] statsSilders = null; // { Speed, FirePower, Shields, health, thrusters, regen }
    [SerializeField] protected Image[] sliderImages;

    protected Color[] alphaChannel = new Color[14];
    protected bool slideAlpha;

    void Awake() { Instance = this; }

    void Start() {
        for (int i = 0; i < sliderImages.Length; i++) alphaChannel[i] = sliderImages[i].color;
        if (!ShipStats.Instance) return;

        statsSilders[0].value = ShipStats.Instance.GetStats().GetMaxSpeed();
        for (int i = 1; i < statsSilders.Length; i++) statsSilders[i].value = ShipStats.Instance.GetStats().GetAttributes()[i - 1];
    }

    void Update() {
        if (slideAlpha) {
            for (int i = 0; i < sliderImages.Length; i++) {
                if (sliderImages[i].color.a >= 0) {
                    alphaChannel[i].a -= 1 * Time.deltaTime;
                    sliderImages[i].color = alphaChannel[i];
                }
            }
        }
        else {
            for (int i = 0; i < sliderImages.Length; i++) {
                if (sliderImages[i].color.a <= 1) {
                    alphaChannel[i].a += 1 * Time.deltaTime;
                    sliderImages[i].color = alphaChannel[i];
                }
            }
        }
    }

    public void GetNewValues() { Invoke(nameof(Start), 0.1f); }

    public void SetSlideAlpha(bool state) { slideAlpha = state; }
}
