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
    public float DamageInterval = 1f;
    [Tooltip("The time to stay at each point")]
    public float TimeToStay = 1f;
    [Tooltip("Time to attack after the bot stops at a certain point")]
    public float TimeToAttack = 0.2f;

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
    float _withinDistanceTimeElapsed;
    float _lastDamageTimeElapsed;

    void Start()
    {
        _movement = GetComponent<Movement>();
        player = GameObject.FindGameObjectWithTag("Player");
        CurrentState = BotState.IDLE;
        _patrolEnumerator = NextPoint();
        _lastMoveTimeElapsed = MovementInterval;
        _stayedTimeElapsed = 0;
        _withinDistanceTimeElapsed = 0;
        _lastDamageTimeElapsed = DamageInterval;
    }

    private void Update()
    {
        if (_lastDamageTimeElapsed < DamageInterval) _lastDamageTimeElapsed += Time.deltaTime;
        if (_lastMoveTimeElapsed < MovementInterval) _lastMoveTimeElapsed += Time.deltaTime;
        if (_movement.IsMoving)
            _stayedTimeElapsed = 0;
        else
            _stayedTimeElapsed += Time.deltaTime;
        if (CurrentState != BotState.ALERT && Room.CurrentRoom.EnemyAlert) CurrentState = BotState.ALERT;

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

                // If the agent is not moving and there is the next point to visit, we keep on going
                if (_lastMoveTimeElapsed >= MovementInterval && _stayedTimeElapsed >= TimeToStay && !_movement.IsMoving && _patrolEnumerator.MoveNext())
                {
                    _lastMoveTimeElapsed = 0;
                    _movement.SetDestination(_patrolEnumerator.Current);
                }
                break;
            case BotState.ALERT:
                if (!Room.CurrentRoom.EnemyAlert)
                {
                    Room.CurrentRoom.EnemyAlert = true;
                    player.GetComponent<Player>().alertSound.Play();
                }

                Vector3 playerPos = player.transform.position, botPos = transform.position;
                if (Vector3.Distance(playerPos, botPos) <= PublicVars.MINIMUM_CHASE_DISTANCE * 2)
                    _withinDistanceTimeElapsed += Time.deltaTime;
                else
                    _withinDistanceTimeElapsed = 0;

                if (_movement.IsAttacking)
                {
                    Vector3 target = playerPos - botPos;
                    target.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(target);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Clamp(_movement.AngularSpeed * Time.deltaTime, 0, 1));
                    if (_lastDamageTimeElapsed >= DamageInterval && _withinDistanceTimeElapsed > 0)
                    {
                        _lastDamageTimeElapsed = 0;
                        PublicVars.health -= PublicVars.ENEMY_DAMAGE;
                    }
                }

                // Make sure that we keep some distance from the player
                if (_withinDistanceTimeElapsed >= TimeToAttack)
                {
                    _movement.SetDestination(botPos);
                    _movement.SetAttacking(true);
                }
                else
                    _movement.SetDestination(playerPos + (botPos - playerPos).normalized * PublicVars.MINIMUM_CHASE_DISTANCE);

                if (_movement.IsMoving) _movement.SetAttacking(false);
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

    /// <summary>Detect whether the player in front of you or not</summary>
    private bool DetectFront()
    {
        return RayCastSector(-FrontAlertAngle / 2, FrontAlertAngle / 2, FrontAlertDistance);
    }

    /// <summary>Detect whether the player around you or not</summary>
    private bool DetectSide()
    {
        return RayCastSector(-180, 180, SideAlertDistance);
    }

    /// <summary>Detect whether the player within a sector</summary>
    private bool RayCastSector(float start, float end, float distance)
    {
        Vector3 botToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, botToPlayer);

        if (botToPlayer.magnitude > distance) return false;
        if (angle < start || angle > end) return false;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, botToPlayer.normalized);
        Physics.Raycast(ray, out hit, botToPlayer.magnitude, ~Physics.IgnoreRaycastLayer);
        Debug.Log(hit.collider.gameObject);
        Debug.DrawRay(transform.position, botToPlayer, (hit.collider.gameObject == player) ? Color.red : Color.white, (hit.collider.gameObject == player) ? 1f : 0.1f);
        return hit.collider.gameObject == player;
    }
}
