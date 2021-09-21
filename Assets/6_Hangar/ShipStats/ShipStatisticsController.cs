using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipStatisticsController : MonoBehaviour
{
    public static ShipStatisticsController Instance { get; private set; }

    [SerializeField] protected Slider[] statsSilders = null; // { Max Speed, FirePower, Shields, health, thrusters, regen }

    [SerializeField] Image[] sliderImages;
    private Color[] alphaChannel = new Color[14];
    public bool slideAlpha = false;
    private void Awake() { Instance = this; }
    public void StartGame()
    {
        GameManager.Instance.TraverseScenes("MainScene");
    }
    void Start()
    {
        for (int i = 0; i < sliderImages.Length; i++)
        {
            alphaChannel[i] = sliderImages[i].color;
        }
      
        if (!ShipStatistics.Instance) return;

        for (int i = 0; i < statsSilders.Length; i++) statsSilders[i].value = ShipStatistics.Instance.GetStats().GetAttributes()[i];
    }

    private void Update()
    {
        switch (slideAlpha)
        {
            case true:
                for (int i = 0; i < sliderImages.Length; i++)
                {
                    if (sliderImages[i].color.a >= 0)
                    {
                        alphaChannel[i].a -= 1 * Time.deltaTime;
                        sliderImages[i].color = alphaChannel[i];
                    }
                }
                break;
            case false:
                for (int i = 0; i < sliderImages.Length; i++)
                {
                    if (sliderImages[i].color.a <= 1)
                        {
                            alphaChannel[i].a += 1 * Time.deltaTime;
                            sliderImages[i].color = alphaChannel[i];
                        }
                }
                break;

        }
        
    }

    public void GetNewValues() { Invoke(nameof(Start), 0.1f); }
}
