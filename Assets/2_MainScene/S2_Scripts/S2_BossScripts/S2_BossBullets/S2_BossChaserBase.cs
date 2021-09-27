using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossChaserBase : MonoBehaviour
{
    [SerializeField]
    float speed;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        Invoke(nameof(StopDisableBullet), 3);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }


    public virtual void Movement()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    public void StopDisableBullet()
    {
        gameObject.SetActive(false);
    }

}
