//Created by Dylan LeClair 03/07/21
//Last modified 03/07/21 (Dylan LeClair)
using UnityEngine;

public class S2_ShootingController : MonoBehaviour {
    public static S2_ShootingController Instance { get; protected set; }

    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected Texture2D crosshairs = null;
    [SerializeField] protected LayerMask mask;

    protected Vector2 cursorPosition, crosshairPosition;
    protected Vector3 mousePos, targetPos;

    protected bool canShoot = true, usingGamepad = true;

    public Vector3 ScreenPos { get; protected set; }
    public Vector3 WorldPos { get; protected set; }
    public bool IntroFinished { get; protected set; }

    void Awake() { Instance = this; }

    void Start() {
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
        if (!IntroFinished) return;

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
        ScreenPos = Camera.main.ScreenToWorldPoint(crosshairPosition);
        WorldPos = Camera.main.ScreenToWorldPoint(new Vector3(crosshairPosition.x, crosshairPosition.y, 50));  
    }

    void ToggleIntroFinished() { IntroFinished = true; }

    void OnDestroy() {
        Cursor.visible = MenusManager.Instance.UsingMouse;
        Cursor.lockState = CursorLockMode.None;
    }
}
