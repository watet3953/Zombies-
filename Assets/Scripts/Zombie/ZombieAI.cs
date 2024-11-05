using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public static float stunTime = 2.0f;
}

public class ZombieAI : AdvancedFSM
{

    [SerializeField] private AttackAIProperties attackAIProperties;
    [SerializeField] private DeadAIProperties deadAIProperties;
    [SerializeField] private HuntAIProperties HuntAIProperties;
    [SerializeField] private InvestigateAIProperties investigateAIProperties;
    [SerializeField] private StunAIProperties stunAIProperties;


    public readonly float VisionAngle = Mathf.Cos(90f * Mathf.Deg2Rad);
    public float maxVisionDistance = 50.0f;

    public float attackDistance = 1.0f;

    public PlayerBody player;

    public HearingListener listener;
    
    public NavMeshAgent nma;

    /// <summary> Has the health-dropped been resolved yet? (stun) </summary>
    [HideInInspector] public bool healthDropped = false;

    /// <summary> Is the zombie dead? </summary>
    public bool IsDead {private set; get;} = false;
    private float health = 50f;
    public float Health {
        get {
            return health;
        }
        set {
            healthDropped = (health > value); // health gone down.
            health = value;
            IsDead = (health <= 0.0f); // if health less than 0 then die.
        }
    }

    protected override void Initialize() {
        listener = GetComponent<HearingListener>();
        ConstructFSM();
    }

    protected override void FSMUpdate()
    {
        CurrentState.Act(player.transform, transform);
        CurrentState.Reason(player.transform, transform);
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
        investigateState.AddTransition(Transition.NoiseLost,    FSMStateID.Idle);
        investigateState.AddTransition(Transition.NoiseHeard,   FSMStateID.Investigating); // looping transition, to reset state (go look at new sound.)
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
