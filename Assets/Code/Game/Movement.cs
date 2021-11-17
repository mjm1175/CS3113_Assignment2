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

    public void SetAttacking(bool isAttacking)
    {
        if (Animator?.GetBool("IsAttacking") != isAttacking)
        {
            Animator?.SetBool("IsAttacking", isAttacking);
            Animator?.SetTrigger("Animate");
        }
    }

    public void Die()
    {
        if (!Animator?.GetBool("IsDead") ?? false)
        {
            _navMeshAgent.speed = 0;
            _navMeshAgent.enabled = false;
            Animator?.SetBool("IsDead", true);
            Animator?.SetTrigger("Animate");
        }
    }

    public void SetSpeed(float newSpeed)
    {
        _navMeshAgent.speed = newSpeed;
    }

    public float GetSpeed()
    {
        return _navMeshAgent.speed;
    }

    public void SetDestination(Vector3 destination)
    {
        if (_navMeshAgent.enabled)
            _navMeshAgent.destination = destination;
    }
}