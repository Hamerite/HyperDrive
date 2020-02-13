using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PlayerController : MonoBehaviour
{
    public ParticleSystem playerDeathParticles;
    public AudioClip[] clips;

    GameManager gameManager;
    AudioSource audioSource;
    RaycastHit hit;

    int index;
    Transform shipModel;
    readonly List<GameObject> shipChoice = new List<GameObject>();

    new Collider collider;
    readonly Vector3[] dir = new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    bool canScore = true;
    readonly WaitForSeconds timer = new WaitForSeconds(0.25f);

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[0];
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            shipChoice.Add(item);
            item.SetActive(false);
        }
    }

    void Start()
    {
        index = PlayerPrefs.GetInt("Selection");
        shipChoice[index].SetActive(true);

        shipModel = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        collider = GetComponent<Collider>();

        audioSource.Play();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * 12.0f * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * 12.0f * Time.deltaTime;
        transform.Translate(horizontal, vertical, 0.0f);

        float angleH = Input.GetAxis("Horizontal") * 700.0f * Time.deltaTime;
        float angleV = Input.GetAxis("Vertical") * 700.0f * Time.deltaTime;
        shipModel.rotation = Quaternion.RotateTowards(shipModel.rotation, Quaternion.Euler(-angleV, 0, -angleH), 700.0f);

        foreach (Vector3 item in dir)
            if (Physics.Raycast(transform.position, item, out RaycastHit hit, 0.5f, 1 << 8))
                transform.Translate(hit.normal / 4.5f);

        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, transform.rotation, 2.1f, 1 << 9))
        {
            audioSource.clip = clips[1];
            audioSource.Play();
            playerDeathParticles.Play();

            shipChoice[index].SetActive(false);
            gameManager.ResetGame(null);

            if(!playerDeathParticles.IsAlive())
                gameManager.TraverseScenes(1, 2);
        }

        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, transform.rotation, 2.1f, 1 << 10))
        {
            if (gameManager.GetisAlive() && canScore)
            {
                gameManager.SetScore(1);
                canScore = false;

                StartCoroutine(ScoreLimiter());
            }
        }
        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, transform.rotation, 2.1f, 1 << 11))
        {
            if (gameManager.GetisAlive() && canScore)
            {
                gameManager.SetScore(2);
                canScore = false;

                StartCoroutine(ScoreLimiter());
            }
        }
        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, transform.rotation, 2.1f, 1 << 12))
        {
            if (gameManager.GetisAlive() && canScore)
            {
                gameManager.SetScore(3);
                canScore = false;

                StartCoroutine(ScoreLimiter());
            }
        }

        IEnumerator ScoreLimiter()
        {
            yield return timer;

            canScore = true;
        }
    }
}