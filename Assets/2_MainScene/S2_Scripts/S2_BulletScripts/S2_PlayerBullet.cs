//Created by Dylan LeClair 04/08/21
//Last modified 04/08/21 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerBullet : MonoBehaviour {
    [SerializeField] protected new Rigidbody rigidbody = null;

    protected Vector3 worldPosition;

    protected Vector3 offset = new Vector3(0, 0, -5);

    void OnEnable() {
        Plane plane = new Plane(Vector3.back, 30);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(S2_ShootingController.Instance.GetCrosshairPosition());

        if (plane.Raycast(ray, out distance)) worldPosition = ray.GetPoint(distance);

        rigidbody.velocity = Vector3.zero;
        transform.LookAt(worldPosition);
        Invoke(nameof(TimedOut), 7);
    }

    void Update() { rigidbody.AddForce(transform.forward * 5, ForceMode.Impulse); }

    void TimedOut() { gameObject.SetActive(false); }

    void OnTriggerEnter(Collider other) {
        ParticleSystem newExplosion = S2_HitExplosionPooler.Instance.GetHitExplosion();

        newExplosion.transform.position = transform.position + offset;
        newExplosion.time = 0;
        newExplosion.Play();
        newExplosion.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
