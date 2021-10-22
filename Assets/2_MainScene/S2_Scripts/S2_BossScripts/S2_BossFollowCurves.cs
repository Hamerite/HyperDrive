//created 22/09/21 by AT
//script to draw gismos for boss paths to help visualize in engine

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossFollowCurves : MonoBehaviour
{
    [SerializeField]
    Transform[] points;

    private Vector3 gismoPosition;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            //bezier curve
            gismoPosition = Mathf.Pow(1 - t, 3) * points[0].position + 3 * Mathf.Pow(1 - t, 2) * t * points[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2].position + Mathf.Pow(t, 3) * points[3].position;

            Gizmos.DrawSphere(gismoPosition, 0.25f);
        }

        //for visualization
        Gizmos.DrawLine(points[0].position, points[1].position);

        Gizmos.DrawLine(points[2].position, points[3].position);

    }

}
