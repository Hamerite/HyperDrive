using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CPC_ECurveType { EaseInAndOut, Linear, Custom }

public enum CPC_EAfterLoop { Continue, Stop }

[System.Serializable]
public class CPC_Visual {
    public Color pathColor = Color.green;
    public Color inactivePathColor = Color.gray;
    public Color frustrumColor = Color.white;
    public Color handleColor = Color.yellow;
}

[System.Serializable]
public class CPC_Point {
    public Quaternion rotation;
    public Vector3 position;
    public Vector3 handleprev;
    public Vector3 handlenext;

    public AnimationCurve rotationCurve;
    public AnimationCurve positionCurve;

    public CPC_ECurveType curveTypeRotation;
    public CPC_ECurveType curveTypePosition;

    public bool chained;
    public CPC_Point(Vector3 pos, Quaternion rot) {
        rotation = rot;
        position = pos;
        handleprev = Vector3.back;
        handlenext = Vector3.forward;

        rotationCurve = AnimationCurve.EaseInOut(0,0,1,1);
        positionCurve = AnimationCurve.Linear(0,0,1,1);

        curveTypeRotation = CPC_ECurveType.EaseInAndOut;
        curveTypePosition = CPC_ECurveType.Linear;

        chained = true;
    }
}

public class CPC_CameraPath : MonoBehaviour {
    public CPC_Visual visual;

    [SerializeField] protected Camera selectedCamera;
    [SerializeField] protected Transform target;
    [SerializeField] protected List<CPC_Point> points = new List<CPC_Point>();

    [SerializeField] protected bool playOnAwake, useMainCamera, lookAtTarget, looped, hasTIme, alwaysShow;
    [SerializeField] protected float m_Timer;
    [SerializeField] protected CPC_EAfterLoop afterLoop = CPC_EAfterLoop.Continue;

    protected bool playing, paused;
    protected int currentWaypointIndex;
    protected float currentTimeInWaypoint, timePerSegment;

    void Start () {
	    foreach (var index in points) {
            if (index.curveTypeRotation == CPC_ECurveType.EaseInAndOut) index.rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (index.curveTypeRotation == CPC_ECurveType.Linear) index.rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
            if (index.curveTypePosition == CPC_ECurveType.EaseInAndOut) index.positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (index.curveTypePosition == CPC_ECurveType.Linear) index.positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }

    public void PlayPath(float time, float t, float ET ,CPC_CameraPath endCam) {
        if (time <= 0) time = 0.001f;
        paused = false;
        playing = true;

        StopAllCoroutines();
        StartCoroutine(FollowPath(time,t, ET,endCam));
    }

    public void ResumePath() {
        if (paused) playing = true;
        paused = false;
    }

    public void StopPath(float t, float ET,CPC_CameraPath endCam) {
        playing = false;
        paused = false;
        if (!playing && t != 0) Invoke(nameof(waitForTime), t);
        else if (!playing && ET != 0) Invoke(nameof(waitForTime), ET);
    }

    public void PausePath() {
        paused = true;
        playing = false;
    }

    void waitForTime() { StopAllCoroutines(); }

    public void UpdateTimeInSeconds(float seconds) {
        if (hasTIme) timePerSegment = seconds;
        else timePerSegment = seconds / ((looped) ? points.Count : points.Count - 1);
    }

    public void RefreshTransform() {
        Camera.main.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
       
        if (!lookAtTarget) Camera.main.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
        else Camera.main.transform.rotation = Quaternion.LookRotation((target.transform.position - Camera.main.transform.position).normalized);
    }

    IEnumerator FollowPath(float time, float t, float ET,CPC_CameraPath endCam) {
        if (!hasTIme) {
            UpdateTimeInSeconds(time);
            currentWaypointIndex = 0;
         
            while (currentWaypointIndex < points.Count) {
                currentTimeInWaypoint = 0;
                while (currentTimeInWaypoint < 1) {
                    if (!paused) {
                        currentTimeInWaypoint += Time.deltaTime / timePerSegment;
                        Camera.main.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
                       
                        if (!lookAtTarget) Camera.main.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
                        else Camera.main.transform.rotation = Quaternion.LookRotation((target.transform.position - Camera.main.transform.position).normalized);
                    }
                    yield return 0;
                }
                ++currentWaypointIndex;
                if (currentWaypointIndex == points.Count - 1 && !looped) break;
                if (currentWaypointIndex == points.Count && afterLoop == CPC_EAfterLoop.Continue) currentWaypointIndex = 0;
            }
            StopPath(t,ET,endCam);
        }
        else {
            UpdateTimeInSeconds(m_Timer);
            currentWaypointIndex = 0;

            while (currentWaypointIndex < points.Count) {
                currentTimeInWaypoint = 0;
                while (currentTimeInWaypoint < 1) {
                    if (!paused) {
                        currentTimeInWaypoint += Time.deltaTime / timePerSegment;
                        Camera.main.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
                        
                        if (!lookAtTarget) Camera.main.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
                        else Camera.main.transform.rotation = Quaternion.LookRotation((target.transform.position - Camera.main.transform.position).normalized);
                    }
                    yield return 0;
                }
                ++currentWaypointIndex;
                if (currentWaypointIndex == points.Count - 1 && !looped) break;
                if (currentWaypointIndex == points.Count && afterLoop == CPC_EAfterLoop.Continue) currentWaypointIndex = 0;
            }
            StopPath(t, ET,endCam);
        }
    }

    int GetNextIndex(int index) {
        if (index == points.Count-1) return 0;
        return index + 1;
    }

    Vector3 GetBezierPosition(int pointIndex, float time) {
        if (!hasTIme) {
            float t = points[pointIndex].positionCurve.Evaluate(time);
            int nextIndex = GetNextIndex(pointIndex);

            return
                Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[pointIndex].position, points[pointIndex].position + points[pointIndex].handlenext, t), Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext, points[nextIndex].position + points[nextIndex].handleprev, t), t),
                Vector3.Lerp(Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext, points[nextIndex].position + points[nextIndex].handleprev, t),
                Vector3.Lerp(points[nextIndex].position + points[nextIndex].handleprev, points[nextIndex].position, t), t), t);
        }
        else {
            float t = points[pointIndex].positionCurve.Evaluate(time);
            int nextIndex = GetNextIndex(pointIndex);
            
            return
                Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[pointIndex].position, points[pointIndex].position + points[pointIndex].handlenext, t), Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext, points[nextIndex].position + points[nextIndex].handleprev, t), t),
                Vector3.Lerp(Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext, points[nextIndex].position + points[nextIndex].handleprev, t),
                Vector3.Lerp(points[nextIndex].position + points[nextIndex].handleprev, points[nextIndex].position, t), t), t);
        }
    }

    Quaternion GetLerpRotation(int pointIndex, float time) { return Quaternion.LerpUnclamped(points[pointIndex].rotation, points[GetNextIndex(pointIndex)].rotation, points[pointIndex].rotationCurve.Evaluate(time)); }

    public List<CPC_Point> GetPoints() { return points; }

    public bool IsPaused() { return paused; }

    public bool IsPlaying() { return playing; }

    public bool GetLooped() { return looped; }

    public int GetCurrentWayPoint() { return currentWaypointIndex; }

    public float GetCurrentTimeInWaypoint() { return currentTimeInWaypoint; }

    public void SetCurrentWayPoint(int value) { currentWaypointIndex = value; }

    public void SetCurrentTimeInWaypoint(float value) { currentTimeInWaypoint = value; }

#if UNITY_EDITOR
    public void OnDrawGizmos() {
        if (UnityEditor.Selection.activeGameObject == gameObject || alwaysShow) {
            if (points.Count >= 2) {
                for (int i = 0; i < points.Count; i++) {
                    if (i < points.Count - 1) {
                        var index = points[i];
                        var indexNext = points[i + 1];
                        UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handlenext,
                            indexNext.position + indexNext.handleprev,((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                    }
                    else if (looped) {
                        var index = points[i];
                        var indexNext = points[0];
                        UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handlenext,
                            indexNext.position + indexNext.handleprev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                    }
                }
            }

            for (int i = 0; i < points.Count; i++) {
                var index = points[i];
                Gizmos.matrix = Matrix4x4.TRS(index.position, index.rotation, Vector3.one);
                Gizmos.color = visual.frustrumColor;
                Gizmos.DrawFrustum(Vector3.zero, 90f, 0.25f, 0.01f, 1.78f);
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
#endif
}
