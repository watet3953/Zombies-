using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsPlayerVisible(Transform player, Transform npc, float maxVisionDistance, float VisionAngle) {
        Vector3 lookVector = player.position - npc.position;

        if (lookVector.magnitude > maxVisionDistance) return false;

        lookVector = lookVector.normalized;

        if (Vector3.Dot(lookVector, npc.transform.forward) < VisionAngle) return false;

        //FIXME: put start of raycast at eye point to avoid half walls blocking view.
        if (Physics.Raycast(npc.position, lookVector, out RaycastHit hit)) {
            if (hit.collider.CompareTag("Player")) {
                return true;
            }
        }
        return false;
    }
}
