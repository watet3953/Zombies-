using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : FSMState
{
    private ZombieAI controller;

    public DeadState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Dead;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        // hide the zombie, move it back to object pool.
    }

    public override void Act(Transform player, Transform npc)
    {
        // dead, notify object pooling to remove & reset zombie.
    }

    public override void Reason(Transform player, Transform npc)
    {
        // nada
    }
}
