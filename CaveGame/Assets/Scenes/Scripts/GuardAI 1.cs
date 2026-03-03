using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        Chase,
        Stalk
    }

    [Header("References")]
    public Transform player;
    public Transform[] patrolPoints;
    public NightVisionCam camScript;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float losePlayerRange = 15f;
    public float soundRange = 25f; //if player flashes camera and AI is with range, stalk will start

    [Header("Patrol")]
    public float waypointTolerance = 0.5f;

    private NavMeshAgent agent;
    private AIState currentState;
    private int currentPatrolIndex = 0;

    [Header("Traits")]
    public float patrolSpeed = 4f;
    public float stalkSpeed = 2f;
    public float chaseSpeed = 8f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(AIState.Patrol);
        camScript = GetComponent<NightVisionCam>();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrol();
                break;

            case AIState.Chase:
                UpdateChase();
                break;

            case AIState.Stalk:
                UpdateStalk();
                break;
        }
    }

  
    // STATE LOGIC
    

    void UpdatePatrol()
    {
        if (CanSeePlayer())
        {
            ChangeState(AIState.Chase);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            GoToNextPatrolPoint();
        }
        if (camScript.cameraFlash && DistanceToPlayer() <= soundRange)
        {
            ChangeState(AIState.Stalk);
        }
    }

    void UpdateChase()
    {
        agent.SetDestination(player.position);

        if (!CanSeePlayer() && DistanceToPlayer() > losePlayerRange)
        {
            ChangeState(AIState.Patrol);
        }
    }

    void UpdateStalk()
    {
        //agent.SetDestination(player.position);
        //follow player slowly instead of random patrol
        //if player gets out of range go back to patroling 
        //if players gets withing vision go to chase
       
        if (!CanSeePlayer() && DistanceToPlayer() > losePlayerRange)
        {
            ChangeState(AIState.Patrol);
        }
        if (CanSeePlayer())
        {
            ChangeState(AIState.Chase);
            return;
        }

    }
    // STATE TRANSITIONS
   

    void ChangeState(AIState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case AIState.Patrol:
                GoToNextPatrolPoint();
                break;

            case AIState.Chase:
                break;

            case AIState.Stalk:
                break;
        }
    }

    // PATROL LOGIC


    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // ------------------------
    // PERCEPTION
    // ------------------------

    bool CanSeePlayer()
    {
        return DistanceToPlayer() <= detectionRange;
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
}