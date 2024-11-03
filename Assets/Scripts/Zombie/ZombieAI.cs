using UnityEngine;

public abstract class AIProperties {
    public float speed = 3.0f;
    public float chaseDistance = 20;
}

[System.Serializable]
public class AttackAIProperties : AIProperties {

}

[System.Serializable]
public class DeadAIProperties : AIProperties {

}

[System.Serializable]
public class HuntAIProperties : AIProperties {

}

[System.Serializable]
public class InvestigateAIProperties : AIProperties {

}

[System.Serializable]
public class StunAIProperties : AIProperties {

}

public class ZombieAI : AdvancedFSM
{

    [SerializeField] private AttackAIProperties attackAIProperties;
    [SerializeField] private DeadAIProperties deadAIProperties;
    [SerializeField] private HuntAIProperties HuntAIProperties;
    [SerializeField] private InvestigateAIProperties investigateAIProperties;
    [SerializeField] private StunAIProperties stunAIProperties;

    private float health;
    public float Health {
        get {
            return health;
        }
    }

    private void ConstructFSM() {
        /* Create States */

        // Idle State
        IdleState idleState = new IdleState(this);

        idleState.AddTransition(Transition.NoiseHeard,      FSMStateID.Investigating);
        idleState.AddTransition(Transition.PlayerSpotted,   FSMStateID.Hunting);
        idleState.AddTransition(Transition.Hit,             FSMStateID.Stunned);
        idleState.AddTransition(Transition.Killed,          FSMStateID.Dead);

        // Investigate State
        InvestigateState investigateState = new InvestigateState(this);

        investigateState.AddTransition(Transition.NoiseReached, FSMStateID.Idle);
        investigateState.AddTransition(Transition.PlayerSpotted,FSMStateID.Hunting);
        investigateState.AddTransition(Transition.Hit,          FSMStateID.Stunned);
        investigateState.AddTransition(Transition.Killed,       FSMStateID.Dead);

        // Hunting State
        HuntState huntState = new HuntState(this);

        huntState.AddTransition(Transition.PlayerLost,      FSMStateID.Investigating);
        huntState.AddTransition(Transition.PlayerReached,   FSMStateID.Attacking);
        huntState.AddTransition(Transition.Hit,             FSMStateID.Stunned);
        huntState.AddTransition(Transition.Killed,          FSMStateID.Dead);

        // Attacking State
        AttackState attackState = new AttackState(this);

        attackState.AddTransition(Transition.PlayerEscaped, FSMStateID.Hunting);
        attackState.AddTransition(Transition.Hit,           FSMStateID.Stunned);
        attackState.AddTransition(Transition.Killed,        FSMStateID.Dead);

        // Stun State
        StunState stunState = new StunState(this);

        stunState.AddTransition(Transition.StunEnded, FSMStateID.Idle);
        stunState.AddTransition(Transition.Hit, FSMStateID.Stunned); // looping transition, to reset state (stun-locking).
        stunState.AddTransition(Transition.Killed, FSMStateID.Dead);

        // Dead State
        DeadState deadState = new DeadState(this);

        /* Add States to State List */
        AddFSMState(idleState);
        AddFSMState(investigateState);
        AddFSMState(huntState);
        AddFSMState(attackState);
        AddFSMState(stunState);
        AddFSMState(deadState);
    }
}
