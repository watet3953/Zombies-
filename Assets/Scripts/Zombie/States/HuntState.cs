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
    
    public override void Act(Transform player, Transform npc)
    {
        // navigate towards player quickly, swap to attacking state if at barricade as well (pathing should be handled by navmesh)
    }

    public override void Reason(Transform player, Transform npc)
    {
        // Player Lost
        // Player Reached
        // Hurt
        // Killed
    }
}
