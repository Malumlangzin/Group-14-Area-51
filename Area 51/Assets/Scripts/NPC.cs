using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] float walkRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();  
    }

    void Patrol()
    {         if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
  
        if (distanceToWalkPoint.magnitude < 5f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkRange, walkRange);
        float randomX = Random.Range(-walkRange, walkRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
        }
    }
}
