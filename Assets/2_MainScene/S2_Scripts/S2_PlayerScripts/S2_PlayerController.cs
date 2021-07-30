//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerController : MonoBehaviour {
    public static S2_PlayerController Instance { get; private set; }

    [SerializeField] protected GameObject[] shipChoice = null;

    protected int index;

    void Awake() { Instance = this; }

    void Start() {
        SPD data = PDSM.LoadData();
        if (data != null) index = data.shipSelected;

        shipChoice[index].SetActive(true);

        S2_PlayerAnimationController.Instance.enabled = true;
    }

    void Update() {
        if (!ShipStats.Instance) return;

        if (S2_PlayerAnimationController.Instance.GetIsRolling()) return;
        float vertical = Input.GetAxis("Vertical") * ShipStats.Instance.GetStats().GetMaxSpeed() * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * ShipStats.Instance.GetStats().GetMaxSpeed() * Time.deltaTime;
        transform.Translate(horizontal, vertical, 0.0f);

        float angleV = Input.GetAxis("Vertical") * 1500.0f * Time.deltaTime;
        float angleH = Input.GetAxis("Horizontal") * 1500.0f * Time.deltaTime;
        shipChoice[index].transform.rotation = Quaternion.RotateTowards(shipChoice[index].transform.rotation, Quaternion.Euler(-angleV, 0, -angleH), 1500.0f);
    }

    public void PlayerDeath() { shipChoice[index].transform.gameObject.SetActive(false); }
}