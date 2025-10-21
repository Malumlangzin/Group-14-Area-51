using UnityEngine;
using UnityEngine.AI;

public class NaveMeshAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] patrolPoints;
    private int currentPoint = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint++;

            if(currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }

            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }
}
