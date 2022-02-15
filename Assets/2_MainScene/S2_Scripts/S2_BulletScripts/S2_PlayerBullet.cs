//Created by Dylan LeClair 04/08/21
//Last modified 04/08/21 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerBullet : MonoBehaviour {
    [SerializeField] protected new Rigidbody rigidbody;
    [SerializeField] protected LayerMask mask;

    protected Vector3 worldPosition;
    protected Vector3 offset = new Vector3(0, 0, -5);

    void OnEnable() {
        Vector3 screenPos = S2_ShootingController.Instance.ScreenPos;
        Vector3 worldPos = S2_ShootingController.Instance.WorldPos;

        RaycastHit hit;
        Vector3 checkStart = (worldPos - screenPos).normalized;
        bool castHit = Physics.Raycast(screenPos + (checkStart * 15), checkStart, out hit, 35, mask); 

        Vector3 targetPos;
        if (castHit) { targetPos = hit.point; }
        else { targetPos = worldPos; }

        rigidbody.velocity = Vector3.zero;
        transform.LookAt(targetPos);
        Invoke(nameof(TimedOut), 8);
    }

    void FixedUpdate() { rigidbody.position += (transform.forward) * (100 * Time.deltaTime); }

    void TimedOut() { CancelInvoke(nameof(TimedOut)); gameObject.SetActive(false); }

    void OnTriggerEnter(Collider other) {
        bool hitObstacle = false;
        if (other.gameObject.layer == 9) hitObstacle = true;
        ParticleSystem newExplosion = S2_HitExplosionPooler.Instance.GetHitExplosion(hitObstacle);

        if (other.gameObject.layer == 9) { 
            newExplosion.transform.position = transform.position + offset;
            newExplosion.transform.parent = other.transform.parent;
        }
        else { 
            newExplosion.transform.position = transform.position;
            newExplosion.transform.parent = null;
        }
            
        newExplosion.time = 0;
        newExplosion.Play();
        newExplosion.gameObject.SetActive(true);    
        gameObject.SetActive(false);
    }
}
