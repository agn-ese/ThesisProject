using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public static class DemoConeCasting
{
    private static bool IsPartiallyVisible(RaycastHit hit, Vector3 origin)  //Checks if the target is partially visible
    {
        int counterPointsObstructed = 0;
        Vector3[] pointsToCheck = new Vector3[]
        {
            hit.collider.bounds.center,                     // Center
            hit.collider.bounds.min,                        // Bottom-left corner
            hit.collider.bounds.max,                        // Top-right corner
            new Vector3(hit.collider.bounds.min.x, hit.collider.bounds.min.y, hit.collider.bounds.max.z), // Other corners
            new Vector3(hit.collider.bounds.min.x, hit.collider.bounds.max.y, hit.collider.bounds.min.z),
            new Vector3(hit.collider.bounds.max.x, hit.collider.bounds.min.y, hit.collider.bounds.min.z),
            new Vector3(hit.collider.bounds.max.x, hit.collider.bounds.max.y, hit.collider.bounds.min.z),
            new Vector3(hit.collider.bounds.max.x, hit.collider.bounds.max.y, hit.collider.bounds.max.z)
        };

        int usefulLayerMask = LayerMask.GetMask("Useful");
        foreach (Vector3 point in pointsToCheck)
        {
            Vector3 directionToPoint = point - origin;
            RaycastHit obstructionHit;

            if(Physics.Raycast(origin, directionToPoint, out obstructionHit, directionToPoint.magnitude))
            {
                if (obstructionHit.collider.gameObject != hit.collider.gameObject && obstructionHit.transform.gameObject.layer != usefulLayerMask)
                {
                    counterPointsObstructed++;
                }
            }
        }
        if (counterPointsObstructed == 8)
            return false;
        else
            return true;
        
    }

    public static RaycastHit[] coneCast(this Physics physics, Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, LayerMask layer)
    {

        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, layer);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();
        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {

                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 dirToHit = hitPoint - origin;
                float angleToHit = Vector3.Angle(direction, dirToHit);
                if ((angleToHit < coneAngle && IsPartiallyVisible(sphereCastHits[i], origin)) || (IsPartiallyVisible(sphereCastHits[i], origin) && isNear(origin, hitPoint, maxDistance)))
                {
                    coneCastHitList.Add(sphereCastHits[i]);
                }
            }
        }

        float horizontalStep = 60 /*degrees angle*/ / (10 /*rays for horizontal*/ - 1);
        float verticalStep = 60 / (10 - 1);

        float halfHorizontalRange = 60 / 2f;
        float halfVerticalRange = 60 / 2f;
        float rayDistance = maxRadius * 2 + Vector3.Distance(Camera.main.transform.position, origin) + 2;

        RaycastHit[] hits = new RaycastHit[20];
        for (int v = 0; v < 10; v++)
        {
            float verticalAngle = -halfVerticalRange + (v * verticalStep);

            for (int h = 0; h < 10; h++)
            {
                float horizontalAngle = -halfHorizontalRange + (h * horizontalStep);

                Vector3 rayDirection = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * direction;
                int hitCount = Physics.RaycastNonAlloc(Camera.main.transform.position, rayDirection, hits, rayDistance, layer);

                for (int i = 0; i < hitCount; i++)
                {
                    coneCastHitList.Add(hits[i]);
                }

            }
        }


        RaycastHit[] coneCastHits = coneCastHitList.ToArray();
        return coneCastHits;
    }

    public static bool isNear(Vector3 origin, Vector3 hitpoint, float maxDist) //to be sure that when the object is near, it is still considered found
    {
        bool condition = false;
        if(Vector3.Distance(origin, hitpoint) <= maxDist)
        {
            condition = true;
        }
        return condition;
    }

}
