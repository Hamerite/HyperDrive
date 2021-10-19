using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Enemy_Turret : MonoBehaviour
{
    [SerializeField] private float rotRad = 75;
    public float speed = 5;
    [SerializeField] Quaternion turretAngle;
    [SerializeField] GameObject pivotPoint;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetDir = S2_PlayerController.Instance.transform.position - pivotPoint.transform.position;
        //rotRad = Vector3.Angle(targetDir, transform.forward);
        
        rotRad = Mathf.Atan2(S2_PlayerController.Instance.transform.position.y - pivotPoint.transform.position.y, S2_PlayerController.Instance.transform.position.z - pivotPoint.transform.position.z);
        float AngleDeg = (180 / Mathf.PI) * rotRad;

        turretAngle = Quaternion.Slerp(pivotPoint.transform.rotation, Quaternion.Euler(rotRad, 0, 0), speed * Time.deltaTime);
        turretAngle.y = 0.0f;
        turretAngle.z = 0.0f;
        pivotPoint.transform.rotation = turretAngle;

    }
}
