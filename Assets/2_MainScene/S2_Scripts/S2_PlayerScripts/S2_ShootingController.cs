//Created by Dylan LeClair 03/07/21
//Last modified 03/07/21 (Dylan LeClair)
using UnityEngine;

public class S2_ShootingController : MonoBehaviour {
    public static S2_ShootingController Instance { get; private set; }

    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected Texture2D crosshairs = null;
    [SerializeField] private LayerMask mask;

    protected Vector2 cursorPosition, crosshairPosition;
    protected Vector3 mousePos, targetPos, screenPos, worldPos;

    protected bool canShoot = true, usingGamepad = true, wasUsingMouse, introFinished;

    void Awake() { Instance = this; }

    void Start() {
        wasUsingMouse = Cursor.visible;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        mousePos = Input.mousePosition;
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        Invoke(nameof(ToggleIntroFinished), 4);
    }

    void Update() {
        if (usingGamepad && mousePos != Input.mousePosition) {
            usingGamepad = false;
        }
        else if (!usingGamepad && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)) {
            mousePos = Input.mousePosition;
            usingGamepad = true;
        }

        if (canShoot && (Input.GetButton("Fire1") || Input.GetAxis("Fire1") > 0.1f)) {
            canShoot = false;
            audioSource.Play();
            GameObject newBullet = S2_BulletPooler.Instance.GetBullet();

            newBullet.transform.position = ShipStats.Instance.GetGunPosition().position;
            newBullet.SetActive(true);

            Invoke(nameof(ResetFireRate), 0.5f);
        }
    }

    void ResetFireRate() { canShoot = true; }

    void OnGUI() {
        if (!introFinished) return;

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

        GUI.DrawTexture(new Rect(cursorPosition.x - 10, Screen.height - cursorPosition.y - 10, 20, 20), crosshairs);

        crosshairPosition = new Vector3(cursorPosition.x, cursorPosition.y, 0);
        screenPos = Camera.main.ScreenToWorldPoint(crosshairPosition);
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(crosshairPosition.x, crosshairPosition.y, 50));  
    }

    void ToggleIntroFinished() { introFinished = true; }

    void OnDestroy() {
        Cursor.visible = wasUsingMouse;
        Cursor.lockState = CursorLockMode.None;
    }

    public Vector3 GetCursorPosition() { return cursorPosition; }
    public Vector3 GetCrosshairPosition() { return crosshairPosition; }
    public Vector3 GetScreenPos() { return screenPos; }
    public Vector3 GetWorldPos() { return worldPos; }
    public bool GetIntroFinished() { return introFinished; }
}
