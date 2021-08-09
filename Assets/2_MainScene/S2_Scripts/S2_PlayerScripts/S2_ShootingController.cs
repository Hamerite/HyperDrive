//Created by Dylan LeClair 03/07/21
//Last modified 03/07/21 (Dylan LeClair)
using UnityEngine;

public class S2_ShootingController : MonoBehaviour {
    public static S2_ShootingController Instance { get; private set; }

    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected Texture2D crosshairs = null;

    protected Vector3 cursorPosition;
    protected Vector3 mousePos;
    protected Vector3 crosshairPosition;

    protected bool canShoot = true, usingGamepad = true;

    void Awake() { Instance = this; }

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

        if (canShoot && (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0.1f)) {
            canShoot = false;
            audioSource.Play();
            GameObject newObstacle = S2_BulletPooler.Instance.GetBullet();

            newObstacle.transform.position = ShipStats.Instance.GetGunPosition().position;
            newObstacle.SetActive(true);

            Invoke(nameof(ResetFireRate), 0.5f);
        }
    }

    void ResetFireRate() { canShoot = true; }

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

        cursorPosition.z = Input.mousePosition.z;
        crosshairPosition = new Vector3(cursorPosition.x, cursorPosition.y, cursorPosition.z);
    }

    void OnDestroy() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public Vector3 GetCrosshairPosition() { return crosshairPosition; }
}
