//Created by Dylan LeClair 03/07/21
//Last modified 03/07/21 (Dylan LeClair)
using UnityEngine;

public class S2_ShootingController : MonoBehaviour {
    [SerializeField] Texture2D crosshairs = null;

    Vector2 cursorPosition;
    Vector3 mousePos;

    bool usingGamepad = true;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        mousePos = Input.mousePosition;
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    void Update() {
        if (usingGamepad && mousePos != Input.mousePosition) {
            usingGamepad = false;
        }
        else if (!usingGamepad && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)) {
            mousePos = Input.mousePosition;
            usingGamepad = true;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0.1f) { Debug.Log("SHOOTING"); }
    }

    void OnGUI() {
        if (usingGamepad) {
            float horizontal = 500 * Input.GetAxis("Mouse X") * Time.deltaTime;
            float vertical = 500 * Input.GetAxis("Mouse Y") * Time.deltaTime;

            cursorPosition.x += horizontal;
            cursorPosition.y += vertical;
        }
        else {
            cursorPosition.x = Input.mousePosition.x;
            cursorPosition.y = Input.mousePosition.y;
        }

        Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(cursorPosition.x, cursorPosition.y, 0));
        if (pos.x < 0) cursorPosition.x += 5;
        if (pos.x > 52.5) cursorPosition.x -= 5;
        if (pos.y < 1.5f) cursorPosition.y += 5;
        if (pos.y > 53.5) cursorPosition.y -= 5;

        GUI.DrawTexture(new Rect(cursorPosition.x, Screen.height - cursorPosition.y, 20, 20), crosshairs);
    }

    void OnDestroy() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
