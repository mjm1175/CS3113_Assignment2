using UnityEngine;
using UnityEngine.AI;

/// <summary>Use this script to control anything that moves with a navagent</summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
    /// <value>The animator is not mandatory</value>
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _animator?.SetBool("IsMoving", _navMeshAgent.velocity.magnitude > 0.2f);
    }

    public void SetDestination(Vector3 destination)
    {
        _navMeshAgent.destination = destination;
    }
}