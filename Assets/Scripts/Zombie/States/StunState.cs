using UnityEngine;

public class StunState : FSMState
{
    private ZombieAI controller;

    private float stunTimeLeft = 0.0f;



    public StunState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Stunned;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.SetTrigger("isDamaged");
        if (controller.debugText != null) controller.debugText.text = "Stunned";
        controller.nma.destination = controller.transform.position;
        stunTimeLeft = StunAIProperties.stunTime;
    }

    public override void Act(Transform player, Transform npc)
    {
        // tick down timer until state over? play animation.
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
