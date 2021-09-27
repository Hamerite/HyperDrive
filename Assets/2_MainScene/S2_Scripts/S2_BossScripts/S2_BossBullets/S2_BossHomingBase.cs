using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossHomingBase : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float lateralSpeed;

    Transform player;
    [SerializeField]
    Transform child;

    // Start is called before the first frame update
    void Start()
    {
        player = S2_PlayerController.Instance.transform;
    }

    private void OnEnable()
    {
        Invoke(nameof(DisableBullet), 3);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        Vector3 lateralPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, lateralPos, lateralSpeed * Time.deltaTime);
        if (transform.position.z > player.position.z)
        {
            child.transform.LookAt(player.position);
        }
    }

    public void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}
