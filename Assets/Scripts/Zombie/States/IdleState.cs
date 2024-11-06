using UnityEngine;

public class IdleState : FSMState
{
    private ZombieAI controller;
    private AIProperties properties;

    public IdleState(ZombieAI controller, AIProperties properties)
    {
        this.controller = controller;
        stateID = FSMStateID.Idle;
        this.properties = properties;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.ResetTrigger("isMoving");
        controller.animator.ResetTrigger("isAttacking");
        controller.animator.ResetTrigger("isDamaged");
        controller.animator.SetTrigger("isIdle");
        if (controller.debugText != null) controller.debugText.text = "Idle";
        controller.nma.destination = controller.transform.position;
    }

    public override void Act(Transform player, Transform npc)
    {
        // no action needed, idle state.
    }

    public override void Reason(Transform player, Transform npc)
    {
        // killed
        if (controller.IsDead)
        {
            controller.PerformTransition(Transition.Killed);
            return;
        }

        // hurt
        if (controller.healthDropped)
        {
            controller.healthDropped = false;
            controller.PerformTransition(Transition.Hit);
            return;
        }

        // player spotted
        if (Utils.IsPlayerVisible(player, npc, properties.maxVisionDistance, properties.VisionAngle))
        {
            controller.PerformTransition(Transition.PlayerSpotted);
            return;
        }

        // noise heard
        if (controller.listener.HasSound())
        {
            controller.PerformTransition(Transition.NoiseHeard);
            return;
        }

    }
}
