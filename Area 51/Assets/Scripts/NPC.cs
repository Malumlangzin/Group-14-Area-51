using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private LayerMask groundLayer;

    [Header("Patrol Settings")]
    [SerializeField] private float walkRange = 10f;
    [SerializeField] private float reachThreshold = 1f;

    private Vector3 walkPoint;
    private bool walkPointSet;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 12f;
        agent.stoppingDistance = 0f;
        agent.autoBraking = false;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            if (Vector3.Distance(transform.position, walkPoint) < reachThreshold)
            {
                walkPointSet = false;
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkRange, walkRange);
        float randomX = Random.Range(-walkRange, walkRange);

        Vector3 potentialPoint = transform.position + new Vector3(randomX, 0, randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(potentialPoint, out hit, 2f, NavMesh.AllAreas))
        {
              walkPoint = hit.position;
              walkPointSet = true;
   
        }
    }
}
