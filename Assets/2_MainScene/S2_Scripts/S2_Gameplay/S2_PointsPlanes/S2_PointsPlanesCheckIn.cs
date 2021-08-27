//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using System.Collections.Generic;
using UnityEngine;

public class S2_PointsPlanesCheckIn : MonoBehaviour
{
    public static S2_PointsPlanesCheckIn Instance { get; protected set; }

    protected List<GameObject> pointsPlanesTransforms = new List<GameObject>();

    void Awake() { Instance = this; }

    public void CheckIn(GameObject objectTrasform) { pointsPlanesTransforms.Add(objectTrasform); }

    public void CheckOut(GameObject objectTransform) { pointsPlanesTransforms.Remove(objectTransform); }

    public List<GameObject> GetUpNext() { return pointsPlanesTransforms.GetRange(0, 3); }

    public List<GameObject> ReturnPointPlanes() { return pointsPlanesTransforms; }
}
