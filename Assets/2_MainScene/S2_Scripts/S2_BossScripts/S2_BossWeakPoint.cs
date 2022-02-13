//created 21/09/21 (Alek Tepylo)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossWeakPoint : MonoBehaviour
{
    [SerializeField]
    S2_BossBaseClass bossBody;


    [SerializeField]
    bool vulnerable;
    [SerializeField]
    bool weakPoint;
    public bool GetWeakPoint() { return weakPoint; }
    [SerializeField] GameObject weakPoint_PS;

    [SerializeField] MeshRenderer rend;
    public MeshRenderer GetMesh() { return rend; }
    private Color normalColor;
    public Color GetColor() { return normalColor; }
    [SerializeField] private Color emissionLow;
    [SerializeField] private Color emissionHigh;
    private bool glow;
    private bool increaseGlow;
    private float t;

    private Color vulnerableColor = new Color(255, 255, 255);
    public Color GetVulnerableColor() { return vulnerableColor; }

    private void Start()
    {
        normalColor = rend.material.color; //this is needed here for flashing 
    }

    private void Update()
    {
        if (glow)
        {
            Color c;
            if (increaseGlow)
            {
                t += Time.deltaTime;
                c = Color.Lerp(rend.material.GetColor("_EmissionColor"), emissionHigh, t);
                if (t > 1) increaseGlow = false;
            }
            else
            {
                t -= Time.deltaTime;
                c = Color.Lerp(rend.material.GetColor("_EmissionColor"), emissionLow, t);
                if (t < 0) increaseGlow = true;
            }
            rend.material.SetColor("_EmissionColor", c);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14 && vulnerable)
        {
            bossBody.TakeDamage(ShipStats.Instance.GetStats().GetAttributes()[0], weakPoint, this);
        }
    }    

    public void SetVulnerablility(bool b)
    {
        vulnerable = b;
        if (b)
        {
            if (weakPoint)
            {
                rend.material.SetColor("_Color", Color.white);
                weakPoint_PS.SetActive(true);
                //rend.material = vulnerableMat;
                glow = true;
            }
        }
        else
        {
            if (weakPoint)
            {
                rend.material.SetColor("_Color", Color.black);
                weakPoint_PS.SetActive(false);
                glow = false;
                t = 0;
                rend.material.SetColor("_EmissionColor", emissionLow);
                //rend.material = nonVulnerableMat;
            }
        }
    }
}


