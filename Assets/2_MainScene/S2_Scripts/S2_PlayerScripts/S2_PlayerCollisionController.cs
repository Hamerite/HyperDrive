//Created by Dylan LeClair
//Last modified 04-11-21 (Alek Tepylo)
using UnityEngine;

public class S2_PlayerCollisionController : MonoBehaviour {
    public static S2_PlayerCollisionController Instance { get; private set; }

    [SerializeField] protected ParticleSystem playerDeathParticles = null;
    [SerializeField] protected ParticleSystem hitSparks = null;
    [SerializeField] protected new Collider collider = null;

    protected readonly LayerMask[] layers = { 9, 10, 11, 12 };
    protected readonly Vector3[] dir = new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    protected bool[] playerStatus = { true, true, false }; //{ CanScore, IsAlive, WasHit }

    void Start() { Instance = this; }

    void FixedUpdate() {
        for (int i = 0; i < dir.Length; i++) {
            if (Physics.Raycast(transform.position, dir[i], out RaycastHit hit, 0.5f, 1 << 8)) {
                transform.Translate(hit.normal / 4.5f);
                PlaySparks(hit.point);
            }
            if (Physics.Raycast(transform.position, dir[i], 10.0f, 1 << 9) && playerStatus[0]) S2_PlayerAudioController.Instance.PlayRandomWhoosh();
        }      
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!playerStatus[0] || !playerStatus[1] || playerStatus[2]) { return; }

        ScoreLimiter();

        if (collision.gameObject.layer == 9)
        {
            if (S2_HUDUI.Instance.GetAttributes()[0].value > 0) S2_HUDUI.Instance.SetAttributes(0, -1);
            else if (S2_HUDUI.Instance.GetAttributes()[1].value > 1) S2_HUDUI.Instance.SetAttributes(1, -1);
            else
            {
                S2_HUDUI.Instance.SetAttributes(1, -1);
                S2_HUDUI.Instance.SendGameInfo();
                S2_PlayerController.Instance.PlayerDeath();
                S2_PlayerAudioController.Instance.PlayAudio(0);
                Instantiate(playerDeathParticles, transform.position, Quaternion.identity);

                playerStatus[1] = false;
            }

            PlaySparks(collision.ClosestPointOnBounds(transform.position));
            ToggleWasHit();
            Invoke(nameof(ToggleWasHit), 0.25f);
        }
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 12) { S2_HUDUI.Instance.SetScore(collision.gameObject.layer - 9); }

        Invoke(nameof(ScoreLimiter), 0.25f);

    }   

    void PlaySparks(Vector3 hit) { 
        if (!hitSparks.gameObject.activeInHierarchy) {
            hitSparks.transform.position = hit;
            hitSparks.gameObject.SetActive(true);
            S2_PlayerAudioController.Instance.PlayAudio(6);
            hitSparks.Play();
         }
    }

    void ScoreLimiter() { playerStatus[0] = !playerStatus[0]; }

    void ToggleWasHit() { playerStatus[2] = !playerStatus[2]; }

    public bool GetPlayerStatus(int index) { return playerStatus[index]; }
}
