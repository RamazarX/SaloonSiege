using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform targetBase;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (targetBase != null)
        {
            agent.SetDestination(targetBase.position);
        }
    }
}
