//Created by Dylan LeClair 03/07/21
//Last modified 03/07/21 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerAnimationController : MonoBehaviour {
    public static S2_PlayerAnimationController Instance { get; private set; }

    [SerializeField] protected Animator animator = null;

    protected bool isRolling = false;
    protected int thrust;

    void Awake() { Instance = this; }

    void Start() { thrust = ShipStats.Instance.GetStats().GetAttributes()[3]; }

    void Update() {
        if (thrust == 0 || isRolling) return;
        if (Input.GetButtonDown("RollRight")) {
            animator.SetTrigger("RollRight");
            UseThrust();
        }
        else if (Input.GetButtonDown("RollLeft")) {
            animator.SetTrigger("RollLeft");
            UseThrust();
        }
    }

    void UseThrust() {
        ToggleIsRolling();
        thrust--;
        S2_HUDUI.Instance.SetAttributes(2, -1);

        S2_HUDUI.Instance.StartCountdownTimer();
        Invoke(nameof(GainThrust), ShipStats.Instance.GetStats().GetAttributes()[4]);
    }

    void GainThrust() { 
        thrust++;
        S2_HUDUI.Instance.SetAttributes(2, 1);
    }
    public void ResetZRotation()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public bool GetIsRolling() { return isRolling; }

    public void ToggleIsRolling() { isRolling = !isRolling; }
}
