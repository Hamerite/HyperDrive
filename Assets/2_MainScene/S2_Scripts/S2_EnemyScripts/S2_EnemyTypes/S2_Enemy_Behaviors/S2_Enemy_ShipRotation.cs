using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Enemy_ShipRotation : MonoBehaviour
{
    [SerializeField] S2_EnemyStats parentShip = null;
    float angleY = 0;
    // Update is called once per frame
    void Update()
    {
        if (parentShip.GetTargetPosition() != null)
        {
            if (10 - parentShip.transform.position.z < 6) angleY = 180;
            float angleV = 0;//parentShip.GetTargetPosition().y - transform.position.y * 10f * Time.deltaTime;
            float angleH = 0;//parentShip.GetTargetPosition().x - transform.position.x * 10f * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-angleV, angleY, -angleH), 150.00f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            parentShip.TakeDamage();
        }
    }
}
