//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerCollisionController : MonoBehaviour {
    public static S2_PlayerCollisionController Instance { get; private set; }

    [SerializeField] protected ParticleSystem playerDeathParticles = null;
    [SerializeField] protected new Collider collider = null;

    protected readonly LayerMask[] layers = { 9, 10, 11, 12 };
    protected readonly Vector3[] dir = new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    protected bool[] playerStatus = { true, true, false }; //{ CanScore, IsAlive, WasHit }

    void Start() { Instance = this; }

    void FixedUpdate() {
        foreach (Vector3 item in dir) {
            if (Physics.Raycast(transform.position, item, out RaycastHit hit, 0.5f, 1 << 8)) transform.Translate(hit.normal / 4.5f);
            if (Physics.Raycast(transform.position, item, 10.0f, 1 << 9) && playerStatus[0]) S2_PlayerAudioController.Instance.PlayAudio(1);
        }

        if (!playerStatus[0] || !playerStatus[1] || playerStatus[2]) return;
        foreach (LayerMask item in layers) {
            if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, out RaycastHit hit, transform.rotation, 2.1f, 1 << item)) {
                ScoreLimiter();

                if (item == 9) {
                    if (S2_HUDUI.Instance.GetAttributes()[0].value > 0) S2_HUDUI.Instance.SetAttributes(0, -1);
                    else if (S2_HUDUI.Instance.GetAttributes()[1].value > 1) S2_HUDUI.Instance.SetAttributes(1, -1);
                    else {
                        S2_HUDUI.Instance.SetAttributes(1, -1);
                        S2_HUDUI.Instance.SendGameInfo();
                        S2_PlayerController.Instance.PlayerDeath();
                        S2_PlayerAudioController.Instance.PlayAudio(0);
                        Instantiate(playerDeathParticles, transform.position, Quaternion.identity);

                        playerStatus[1] = false;
                    }
                    ToggleWasHit();
                    Invoke(nameof(ToggleWasHit), 0.25f);
                }
                if (item == 10 || item == 11 || item == 12) {
                    S2_HUDUI.Instance.SetScore(item - 9);
                    hit.collider.gameObject.SendMessage("PointsScored");
                }

                Invoke(nameof(ScoreLimiter), 0.25f);
            }
        }
    }

    void ScoreLimiter() { playerStatus[0] = !playerStatus[0]; }

    void ToggleWasHit() { playerStatus[2] = !playerStatus[2]; }

    public bool GetPlayerStatus(int index) { return playerStatus[index]; }
}
