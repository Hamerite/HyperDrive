//Created by Kyle Ennis 13/09/21
//Last modified 22/09/21 (Dylan LeClair)
using UnityEngine;

public class ToggleAlpha : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { S5_ShipSelectController.Instance.SetSlideAlpha(true); }
}
