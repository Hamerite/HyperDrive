//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerController : MonoBehaviour {
    public static S2_PlayerController Instance { get; protected set; }

    [SerializeField] protected GameObject[] shipChoice;

    protected int index;

    void Awake() { Instance = this; }

    void Start() {
        SPD data = PDSM.LoadData();
        if (data != null) index = data.shipSelected;

        shipChoice[index].SetActive(true);

        S2_PlayerAnimationController.Instance.enabled = true;
    }

    void FixedUpdate() {
        if (!S2_ShootingController.Instance.IntroFinished || !ShipStats.Instance || S2_PlayerAnimationController.Instance.IsRolling) return;

        float vertical = Input.GetAxis("Vertical") * ShipStats.Instance.GetStats().GetMaxSpeed() * Time.fixedDeltaTime;
        float horizontal = Input.GetAxis("Horizontal") * ShipStats.Instance.GetStats().GetMaxSpeed() * Time.fixedDeltaTime;
        transform.Translate(horizontal, vertical, 0.0f);

        float angleV = Input.GetAxis("Vertical") * 1500.0f * Time.fixedDeltaTime;
        float angleH = Input.GetAxis("Horizontal") * 1500.0f * Time.fixedDeltaTime;
        shipChoice[index].transform.rotation = Quaternion.RotateTowards(shipChoice[index].transform.rotation, Quaternion.Euler(-angleV, 0, -angleH), 1500.0f);
    }

    public void PlayerDeath() { shipChoice[index].transform.gameObject.SetActive(false); }
}