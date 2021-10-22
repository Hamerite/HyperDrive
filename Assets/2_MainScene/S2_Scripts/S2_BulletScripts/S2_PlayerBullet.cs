//Created by Dylan LeClair 04/08/21
//Last modified 04/08/21 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerBullet : MonoBehaviour {
    [SerializeField] protected new Rigidbody rigidbody = null;

    protected Vector3 worldPosition;
    protected Vector3 offset = new Vector3(0, 0, -5);

    void OnEnable() {
        Plane plane = new Plane(Vector3.back, 25);
        Ray ray = Camera.main.ScreenPointToRay(S2_ShootingController.Instance.GetCrosshairPosition());
        float distance;

        if (plane.Raycast(ray, out distance)) worldPosition = ray.GetPoint(distance);

        rigidbody.velocity = Vector3.zero;
        transform.LookAt(worldPosition);
        Invoke(nameof(TimedOut), 7);
    }

    void FixedUpdate() { rigidbody.position += (transform.forward) * (100 * Time.deltaTime); }

    void TimedOut() { gameObject.SetActive(false); }

    void OnTriggerEnter(Collider other) {
        bool hitObstacle = false;
        if (other.gameObject.layer == 9) hitObstacle = true;
        ParticleSystem newExplosion = S2_HitExplosionPooler.Instance.GetHitExplosion(hitObstacle);

        if(other.gameObject.layer == 9)
            newExplosion.transform.position = transform.position + offset;
        else
            newExplosion.transform.position = transform.position;


        newExplosion.time = 0;
        newExplosion.Play();
        newExplosion.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
