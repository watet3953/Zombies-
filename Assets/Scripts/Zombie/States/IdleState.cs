using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
    private ZombieAI controller;

    public IdleState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Idle;

    }

    public override void Act(Transform player, Transform npc)
    {
        // blank, maybe an animation? i dunno
    }

    public override void Reason(Transform player, Transform npc)
    {
        // noise heard
        // player spotted
        // hit
        // killed
    }
}
