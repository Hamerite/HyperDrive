//Created by Dylan LeClair 03/07/21
//Last modified 03/07/21 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerAnimationController : MonoBehaviour {
    public static S2_PlayerAnimationController Instance { get; protected set; }

    [SerializeField] protected Animator animator;

    protected int thrust;
    public bool IsRolling { get; protected set; }

    void Awake() { Instance = this; }

    void Start() { thrust = ShipStats.Instance.GetStats().GetAttributes()[3]; }

    void Update() {
        if (thrust == 0 || IsRolling) return;
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

    public void ToggleIsRolling() { IsRolling = !IsRolling; }
}
