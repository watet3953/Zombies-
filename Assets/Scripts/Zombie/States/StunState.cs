using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : FSMState
{
    private ZombieAI controller;

    public StunState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Stunned;

    }
    
    public override void Act(Transform player, Transform npc)
    {
        // tick down timer until state over? play animation.
    }

    public override void Reason(Transform player, Transform npc)
    {
        // hit (stun-lock)
        // killed
        // stun eneded.
    }
}
