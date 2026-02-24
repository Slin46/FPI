using UnityEngine;
using UnityEngine.AI;

public class EnemyRadar : MonoBehaviour
{
    [Header("Scanner & Player")]
    public ScannerRadar scanner;
    public Transform scannerTransform;

    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Patrol Settings")]
    public Vector3 boundaryMin;
    public Vector3 boundaryMax;
    public float waitTimeAtPoint = 2f;
    public float patrolSpeed = 3.5f;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // where enemy respawns after 4th picture

    private Vector3 targetPatrolPoint;
    private float waitTimer;
    private bool isInvestigating;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        ChooseNewPatrolPoint();
    }

    void Update()
    {
        float alert = scanner.currentAlertLevel;

        // Check for investigation
        if (alert >= 2) // orange or red
        {
            InvestigateSound();
        }
        else if (alert <= 1 && isInvestigating)
        {
            StopInvestigating();
        }

        // Patrol when not investigating
        if (!isInvestigating)
        {
            Patrol();
        }

        
    }

  
    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                ChooseNewPatrolPoint();
                waitTimer = 0f;
            }
        }
    }

    void ChooseNewPatrolPoint()
    {
        // Random point inside boundaries
        float x = Random.Range(boundaryMin.x, boundaryMax.x);
        float z = Random.Range(boundaryMin.z, boundaryMax.z);
        float y = transform.position.y;

        Vector3 randomPoint = new Vector3(x, y, z);

        // Make sure the point is on NavMesh
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            targetPatrolPoint = hit.position;
            agent.SetDestination(targetPatrolPoint);
        }
    }
  
    void InvestigateSound()
    {
        isInvestigating = true;
        agent.SetDestination(scannerTransform.position);
    }

    void StopInvestigating()
    {
        isInvestigating = false;
        ChooseNewPatrolPoint();
    }
    
    void Respawn()
    {
        agent.Warp(spawnPoint.position); // teleport to spawn
        isInvestigating = false;
        ChooseNewPatrolPoint();
    }
  
    // Optional: visualize boundaries in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = (boundaryMin + boundaryMax) / 2f;
        Vector3 size = boundaryMax - boundaryMin;
        Gizmos.DrawWireCube(center, size);
    }
}
