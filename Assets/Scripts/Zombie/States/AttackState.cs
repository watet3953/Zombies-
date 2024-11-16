using UnityEngine;

public class AttackState : FSMState
{
    private ZombieAI controller;

    private AIProperties properties;

    public AttackState(ZombieAI controller, AIProperties properties)
    {
        this.controller = controller;
        stateID = FSMStateID.Attacking;
        this.properties = properties;

    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        controller.animator.ResetTrigger("isMoving");
        controller.animator.SetTrigger("isAttacking");
        controller.animator.ResetTrigger("isDamaged");
        controller.animator.ResetTrigger("isIdle");

        controller.nma.enabled = true;
        controller.listener.enabled = true;
        if (controller.debugText != null) controller.debugText.text = "Attacking";
        controller.nma.destination = controller.transform.position;
    }

    public override void Act(Transform player, Transform npc)
    {
        controller.transform.LookAt(player.transform);
        // hit player every some amount of time until they're dead.
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

        // player escaped
        if (!Utils.IsPlayerVisible(player, npc, properties.attackDistance, 360.0f))
        { // is the player visible within attack distance (360 degree cone of vision)
            controller.PerformTransition(Transition.PlayerEscaped);
            return;
        }
    }
}
