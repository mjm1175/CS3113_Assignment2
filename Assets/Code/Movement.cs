using UnityEngine;
using UnityEngine.AI;

/// <summary>Use this script to control anything that moves with a navagent</summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
    [Tooltip("Non-mandatory animator object to animate the moving object")]
    public Animator Animator;

    /// <value>Whether the agent is moving or not</value>
    public bool IsMoving => _navMeshAgent.velocity.magnitude > PublicVars.MINIMUM_MOVEMENT_SPEED;

    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Animator?.SetBool("IsMoving", IsMoving);
    }

    public void SetDestination(Vector3 destination)
    {
        _navMeshAgent.destination = destination;
    }
}