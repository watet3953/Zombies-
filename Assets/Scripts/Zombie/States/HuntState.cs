using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : FSMState
{
    private ZombieAI controller;

    public HuntState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Hunting;

    }
    
    public override void EnterStateInit(Transform player, Transform npc)
    {
        
    }
    
    public override void Act(Transform player, Transform npc)
    {
        // navigate towards player quickly, swap to attacking state if at barricade as well (pathing should be handled by navmesh)
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
        
        // Player Lost
        if (!Utils.IsPlayerVisible(player,npc, controller.maxVisionDistance, controller.VisionAngle)) {
            controller.PerformTransition(Transition.PlayerLost);
            return;
        }

        // Player Reached
        
    }
}
