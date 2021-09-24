//Created by Dylan LeClair 23/09/21
//Last modified 23/09/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S5_ButtonsController : MonoBehaviour {
    public static S5_ButtonsController Instance { get; private set; }

    [SerializeField] protected Button[] buttons;
    [SerializeField] protected Text backButtonText;

    protected WaitForSeconds waitForSeconds = new WaitForSeconds(5.5f);

    protected int shipIndex, indexValidator;

    void OnEnable() {
        Instance = this;
        Invoke(nameof(CheckForButtonSelected), 0.001f);
    }

    void CheckForButtonSelected() { MenusManager.Instance.SetSelectedButton(buttons[0], null, false); }

    public void SelectButton() {
        if (S5_ShipSelect.Instance.GetCurrentMenuShip() == true) {
            AudioManager.Instance.PlayInteractionSound(1);
            S5_ShipSelect.Instance.SavePurchaseData();
            S5_AnimationsController.Instance.PlayShipAnimationTwo();
            SetButtonsInteractable();

            Invoke(nameof(StartGame), 5.5f);
        }
        else if (S5_ShipSelect.Instance.GetCurrentMenuShip() == false && S5_ShipSelect.Instance.CheckPlayerWealth()) {
            MenusManager.Instance.SetSelectedButton(buttons[0], null, false);

            AudioManager.Instance.PlayInteractionSound(6);
            FeedbackMessageController.Instance.SetMessage("PURCHASED", Color.yellow);
            S5_ShipSelect.Instance.PurchaseShip();
        }
        else {
            MenusManager.Instance.SetSelectedButton(buttons[0], null, false);
            AudioManager.Instance.PlayInteractionSound(4);
        }
    }

    void StartGame() { GameManager.Instance.TraverseScenes("MainScene"); }

    public void Previous() {
        MenusManager.Instance.SetSelectedButton(buttons[1], null, false);
        S5_ShipSelect.Instance.GoToNextShip(-1);
    }

    public void Next() {
        MenusManager.Instance.SetSelectedButton(buttons[2], null, false);
        S5_ShipSelect.Instance.GoToNextShip(1);
    }

    public void InspectShip() {
        AudioManager.Instance.PlayInteractionSound(1);
        S5_TrackingCamController.Instance.SetIsInspect(true);

        SetButtonsActive(false);
        backButtonText.text = "BACK";

        S5_ShipSelect.Instance.SetInformationPanel(true);
        S5_ShipSelect.Instance.SetShipInformation(ShipStats.Instance.GetStats().GetDescription());
    }

    public void BackButton() {
        AudioManager.Instance.PlayInteractionSound(1);

        if (!S5_ShipSelect.Instance.GetInformationPanelState()) GameManager.Instance.TraverseScenes("StartScreen");
        else {
            S5_TrackingCamController.Instance.SetIsInspect(false);

            SetButtonsActive(true);
            backButtonText.text = "MAIN MENU";

            S5_ShipSelect.Instance.SetInformationPanel(false);
            S5_ShipSelectController.Instance.SetSlideAlpha(false);
        }
    }

    public void SelectShipFromDropDown(int index) {
        AudioManager.Instance.PlayInteractionSound(1);
        ShipDropDownSelector.Instance.GetAnimator().SetTrigger("Close");
        S5_AnimationsController.Instance.ToggleUIAnimation();

        shipIndex = 0;
        if (ShipDropDownSelector.Instance.ReturnTier() == 0) shipIndex = index;
        else if (ShipDropDownSelector.Instance.ReturnTier() == 1) shipIndex = index + 5;
        else if (ShipDropDownSelector.Instance.ReturnTier() == 2) shipIndex = index + 10;

        S5_ShipSelect.Instance.SetIndex(shipIndex);

        if (shipIndex != indexValidator) {
            if (S5_AnimationsController.Instance.GetShipAnimationState() == "shipIdle") {
                S5_AnimationsController.Instance.PlayShipAnimationOne();
                Invoke(nameof(SetButtonsInteractable), 3);

                for (int i = 0; i < S5_ShipSelect.Instance.GetShips().Length; i++) {
                    if (i == shipIndex) S5_ShipSelect.Instance.GetShips()[shipIndex].SetActive(true);
                    else if (S5_ShipSelect.Instance.GetShips()[i].activeInHierarchy) S5_ShipSelect.Instance.GetShips()[i].SetActive(false);
                }

                indexValidator = shipIndex;
                S5_AnimationsController.Instance.ToggleUIAnimation();
            }
            else {
                S5_AnimationsController.Instance.PlayShipAnimationTwo();
                SetButtonsInteractable();
                StartCoroutine(WaitForShip(index));
            }
        }
    }

    public IEnumerator WaitForShip(int tier) {
        yield return waitForSeconds;
        SelectShipFromDropDown(tier);
    }

    public void ButtonSelected() { AudioManager.Instance.PlayInteractionSound(0); }

    void SetButtonsActive(bool state) { for (int i = 0; i < buttons.Length - 1; i++) buttons[i].gameObject.SetActive(state); }
    
    public void SetButtonsInteractable() { foreach (Button item in buttons) item.interactable = !item.interactable; }

    public int GetShipIndex() { return shipIndex; }
}
