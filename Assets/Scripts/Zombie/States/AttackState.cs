using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState
{
    private ZombieAI controller;

    public AttackState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Attacking;

    }

    public override void Act(Transform player, Transform npc)
    {
        // hit player every some amount of time until they're dead.
    }

    public override void Reason(Transform player, Transform npc)
    {
        // player escaped
        // hit
        // killed.
    }
}
