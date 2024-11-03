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
    
    public override void Act(Transform player, Transform npc)
    {
        // move towards most notable sound, base notability off time since sound and volume of sound.
    }

    public override void Reason(Transform player, Transform npc)
    {
        // Point Reached
        // Player Spotted
        // Hurt
        // Killed
    }
}
