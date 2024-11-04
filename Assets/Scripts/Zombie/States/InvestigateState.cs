using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : FSMState
{
    private ZombieAI controller;

    public InvestigateState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Investigating;

    }
    

    public override void EnterStateInit(Transform player, Transform npc)
    {
        
    }
    
    public override void Act(Transform player, Transform npc)
    {
        // move towards most notable sound, base notability off time since sound and volume of sound.
    }

    public override void Reason(Transform player, Transform npc)
    {
        // killed
        if (controller.dead) {
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
        
    }
}
