using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    GameObject player;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LookForPlayer());
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
