using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class FlashlightBot : MonoBehaviour
{
    GameObject player;

    /// <value>The points for patrolling</value>
    public Vector3[] PathPoints = new Vector3[] { };
    public float speed = 1.0f;

    Movement _movement;

    void Start()
    {
        _movement = GetComponent<Movement>();
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(BasicWalk());
        //StartCoroutine(LookForPlayer());
    }

    IEnumerator MoveObject(Vector3 endPos, float time)
    {
        _movement.SetDestination(endPos);
        yield return new WaitForSeconds(time);
    }

    IEnumerator BasicWalk()
    {
        // Keep moving from point to point
        for (int i = 0; ; i++, i %= PathPoints.Length)
            yield return StartCoroutine(MoveObject(PathPoints[i], 3.0f));
    }

    IEnumerator LookForPlayer()
    {
        while (true)
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
