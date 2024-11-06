using UnityEngine;

public class HuntState : FSMState
{
    private ZombieAI controller;

    public HuntState(ZombieAI controller)
    {
        this.controller = controller;
        stateID = FSMStateID.Hunting;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.SetTrigger("isMoving");
        controller.animator.ResetTrigger("isAttacking");
        controller.animator.ResetTrigger("isDamaged");
        controller.animator.ResetTrigger("isIdle");

        if (controller.debugText != null) controller.debugText.text = "Hunting";
        controller.nma.destination = player.position;
    }

    public override void Act(Transform player, Transform npc)
    {
        if (Utils.IsPlayerVisible(player, npc, controller.maxVisionDistance, controller.VisionAngle))
        {
            controller.nma.destination = player.position;
        }
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

        // Player Lost
        if (!Utils.IsPlayerVisible(player, npc, controller.maxVisionDistance, controller.VisionAngle))
        {
            controller.PerformTransition(Transition.PlayerLost);
            return;
        }

        // Player Reached
        if (Utils.IsPlayerVisible(player, npc, controller.attackDistance - 0.2f, Mathf.PI * 2))
        { // is the player visible within attack distance (360 degree cone of vision)
            controller.PerformTransition(Transition.PlayerReached);
            return;
        }
    }
}
