//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)

using UnityEngine;

public class S2_PlayerCollisionController : MonoBehaviour
{
    public static S2_PlayerCollisionController Instance { get; private set; }

    [SerializeField] S2_PlayerAudioController playerAudio;
    [SerializeField] ParticleSystem playerDeathParticles;
    [SerializeField] new Collider collider;

    readonly LayerMask[] layers = { 9, 10, 11, 12 };
    readonly Vector3[] dir = new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    bool canScore = true;
    bool isAlive = true;

    void Start()
    {
        Instance = this;
    }

    void FixedUpdate()
    {
        foreach (Vector3 item in dir)
        {
            if (Physics.Raycast(transform.position, item, out RaycastHit hit, 0.5f, 1 << 8))
                transform.Translate(hit.normal / 4.5f);
            if (Physics.Raycast(transform.position, item, 10.0f, 1 << 9) && canScore)
                playerAudio.PlayAudio(1);
        }

        foreach (LayerMask item in layers)
        {
            if (canScore && isAlive && Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, out RaycastHit hit, transform.rotation, 2.1f, 1 << item))
            {
                canScore = false;

                if (item == 9)
                {
                    GetComponent<S2_PlayerController>().PlayerDeath();
                    playerAudio.PlayAudio(0);
                    Instantiate(playerDeathParticles, transform.position, Quaternion.identity);

                    isAlive = false;
                }
                if (item == 10 || item == 11 || item == 12)
                {
                    GameManager.Instance.SetNumbers("Score", item - 9);
                    hit.collider.gameObject.SendMessage("PointsScored");
                }

                Invoke(nameof(ScoreLimiter), 0.25f);
            }
        }
    }

    void ScoreLimiter()
    {
        canScore = true;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }
}
