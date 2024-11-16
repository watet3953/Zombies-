using UnityEngine;

public class DeadState : FSMState
{
    private ZombieAI controller;

    public DeadState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Dead;

    }

    float timeToDespawn;

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.ResetTrigger("isMoving");
        controller.animator.ResetTrigger("isAttacking");
        controller.animator.SetTrigger("isDamaged");
        controller.animator.ResetTrigger("isIdle");
        
        if (controller.debugText != null) controller.debugText.text = "Dead";
        controller.nma.enabled = false; // hacky way of disabling
        controller.GetComponent<Collider>().enabled = false;
        controller.listener.enabled = false;
        timeToDespawn = 5.0f;
    }

    public override void Act(Transform player, Transform npc)
    {
        timeToDespawn -= Time.deltaTime;
        while (timeToDespawn > 0.0) return;
        // hide the zombie, move it back to object pool.
        controller.enabled = false;
        MapManager.Instance.pool.ReturnToPool(controller);
    }

    public override void Reason(Transform player, Transform npc)
    {
        // nada
    }
}
