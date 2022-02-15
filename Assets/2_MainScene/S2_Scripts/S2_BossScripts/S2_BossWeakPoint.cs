//created 21/09/21 (Alek Tepylo)
//Last modified 15/02/22 ~Dylan LeClair
using System.Collections;
using UnityEngine;

public class S2_BossWeakPoint : MonoBehaviour {
    [SerializeField] protected S2_BossBaseClass bossBody;
    [SerializeField] protected GameObject weakPoint_PS;
    [SerializeField] protected MeshRenderer rend;

    [SerializeField] protected Color emissionLow;
    [SerializeField] protected Color emissionHigh;

    [SerializeField] protected bool vulnerable, weakPoint;

    protected Color normalColor, vulnerableColor = new Color(255, 255, 255);

    protected bool increaseGlow;
    protected float t;

    public MeshRenderer GetMesh() { return rend; }

    public Color GetColor() { return normalColor; }

    void Start() { normalColor = rend.material.color; } //this is needed here for flashing 

    void OnTriggerEnter(Collider other) { if (other.gameObject.layer == 14 && vulnerable) { bossBody.TakeDamage(ShipStats.Instance.GetStats().GetAttributes()[0], weakPoint, this); } }    

    public void SetVulnerablility(bool b) {
        vulnerable = b;
        if (b) {
            if (weakPoint) {
                rend.material.SetColor("_Color", Color.white);
                weakPoint_PS.SetActive(true);
                StartCoroutine(Glow());
            }
        }
        else {
            if (weakPoint) {
                rend.material.SetColor("_Color", Color.black);
                rend.material.SetColor("_EmissionColor", emissionLow);
                weakPoint_PS.SetActive(false);
                t = 0;
                StopAllCoroutines();
            }
        }
    }

    IEnumerator Glow() {
        do {
            Color c;
            if (increaseGlow) {
                t += Time.deltaTime;
                c = Color.Lerp(rend.material.GetColor("_EmissionColor"), emissionHigh, t);
                if (t > 1) increaseGlow = false;
            }
            else {
                t -= Time.deltaTime;
                c = Color.Lerp(rend.material.GetColor("_EmissionColor"), emissionLow, t);
                if (t < 0) increaseGlow = true;
            }
            rend.material.SetColor("_EmissionColor", c);
            yield return null;
        }
        while (vulnerable);
    }
}


