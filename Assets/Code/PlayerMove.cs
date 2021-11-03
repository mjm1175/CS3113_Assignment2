using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Camera mainCam;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        mainCam = Camera.main;      // tag lookup, not instant, that's why cache
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            // raycast; drawing vector & what gets hit
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 200)){      // last param is length of ray; shorter is more efficient longer is more accurate
                // uses AI to navigate around mesh to this point
                _navMeshAgent.destination = hit.point;
                // can be used for enemies too; not dependent on mouse; destination could be player
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        // i = number of doors in this scene; same as array size in PublicVars
        /*for (int i=0; i < PublicVars.hasKey.Length; i++){
            if (other.gameObject.CompareTag("Key"+i)){
                Destroy(other.gameObject);
                PublicVars.hasKey[i] = true;
            }
        }*/
         if (other.CompareTag("Key0")){
            Destroy(other.gameObject);
            PublicVars.hasKey[0] = true;
        }
        if (other.gameObject.CompareTag("Key0")){
            Destroy(other.gameObject);
            PublicVars.hasKey[0] = true;
        } else if (other.gameObject.CompareTag("Key1")){
            Destroy(other.gameObject);
            PublicVars.hasKey[1] = true;
        } else if (other.gameObject.CompareTag("Key2")){
            Destroy(other.gameObject);
            PublicVars.hasKey[2] = true;
        }
    }
}
