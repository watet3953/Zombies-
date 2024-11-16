using UnityEngine;

public class HuntState : FSMState
{
    private ZombieAI controller;
    private AIProperties properties;

    public HuntState(ZombieAI controller, AIProperties properties)
    {
        this.controller = controller;
        stateID = FSMStateID.Hunting;
        this.properties = properties;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.SetTrigger("isMoving");
        controller.animator.ResetTrigger("isAttacking");
        controller.animator.ResetTrigger("isDamaged");
        controller.animator.ResetTrigger("isIdle");

        controller.nma.enabled = true;
        controller.listener.enabled = true;
        if (controller.debugText != null) controller.debugText.text = "Hunting";
        controller.nma.destination = player.position;
    }

    public override void Act(Transform player, Transform npc)
    {
        if (Utils.IsPlayerVisible(player, npc, properties.maxVisionDistance, properties.VisionAngle))
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
        if (!Utils.IsPlayerVisible(player, npc, properties.maxVisionDistance, properties.VisionAngle))
        {
            controller.PerformTransition(Transition.PlayerLost);
            return;
        }

        // Player Reached
        if (Utils.IsPlayerVisible(player, npc, properties.attackDistance - 0.2f, Mathf.PI * 2))
        { // is the player visible within attack distance (360 degree cone of vision)
            controller.PerformTransition(Transition.PlayerReached);
            return;
        }
    }
}
