using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Movement))]
public class FlashlightBot : MonoBehaviour
{
    BotState CurrentState;
    GameObject player;

    [Tooltip("The points for patrolling")]
    public Vector3[] PathPoints = new Vector3[] { };
    [Tooltip("The interval between moving from points to points")]
    public float MovementInterval = 2f;
    [Tooltip("The time to stay at each point")]
    public float TimeToStay = 1f;

    Movement _movement;
    int _lastVisitedPoint = -1;
    IEnumerator<Vector3> _patrolEnumerator;
    float _lastMoveTimeElapsed;
    float _stayedTimeElapsed;

    void Start()
    {
        _movement = GetComponent<Movement>();
        player = GameObject.FindGameObjectWithTag("Player");
        CurrentState = BotState.IDLE;
        _patrolEnumerator = NextPoint();
        _lastMoveTimeElapsed = MovementInterval;
        _stayedTimeElapsed = 0;
    }

    private void Update()
    {
        if (_lastMoveTimeElapsed < MovementInterval) _lastMoveTimeElapsed += Time.deltaTime;
        // Always transit from idle state to patrol state
        switch (CurrentState)
        {
            case BotState.IDLE:
                CurrentState = BotState.PATROL;
                break;
            case BotState.PATROL:
                if (_movement.IsMoving)
                    _stayedTimeElapsed = 0;
                else
                    _stayedTimeElapsed += Time.deltaTime;

                // If the agent is not moving and there is the next point to visit, we keep on going
                if (_lastMoveTimeElapsed >= MovementInterval && _stayedTimeElapsed >= TimeToStay && !_movement.IsMoving && _patrolEnumerator.MoveNext())
                {
                    _lastMoveTimeElapsed = 0;
                    _movement.SetDestination(_patrolEnumerator.Current);
                }
                break;
            case BotState.ALERT:
                StartCoroutine(LookForPlayer());
                break;
        }
    }

    IEnumerator<Vector3> NextPoint()
    {
        if (PathPoints.Length == 0) yield break;

        // When the last visited point is -1, we go to the nearest point
        if (_lastVisitedPoint == -1)
        {
            _lastVisitedPoint = PathPoints
                .Select((point, index) => (point, index))
                .OrderBy(item => Vector3.Distance(item.point, player.transform.position))
                .First().index;
            yield return PathPoints[_lastVisitedPoint];
        }

        while (CurrentState == BotState.PATROL)
        {
            _lastVisitedPoint %= PathPoints.Length;
            Debug.Log(_lastVisitedPoint);
            yield return PathPoints[_lastVisitedPoint++];
        }
    }

    IEnumerator LookForPlayer()
    {
        while (CurrentState == BotState.ALERT)
        {
            yield return new WaitForSeconds(.5f);       // time to adjust path; slower = dumber
            _movement.SetDestination(player.transform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            PublicVars.kill_count++;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
