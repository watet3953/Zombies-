using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateState : FSMState
{
    private ZombieAI controller;

    Queue<Vector3> pathToPoints = new();

    public InvestigateState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Investigating;

    }
    

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.listener.HasNewSound(); // flush out the sound from a normal transition FIXME: ew
        Vector3? soundPos = controller.listener.GetTopSoundPosition();
        NavMeshHit navHit;
        NavMeshPath path = new();

        if (soundPos == null) return;

        if (!NavMesh.SamplePosition((Vector3)soundPos, out navHit, 10f, NavMesh.AllAreas)) return; // will get discarded as "point reached"

        if (!NavMesh.CalculatePath(npc.position,navHit.position,NavMesh.AllAreas, path)) return; // will get discarded as "point reached"
        // FIXME: just rewrite this with the navmesh agent, it'll work a lot nicer
        foreach (Vector3 point in path.corners) { // this relies on an async calculation (the navmesh), move to async await if becomes glitchy.
            pathToPoints.Enqueue(point);
        }

    }
    
    public override void Act(Transform player, Transform npc)
    {
        if (controller.nma.remainingDistance < 0.1f && pathToPoints.TryPeek(out Vector3 _)) {
            controller.nma.destination = pathToPoints.Dequeue(); // this is calculating navmesh twice, but whatever
        }
    }

    public override void Reason(Transform player, Transform npc)
    {
        // killed
        if (controller.IsDead) {
            controller.PerformTransition(Transition.Killed);
            return;
        }

        // hurt
        if (controller.healthDropped) {
            controller.healthDropped = false;
            controller.PerformTransition(Transition.Hit);
            return;
        }
        
        // Player Spotted
        if (Utils.IsPlayerVisible(player,npc, controller.maxVisionDistance, controller.VisionAngle)) {
            controller.PerformTransition(Transition.PlayerSpotted);
            return;
        }

        // Point Reached
        if (!pathToPoints.TryPeek(out Vector3 _)) { // no destination points
            controller.PerformTransition(Transition.NoiseReached);
            return;
        }

        // heard new sound?
        if (controller.listener.HasNewSound()) {
            controller.PerformTransition(Transition.NoiseHeard);
            return;
        }

        // Lost Interest
        //if (!controller.listener.HasSound()) {
        //    controller.PerformTransition(Transition.NoiseLost);
        //    return;
        //}
        
    }
}
