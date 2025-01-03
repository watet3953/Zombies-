using UnityEngine;

public class StunState : FSMState
{
    private ZombieAI controller;

    private StunAIProperties properties;

    private float stunTimeLeft = 0.0f;



    public StunState(ZombieAI controller, StunAIProperties properties)
    {
        this.controller = controller;
        stateID = FSMStateID.Stunned;
        this.properties = properties;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.ResetTrigger("isMoving");
        controller.animator.ResetTrigger("isAttacking");
        controller.animator.SetTrigger("isDamaged");
        controller.animator.ResetTrigger("isIdle");

        if (controller.debugText != null) controller.debugText.text = "Stunned";
        controller.nma.enabled = false;
        controller.listener.enabled = true;
        stunTimeLeft = properties.stunTime;
    }

    public override void Act(Transform player, Transform npc)
    {
        // tick down timer until state over
        stunTimeLeft -= Time.deltaTime;
    }

    public override void Reason(Transform player, Transform npc)
    {
        // killed
        if (controller.IsDead)
        {
            controller.PerformTransition(Transition.Killed);
            return;
        }

        // hit (stun-lock)
        if (controller.healthDropped)
        {
            controller.healthDropped = false;
            controller.PerformTransition(Transition.Hit);
            return;
        }

        // stun ended.
        if (stunTimeLeft <= 0.0f)
        {
            controller.PerformTransition(Transition.StunEnded);
            return;
        }
    }
}
