using UnityEngine;

public class IdleState : FSMState
{
    private ZombieAI controller;

    public IdleState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Idle;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.SetTrigger("isIdle");
        if (controller.debugText != null) controller.debugText.text = "Idle";
        controller.nma.destination = controller.transform.position;
    }

    public override void Act(Transform player, Transform npc)
    {
        // blank, maybe an animation? i dunno
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
        if (Utils.IsPlayerVisible(player, npc, controller.maxVisionDistance, controller.VisionAngle))
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
