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
        controller.animator.SetTrigger("isDamaged");
        if (controller.debugText != null) controller.debugText.text = "Dead";
        controller.nma.destination = controller.transform.position; // hacky way of disabling
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
