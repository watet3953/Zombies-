using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateState : FSMState
{
    private ZombieAI controller;
    private AIProperties properties;

    public InvestigateState(ZombieAI controller, AIProperties properties)
    {
        this.controller = controller;
        stateID = FSMStateID.Investigating;
        this.properties = properties;
    }


    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.SetTrigger("isMoving");
        controller.animator.ResetTrigger("isAttacking");
        controller.animator.ResetTrigger("isDamaged");
        controller.animator.ResetTrigger("isIdle");


        controller.nma.enabled = true;
        controller.listener.enabled = true;
        if (controller.debugText != null) controller.debugText.text = "Investigating";
        controller.listener.HasNewSound(); // flush out the sound from a normal transition FIXME: ew
        Vector3? soundPos = controller.listener.GetTopSoundPosition();

        if (soundPos == null) return;

        if (!NavMesh.SamplePosition((Vector3)soundPos, out NavMeshHit navHit, 10f, NavMesh.AllAreas)) return; // will get discarded as "point reached"

        controller.nma.speed = properties.speed;
        controller.nma.destination = navHit.position;

    }

    public override void Act(Transform player, Transform npc)
    {
        // navmesh movement handled by agent
    }

    public override void Reason(Transform player, Transform npc)
    {
        // killed
        if (controller.IsDead)
        {
            controller.PerformTransition(Transition.Killed);
            return;
        }

        // hurt
        if (controller.healthDropped)
        {
            controller.healthDropped = false;
            controller.PerformTransition(Transition.Hit);
            return;
        }

        // Player Spotted
        if (Utils.IsPlayerVisible(player, npc, properties.maxVisionDistance, properties.VisionAngle))
        {
            controller.PerformTransition(Transition.PlayerSpotted);
            return;
        }

        // Point Reached
        if ((controller.transform.position - controller.nma.destination).magnitude < 0.2f)
        {
            controller.PerformTransition(Transition.NoiseReached);
            return;
        }

        // heard new sound
        if (controller.listener.HasNewSound())
        {
            controller.PerformTransition(Transition.NoiseHeard);
            return;
        }

    }
}
