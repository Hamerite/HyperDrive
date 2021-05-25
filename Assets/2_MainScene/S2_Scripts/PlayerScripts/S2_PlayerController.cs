//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerController : MonoBehaviour {
    Transform shipModel;

    [SerializeField] GameObject[] shipChoice = null;

    int index;

    void Start() {
        index = PlayerPrefs.GetInt("Selection");
        shipChoice[index].SetActive(true);
        shipModel = shipChoice[index].transform;
    }

    void Update() {
        float vertical = Input.GetAxis("Vertical") * 12.0f * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * 12.0f * Time.deltaTime;
        transform.Translate(horizontal, vertical, 0.0f);

        float angleV = Input.GetAxis("Vertical") * 1500.0f * Time.deltaTime;
        float angleH = Input.GetAxis("Horizontal") * 1500.0f * Time.deltaTime;
        shipModel.rotation = Quaternion.RotateTowards(shipModel.rotation, Quaternion.Euler(-angleV, 0, -angleH), 1500.0f);
    }

    public void PlayerDeath() { shipModel.gameObject.SetActive(false); }
}