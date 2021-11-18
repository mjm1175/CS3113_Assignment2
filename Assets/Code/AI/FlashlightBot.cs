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

    [Tooltip("The maximum distance for the bot to detect the player")]
    public float FrontAlertDistance = 100;
    [Range(0, 360)]
    [Tooltip("The angle that defines the front alert area")]
    public float FrontAlertAngle = 60;
    [Tooltip("The radius around the bot to detect the player")]
    public float SideAlertDistance = 20;

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
                if (DetectFront() || DetectSide())
                {
                    _lastVisitedPoint = -1;
                    _patrolEnumerator = NextPoint();
                    CurrentState = BotState.ALERT;
                }

                if (_movement.IsMoving)
                {
                    _stayedTimeElapsed = 0;
                }
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
                Vector3 playerPos = player.transform.position, botPos = transform.position;
                // Make sure that we keep some distance from the player
                _movement.SetDestination(playerPos + (botPos - playerPos).normalized * PublicVars.MINIMUM_CHASE_DISTANCE);
                _movement.SetAttacking(Vector3.Distance(playerPos, botPos) <= 2 * PublicVars.MINIMUM_CHASE_DISTANCE);
                break;
        }
    }

    public void Hit() { return; }

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

    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            _movement.Die();
            Destroy(gameObject, 2);
        }
    }*/

    /// <summary>Detect whether there is a player in front of you or not</summary>
    private bool DetectFront()
    {
        return RayCastSector(-FrontAlertAngle / 2, FrontAlertAngle / 2, FrontAlertDistance);
    }

    /// <summary>Detect whether there is a player around you or not</summary>
    private bool DetectSide()
    {
        return RayCastSector(-180, 180, SideAlertDistance);
    }

    /// <summary>Detect whether there is a player within a sector</summary>
    private bool RayCastSector(float start, float end, float distance)
    {
        for (float i = start; i <= end; i += PublicVars.ALERT_DETECTION_STEP)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Quaternion.AngleAxis(i, Vector3.up) * transform.forward);

            if (Physics.Raycast(ray, out hit, distance) && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}
