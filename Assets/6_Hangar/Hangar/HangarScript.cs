using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.XR;

public class HangarScript : MonoBehaviour
{
    public static HangarScript Instance { private set; get; }
    [SerializeField] Button[] hangarButtons;
    [SerializeField] GameObject[] hangarAssets;
    [SerializeField] GameObject[] shipAssets;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject shipDropDownPanel;
    [SerializeField] float hangarLightTime, inspectionZoom;
    int hangarIndex = 0, shipIndex = 0, indexValidator;
    [SerializeField] Animator shipAnim, hangarDoorAnim, UIAnim;
    [SerializeField] GameObject camPos, camDefaultPos;
    [SerializeField] Camera cam;
    public Camera GetCam()
    {
        return cam;
    }
    bool isInspect = false, isStatic = false;
    [SerializeField] Light hangarLight;

    [SerializeField] Text shipInformation;
    [SerializeField] GameObject shipInformationPanel;
    [SerializeField] GameObject[] staticBullshit;
    public void SetShipInformation(string x)
    {
        shipInformation.text = x;
    }
    public GameObject GetShipDropDownPanel()
    {
        return shipDropDownPanel; 
    }
     
    void Start()
    {
        for (int i = 0; i < staticBullshit.Length; i++)
        {
            staticBullshit[i].SetActive(false);
        }
        shipDropDownPanel.SetActive(false);
        Instance = this;
        PlayShipAnimationOne();
        Invoke(nameof(SetButtonsInteractable), 3);
        backButton.SetActive(false);
        shipInformationPanel.SetActive(false);
        for (int i = 0; i < hangarAssets.Length; i++)
        {
            hangarAssets[i].transform.position = Vector3.zero;
            if (i != 0) hangarAssets[i].SetActive(false);
        }
        for (int i = 0; i < shipAssets.Length; i++)
        {
            if (i != 0) shipAssets[i].SetActive(false);
        }
        Invoke(nameof(ChangeHangarLight), hangarLightTime);
        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close"))
            UIAnim.SetTrigger("open");
    }

    private void Update()
    {
 
        hangarLight.intensity = Mathf.PingPong(Time.time * 5, 5);
    }

    private void LateUpdate()
    {
        switch (isInspect)
        {
            case true:
                if (Vector3.Distance(cam.transform.position, camPos.transform.position) > 0.01f) cam.transform.position = Vector3.Slerp(cam.transform.position, camPos.transform.position, inspectionZoom * Time.deltaTime);
                break;
            case false:
                if (Vector3.Distance(cam.transform.position, camDefaultPos.transform.position) > 0.01f) cam.transform.position = Vector3.Slerp(cam.transform.position, camDefaultPos.transform.position, inspectionZoom * Time.deltaTime);
                break;
        }
    }
    public void NextShip()
    {
        ShipDropDownSelector.Instance.GetAnimator().SetTrigger("Close");
        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("open"))
            UIAnim.SetTrigger("close");
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle"))
        {
            Invoke(nameof(SetButtonsInteractable), 3);
            shipIndex++;
            PlayShipAnimationOne();
            if (shipIndex > shipAssets.Length-1)
                shipIndex = 0;
            for (int i = 0; i < shipAssets.Length; i++)
            {
                if (i == shipIndex) shipAssets[i].SetActive(true);
                else
                {
                    if (shipAssets[i].activeInHierarchy) shipAssets[i].SetActive(false);
                }
            }
            if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close"))
                UIAnim.SetTrigger("open");
        }
        else if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter") || shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipExit"))
        {
            SetButtonsInteractable();
            PlayShipAnimationTwo();
            Invoke(nameof(NextShip), 5.5f);
        }
    }

    public void PreviousShip()
    {
        ShipDropDownSelector.Instance.GetAnimator().SetTrigger("Close");
        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("open"))
            UIAnim.SetTrigger("close");
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle"))
        {
            Invoke(nameof(SetButtonsInteractable),3);
            shipIndex--;
            PlayShipAnimationOne();
            if (shipIndex < 0) shipIndex = shipAssets.Length-1;
            for (int i = 0; i < shipAssets.Length; i++)
            {
                if (i == shipIndex) shipAssets[i].SetActive(true);
                else
                {
                    if (shipAssets[i].activeInHierarchy) shipAssets[i].SetActive(false);
                }
            }
            if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close"))
                UIAnim.SetTrigger("open");

        }
        else if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter") || shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipExit"))
        {
            SetButtonsInteractable();
            PlayShipAnimationTwo();
            Invoke(nameof(PreviousShip), 5.5f);
        }
    }

    public void InspectShip()
    {
        isInspect = true;
       // ShipStatsController.Instance.slideAlpha = true;
        SetButtonsActive(false);
        backButton.SetActive(true);
        shipInformationPanel.SetActive(true);
        SetShipInformation(ShipStatistics.Instance.GetStats().GetInformation());
    }

    public void DefaultCam()
    {
        isInspect = false;
        ShipStatisticsController.Instance.slideAlpha = false;
        SetButtonsActive(true);
        backButton.SetActive(false);
        shipInformationPanel.SetActive(false);
    }
    void SetButtonsInteractable()
    {
        for (int i = 0; i < hangarButtons.Length; i++)
        {
            hangarButtons[i].interactable = !hangarButtons[i].interactable;
        }
    }

    void SetButtonsActive(bool x)
    {
        for (int i = 0; i < hangarButtons.Length; i++)
        {
            hangarButtons[i].gameObject.SetActive(x);
        }
    }

    void PlayShipAnimationOne()
    {
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle"))
        {
            hangarDoorAnim.SetTrigger("triggerDoor");
            shipAnim.SetTrigger("ShipIn");
        }
    }

    void PlayShipAnimationTwo()
    {
        if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter"))
        {
            Invoke(nameof(PlayHangarDoorAnimation), 3.3f);
            shipAnim.SetTrigger("ShipOut");
        }
    }

    void PlayHangarDoorAnimation()
    {
        hangarDoorAnim.SetTrigger("triggerDoor");
    }
    void ChangeHangarLight()
    {
        hangarIndex++;
        if (hangarIndex > hangarAssets.Length - 1) hangarIndex = 0;
        for (int i = 0; i < hangarAssets.Length; i++)
        {
            if (i == hangarIndex) hangarAssets[i].SetActive(true);
            else
                hangarAssets[i].SetActive(false);
        }
        Invoke(nameof(ChangeHangarLight), hangarLightTime);
    }

    public void SelectShipFromDropDown(int index)
    {
        ShipDropDownSelector.Instance.GetAnimator().SetTrigger("Close");
        if(UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("open"))
            UIAnim.SetTrigger("close");
        shipIndex = 0;
        switch (ShipDropDownSelector.Instance.ReturnTier())
        {
            case 0:
                shipIndex = index;
                if (shipIndex != indexValidator)
                {
                    if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle"))
                    {
                        Invoke(nameof(SetButtonsInteractable), 3);
                        PlayShipAnimationOne();
                        for (int i = 0; i < shipAssets.Length; i++)
                        {
                            if (i == shipIndex) shipAssets[shipIndex].SetActive(true);
                            else
                            {
                                if (shipAssets[i].activeInHierarchy) shipAssets[i].SetActive(false);
                            }
                        }
                        indexValidator = shipIndex;
                        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close"))
                            UIAnim.SetTrigger("open");
                    }
                    else if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter") || shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipExit"))
                    {
                        SetButtonsInteractable();
                        PlayShipAnimationTwo();
                        StartCoroutine(WaitForShip(5.5f, index));
                    }
                }
                break;
            case 1:
                shipIndex = index + 5;
                if (shipIndex != indexValidator)
                {
                    if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle"))
                    {
                        Invoke(nameof(SetButtonsInteractable), 3);
                        PlayShipAnimationOne();
                        for (int i = 0; i < shipAssets.Length; i++)
                        {
                            if (i == shipIndex) shipAssets[shipIndex].SetActive(true);
                            else
                            {
                                if (shipAssets[i].activeInHierarchy) shipAssets[i].SetActive(false);
                            }
                        }
                        indexValidator = shipIndex;
                        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close"))
                            UIAnim.SetTrigger("open");
                    }
                    else if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter") || shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipExit"))
                    {
                        SetButtonsInteractable();
                        PlayShipAnimationTwo();
                        StartCoroutine(WaitForShip(5.5f, index));
                    }
                }
                break;
            case 2:
                shipIndex = index + 10;
                if (shipIndex != indexValidator)
                {
                    if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipIdle"))
                    {
                        Invoke(nameof(SetButtonsInteractable), 3);
                        PlayShipAnimationOne();
                        for (int i = 0; i < shipAssets.Length; i++)
                        {
                            if (i == shipIndex) shipAssets[shipIndex].SetActive(true);
                            else
                            {
                                if (shipAssets[i].activeInHierarchy) shipAssets[i].SetActive(false);
                            }
                        }
                        if (UIAnim.GetCurrentAnimatorStateInfo(0).IsTag("close"))
                            UIAnim.SetTrigger("open");
                        indexValidator = shipIndex;
                    }
                    else if (shipAnim.GetCurrentAnimatorStateInfo(0).IsName("ShipEnter") || shipAnim.GetCurrentAnimatorStateInfo(0).IsName("shipExit"))
                    {
                        SetButtonsInteractable();
                        PlayShipAnimationTwo();
                        StartCoroutine(WaitForShip(5.5f, index));
                    }
              
                }
                break;
        }
    }

    public IEnumerator WaitForShip(float waitTime, int tier)
    {
        yield return new WaitForSeconds(waitTime);
        SelectShipFromDropDown(tier);
    }

    public void StartStatic()
    {
        isStatic = true;
        StartCoroutine(nameof(WaitForStatic));
    }
    public IEnumerator WaitForStatic()
    {
        switch (isStatic)
        {
            case true:
                for (int i = 0; i < staticBullshit.Length; i++)
                {
                    staticBullshit[i].SetActive(true);
                }
                isStatic = false;
                StartCoroutine(nameof(WaitForStatic));
                break;
            case false:
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < staticBullshit.Length; i++)
                {
                    staticBullshit[i].SetActive(false);
                }
                break;
        }

    }
}
