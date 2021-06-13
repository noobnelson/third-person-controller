using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingCheck : MonoBehaviour
{
    internal float minPosY;
    internal float maxPosY;
    internal float minPosX;
    internal float maxPosX;
    internal float minPosZ;
    internal float maxPosZ;

    [SerializeField]
    private List<int> checkLayers = new List<int>();
    private int combinedMask;

    [Header("Boundary adjustments")]
    [SerializeField]
    private float xLimitOffset = 1f;
    [SerializeField]
    private float yLimitOffset;
    [SerializeField]
    private float zLimitOffset = 1f;

    [Header("Position to raycast")]
    [SerializeField]
    private Vector3 checkPositionOffset;
    private Vector3 checkPosition;
    
    private float maxPositive = 9999f;
    private float maxNegative = -9999f;
    private float limitCheckDistance = 99f;
    
    void Awake()
    {
        foreach (int layer in checkLayers)
        {
            int mask = 1 << layer;
            combinedMask = combinedMask | mask;
        }
    }

    void Update()
    {        
        checkPosition = transform.position + checkPositionOffset;

        maxPosZ = LimitCheck(Vector3.forward, 2) - zLimitOffset;
        minPosZ = LimitCheck(Vector3.back, 2) + zLimitOffset;

        maxPosY = LimitCheck(Vector3.up, 1) - yLimitOffset;
        minPosY = LimitCheck(Vector3.down, 1) + yLimitOffset;

        maxPosX = LimitCheck(Vector3.right, 0) - xLimitOffset;
        minPosX = LimitCheck(Vector3.left, 0) + xLimitOffset;
    }

    float LimitCheck(Vector3 direction, int axis)
    {
        RaycastHit hit;
        if (Physics.Raycast(checkPosition, direction, out hit, limitCheckDistance, combinedMask))
        {
            return hit.point[axis];
        }
        else
        {
            if (direction[axis] == 1) // 1 means positive direction ie. Vector3(0, 1, 0) goes up 
            {
                return maxPositive;
            }
            else
            {
                return maxNegative;
            }
        }
    }

    public Vector3 PositionWithinBoundaries()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minPosX, maxPosX);
        float clampedY = Mathf.Clamp(transform.position.y, minPosY, maxPosY);
        float clampedZ = Mathf.Clamp(transform.position.z, minPosZ, maxPosZ);
        return new Vector3(clampedX, clampedY, clampedZ);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 xStart = new Vector3(maxPosX, checkPosition.y, checkPosition.z);
        Vector3 xEnd = new Vector3(minPosX, checkPosition.y, checkPosition.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(xStart, xEnd);

        Vector3 yStart = new Vector3(checkPosition.x, maxPosY, checkPosition.z);
        Vector3 yEnd = new Vector3(checkPosition.x, minPosY, checkPosition.z);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(yStart, yEnd);

        Vector3 zStart = new Vector3(checkPosition.x, checkPosition.y, maxPosZ);
        Vector3 zEnd = new Vector3(checkPosition.x, checkPosition.y, minPosZ);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(zStart, zEnd);
    }
}
