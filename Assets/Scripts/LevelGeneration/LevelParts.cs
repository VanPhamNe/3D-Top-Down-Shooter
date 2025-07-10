using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParts : MonoBehaviour
{
    [Header("Intersection Check")]
    [SerializeField] private LayerMask intersection;
    [SerializeField] private Collider[] intersectionCheckCollider;
    [SerializeField] private Transform intersectionCheckParent;

    public bool IntersectionDetect()
    {
        Physics.SyncTransforms(); // Ensure all transforms are updated before checking for intersection
        foreach (var collider in intersectionCheckCollider)
        {
            Collider[] hitCollider = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, intersection);
            foreach(var hit in hitCollider)
            {
                IntersectionCheck intersectionCheck = hit.GetComponentInParent<IntersectionCheck>();
                if (intersectionCheck !=null && intersectionCheckParent != intersectionCheck.transform)
                {
                    return true;                    
                }
            }
        }
        return false;
    }
    private SnapPoint GetSnapPoint(SnapPointType type)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredPoints = new List<SnapPoint>(); //Loc cac snap point theo type
        foreach (SnapPoint snapPoint in snapPoints) // Duyet qua tung phan tu cua snapPoints neu co cung type thi add vao List filter moi
        {
            if (snapPoint.pointType == type)
            {
                filteredPoints.Add(snapPoint);
            }
        }
        if (filteredPoints.Count > 0)  
        {
            int randomIndex = Random.Range(0, filteredPoints.Count); // Chon ngau nhien 1 snap point trong list filteredPoints
            return filteredPoints[randomIndex];
        }
        return null;
    }
    public SnapPoint GetEnterSnapPoint()
    {
        return GetSnapPoint(SnapPointType.Enter);
    }
    public SnapPoint GetExitSnapPoint()
    {
        return GetSnapPoint(SnapPointType.Exit);
    }
    private void SnapTo(SnapPoint ownSnapPoint,SnapPoint targetSnapPoint)
    {
        var offset= transform.position - ownSnapPoint.transform.position;
        var newPosition = targetSnapPoint.transform.position + offset;
        transform.position = newPosition;
    }
    public void SnapAndAlignPart(SnapPoint targetSnapPoint)
    {
        SnapPoint enterPoint = GetEnterSnapPoint();
        AlignTo(enterPoint, targetSnapPoint);
        SnapTo(enterPoint, targetSnapPoint);
  
    }
    private void AlignTo(SnapPoint ownSnapPoint,SnapPoint targetSnapPoint)
    {
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        transform.rotation = targetSnapPoint.transform.rotation;
        transform.Rotate(0, 180, 0);
        transform.Rotate(0, -rotationOffset, 0);
    }
}
