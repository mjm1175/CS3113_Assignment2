using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlashlightBot : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    GameObject player;
   
    private Vector3 pointA;
    private Vector3 pointB;
    public float speed = 1.0f;
    public float xDistance=0;
    public float yDistance=0;
    public float zDistance = 7.5f;
    public string rotateDirection = "z";
    public int rotateSpeed = 1;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        pointA = pointB = transform.position;
        pointB.x += xDistance;
        pointB.y += yDistance;
        pointB.z += zDistance;

        StartCoroutine(BasicWalk());
        //StartCoroutine(LookForPlayer());
    }
   
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i= 0.0f;
        var rate= speed/time;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
        //TODO: ROTATION NOT WORKING
        /*Quaternion currentRotation = thisTransform.rotation;
        if (rotateDirection == "z"){
            Quaternion wantedRotation = Quaternion.Euler(0,0,180);
            thisTransform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotateSpeed);
        } else if (rotateDirection == "y"){
            Quaternion wantedRotation = Quaternion.Euler(0,180,0);
            thisTransform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotateSpeed);
        } else if (rotateDirection == "x"){
            Quaternion wantedRotation = Quaternion.Euler(180,0,0);
            thisTransform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotateSpeed);
        }
        yield return null;*/
    }
    IEnumerator BasicWalk()
    {
        while (true){
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, 3.0f));
            yield return StartCoroutine(MoveObject(transform, pointB, pointA, 3.0f));
        }
    }

    IEnumerator LookForPlayer(){
        while(true){
            yield return new WaitForSeconds(.5f);       // time to adjust path; slower = dumber
            _navMeshAgent.destination = player.transform.position;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Bullet")){
            PublicVars.kill_count++;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
