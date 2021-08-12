//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using System.Collections.Generic;
using UnityEngine;

public class S2_PointsPlanesCheckIn : MonoBehaviour
{
    public static S2_PointsPlanesCheckIn Instance { get; protected set; }

    protected List<Vector3> pointsPlanesTransforms = new List<Vector3>();

    void Awake() { Instance = this; }

    public void CheckIn(Vector3 objectTrasform) { pointsPlanesTransforms.Add(objectTrasform); }

    public void CheckOut(Vector3 objectTransform) { pointsPlanesTransforms.Remove(objectTransform); }

    public List<Vector3> GetUpNext() { return pointsPlanesTransforms.GetRange(0, 3); }

    public List<Vector3> ReturnPointPlanes() { return pointsPlanesTransforms; }
}
