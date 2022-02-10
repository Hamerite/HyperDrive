using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyPathPatterns : MonoBehaviour
{
    [SerializeField]
    Transform[] paths; //multiple spline paths required to make a pattern
    private int nextPath;
    float t, speed = 3;
    bool moveToNext;
    Vector3[] points = new Vector3[4];
    Vector3 targetPosition;
    Coroutine followPath;

    void Start()
    {
        nextPath = 0;
        t = 0;
        moveToNext = false;
    }

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    //ienumerator used to follow the path
    IEnumerator FollowPath(int pathNum)
    {
        if (moveToNext)
        {
            points[0] = paths[pathNum].GetChild(0).position;
            points[1] = paths[pathNum].GetChild(1).position;
            points[2] = paths[pathNum].GetChild(2).position;
            points[3] = paths[pathNum].GetChild(3).position;
        }
        moveToNext = false;

        while (t < 1)
        {
            t += Time.deltaTime * speed / 10;
            //bezier curve
            targetPosition = Mathf.Pow(1 - t, 3) * points[0] + 3 * Mathf.Pow(1 - t, 2) * t * points[1] + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2] + Mathf.Pow(t, 3) * points[3];

            transform.position = targetPosition;
            yield return endOfFrame;
        }

        t = 0;
        nextPath++;
        if (nextPath > paths.Length - 1)
            nextPath = 0;

        moveToNext = true;
    }
}
