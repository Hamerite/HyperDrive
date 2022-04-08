//Created by Kyle Ennis 15/09/21
//Last modified 15/09/21 (Dylan LeClair)
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
    public Vector3 position, handlenext, handleprev;
    public CPC_ECurveType curveTypePosition, curveTypeRotation;

    public AnimationCurve positionCurve, rotationCurve;

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

    [SerializeField] protected CPC_EAfterLoop afterLoop = CPC_EAfterLoop.Continue;

    [SerializeField] protected Camera selectedCamera;
    [SerializeField] protected Transform target;
    [SerializeField] protected List<CPC_Point> points = new List<CPC_Point>();

    [SerializeField] protected bool looped, alwaysShow;
    [SerializeField] protected float m_Timer;

    protected CPC_CameraPath localEndCam;

    protected float timePerSegment, localStayTime, localEndCamTime;

    public bool Playing { get; protected set; }
    public bool Paused { get; protected set; }
    public int CurrentWaypointIndex { get; set; }
    public float CurrentTimeInWaypoint { get; set; }

    public List<CPC_Point> GetPoints() { return points; }

    public bool GetLooped() { return looped; }

    public void SetPaused(bool status) {
        Paused = status;
        Playing = !status;
    }

    void Start () {
        for (int i = 0; i < points.Count; i++) {
            if (points[i].curveTypeRotation == CPC_ECurveType.EaseInAndOut) points[i].rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (points[i].curveTypeRotation == CPC_ECurveType.Linear) points[i].rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
            if (points[i].curveTypePosition == CPC_ECurveType.EaseInAndOut) points[i].positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (points[i].curveTypePosition == CPC_ECurveType.Linear) points[i].positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }

    public void PlayPath(float time, float stayTime, float endCamTime ,CPC_CameraPath endCam) {
        if (time <= 0) time = 0.001f;

        localEndCam = endCam;
        localStayTime = stayTime;
        localEndCamTime = endCamTime;

        SetPaused(false);
        StopAllCoroutines();
        StartCoroutine(FollowPath(time));
    }

    IEnumerator FollowPath(float time) {
        if (m_Timer <= 0) { UpdateTimeInSeconds(time); }
        else { UpdateTimeInSeconds(m_Timer); }

        CurrentWaypointIndex = 0;
         
        while (CurrentWaypointIndex < points.Count) {
            CurrentTimeInWaypoint = 0;
            while (CurrentTimeInWaypoint < 1) {
                if (!Paused) {
                    CurrentTimeInWaypoint += Time.deltaTime / timePerSegment;
                    RefreshTransform();
                }
                yield return 0;
            }
            ++CurrentWaypointIndex;
            if (CurrentWaypointIndex == points.Count - 1 && !looped) break;
            if (CurrentWaypointIndex == points.Count && afterLoop == CPC_EAfterLoop.Continue) CurrentWaypointIndex = 0;
        }
        Playing = false;
        Paused = false;
        Invoke(nameof(WaitForDecision), localStayTime);
    }

    public void UpdateTimeInSeconds(float seconds) {
        if (m_Timer > 0) { timePerSegment = seconds; }
        else { timePerSegment = seconds / ((looped) ? points.Count : points.Count - 1); }
    }

    public void RefreshTransform() {
        Camera.main.transform.position = GetBezierPosition(CurrentWaypointIndex, CurrentTimeInWaypoint);
       
        if (target == null) Camera.main.transform.rotation = GetLerpRotation(CurrentWaypointIndex, CurrentTimeInWaypoint);
        else Camera.main.transform.rotation = Quaternion.LookRotation((target.transform.position - Camera.main.transform.position).normalized);

        Vector3 GetBezierPosition(int pointIndex, float time) {
            float t = points[pointIndex].positionCurve.Evaluate(time);
            int nextIndex = GetNextIndex(pointIndex);

            return
                Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[pointIndex].position, points[pointIndex].position + points[pointIndex].handlenext, t), Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext, points[nextIndex].position + points[nextIndex].handleprev, t), t),
                Vector3.Lerp(Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext, points[nextIndex].position + points[nextIndex].handleprev, t),
                Vector3.Lerp(points[nextIndex].position + points[nextIndex].handleprev, points[nextIndex].position, t), t), t);
        }

        Quaternion GetLerpRotation(int pointIndex, float time) { return Quaternion.LerpUnclamped(points[pointIndex].rotation, points[GetNextIndex(pointIndex)].rotation, points[pointIndex].rotationCurve.Evaluate(time)); }

        int GetNextIndex(int index) {
            if (index == points.Count-1) return 0;
            return index + 1;
        }
    }

    void WaitForDecision() {
        if (localEndCam) {
            Camera.main.transform.SetPositionAndRotation(localEndCam.transform.position, localEndCam.transform.rotation);
            Invoke(nameof(EndPathing), localEndCamTime);
        }
        else { EndPathing(); }
    }

    void EndPathing() { StopAllCoroutines(); }

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
