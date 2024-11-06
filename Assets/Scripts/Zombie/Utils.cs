using UnityEngine;

public static class Utils
{
    public static bool IsPlayerVisible(Transform player, Transform npc, float maxVisionDistance, float VisionAngle)
    {

        Vector3 playerPos = player.position + Vector3.up; // dirty fix for in-ground issues.
        Vector3 npcPos = npc.position + Vector3.up;

        Vector3 lookVector = playerPos - npcPos;
        //Debug.Log("LookVector: " + lookVector);
        if (lookVector.magnitude > maxVisionDistance)
        {
            //Debug.Log("Out of Range: " + lookVector.magnitude + " vs " + maxVisionDistance);
            return false;
        }


        if (Vector3.Angle(lookVector.normalized, npc.transform.forward) > VisionAngle)
        {
            //Debug.Log("Not In FOV: " + lookVector.normalized + " " + npc.transform.forward);
            return false;
        }

        //FIXME: put start of raycast at eye point to avoid half walls blocking view.
        if (Physics.Raycast(npcPos, lookVector.normalized, out RaycastHit hit)) // TODO: replace with linecast
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        //Debug.Log("Raycast Failed");
        return false;
    }
}
