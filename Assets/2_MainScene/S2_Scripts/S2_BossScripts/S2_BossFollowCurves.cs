//created 22/09/21 by AT
//Last modified 15/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_BossFollowCurves : MonoBehaviour {
    [SerializeField] protected Transform[] points;

    protected Vector3 gismoPosition;

    void OnDrawGizmos() {
        for (float t = 0; t <= 1; t += 0.05f) {
            gismoPosition = Mathf.Pow(1 - t, 3) * points[0].position + 3 * Mathf.Pow(1 - t, 2) * t * points[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2].position + Mathf.Pow(t, 3) * points[3].position; //bezier curve
            Gizmos.DrawSphere(gismoPosition, 0.25f);
        }

        Gizmos.DrawLine(points[0].position, points[1].position); //for visualization
        Gizmos.DrawLine(points[2].position, points[3].position);
    }
}
