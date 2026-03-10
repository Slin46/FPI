using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
    public NightVisionCam camScript;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float losePlayerRange = 15f;
    public float soundRange = 25f; //if player flashes camera and AI is with range, stalk will start

    [Header("Patrol")]
    public float waypointTolerance = 1f;
    public float patrolRadius = 15f;

    private NavMeshAgent agent;
    private AIState currentState;
    

    [Header("Traits")]
    public float patrolSpeed = 4f;
    public float stalkSpeed = 2f;
    public float chaseSpeed = 8f;

    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float wanderMinDistance = 4f;
    public float idleTime = 2f;

    private float idleTimer;
    private Vector3 lastDestination;
    public Animator anim;

    [Header("Bools")]
    //public bool isIdle = false;
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isRoar = false;

    [Header("Roar Settings")]
    private float roarDuration = 4f;
    private bool isRoaring = false;
    public AudioSource roarSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(AIState.Patrol);
        anim = GetComponent<Animator>();
        //GameObject Ncam = GameObject.FindGameObjectWithTag("Camera");
        //NightVisionCam camScript = Ncam.GetComponent<NightVisionCam>();
    }

    void Update()
    {
        if (isRoaring)
            return;
        switch (currentState)
        {
            case AIState.Patrol:
                //Debug.Log("In patrol");
                UpdatePatrol();
                break;

            case AIState.Chase:
                UpdateChase();
                //Debug.Log("In Chase");
                break;

            case AIState.Stalk:
                UpdateStalk();
                Debug.Log("In Stalk");
                break;
        }
    }

  
    // STATE LOGIC
    

    void UpdatePatrol()
    {
        if (CanSeePlayer() && !isRoaring)
        {
            StartCoroutine(RoarThenChase());
            return;
        }

        if (camScript.cameraFlash && Vector3.Distance(transform.position, camScript.flashPosition) <= soundRange)
        {
            ChangeState(AIState.Stalk);
            return;
        }
        //if destination is reached pick a new random one
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool("isWalking", false);
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                Wander();
                idleTimer = idleTime;
            }

        }
    }
    IEnumerator RoarThenChase()
    {
        isRoaring = true;
        agent.isStopped = true;

        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isRoar", true);

        roarSound.Play();

        yield return new WaitForSeconds(roarDuration);
        anim.SetBool("isRoar", false);
        agent.isStopped = false;

        ChangeState(AIState.Chase);
        isRoaring = false;
    }
    void Wander()
    {
        anim.SetBool("isWalking", true);
        for (int i = 0; i < 15; i++)
        {
            // Bias movement forward instead of fully random
            Vector3 randomDirection = transform.forward * Random.Range(2f, wanderRadius);
            randomDirection += Random.insideUnitSphere * wanderRadius * 0.5f;
            randomDirection.y = 0;

            Vector3 candidate = transform.position + randomDirection;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(candidate, out hit, wanderRadius, NavMesh.AllAreas))
            {
                float distance = Vector3.Distance(transform.position, hit.position);

                // Avoid very close points
                if (distance < wanderMinDistance)
                    continue;

                // Avoid going back to last location
                if (Vector3.Distance(hit.position, lastDestination) < wanderMinDistance)
                    continue;

                agent.SetDestination(hit.position);
                lastDestination = hit.position;
                return;
            }
        }
    }
   
    void UpdateChase()
    {
        anim.SetBool("isRunning", true);
        anim.SetBool("isWalking", false);
        agent.SetDestination(player.position);

        if (!CanSeePlayer() && DistanceToPlayer() > losePlayerRange)
        {
            anim.SetBool("isWalking", true);
            ChangeState(AIState.Patrol);
        }
    }

    void UpdateStalk()
    {
        //agent.SetDestination(player.position);
        //follow player slowly instead of random patrol
        //if player gets out of range go back to patroling 
        //if players gets withing vision go to chase
        agent.SetDestination(camScript.flashPosition);

        if (CanSeePlayer())
        {
            ChangeState(AIState.Chase);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeState(AIState.Patrol);
        }

    }
    // STATE TRANSITIONS
   

    void ChangeState(AIState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case AIState.Patrol:
                agent.speed = patrolSpeed;
                idleTimer = 0f;
                Wander();
                break;

            case AIState.Chase:
                agent.speed = chaseSpeed;
                break;

            case AIState.Stalk:
                agent.speed = stalkSpeed;
                break;
        }
    }

    bool CanSeePlayer()
    {
        return DistanceToPlayer() <= detectionRange;
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //freeze player
            //turn to face monster 
            //play monster roar
            Debug.Log("Player caught, loading end screen");
            //Load EndScreen
            SceneManager.LoadScene("EndScreen");

        }
    }
}