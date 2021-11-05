using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Camera mainCam;
    public Transform spawnPoint;
    public GameObject bulletPrefab;
    public int bulletForce = 200;
    int health = 100;
    public Text healthText;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        mainCam = Camera.main;      // tag lookup, not instant, that's why cache
    }

    void Update()
    {
        healthText.text = health.ToString();
        // left click to walk
        if(Input.GetMouseButtonDown(0)){
            // raycast; drawing vector & what gets hit
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 200)){      // last param is length of ray; shorter is more efficient longer is more accurate
                // uses AI to navigate around mesh to this point
                _navMeshAgent.destination = hit.point;
                // can be used for enemies too; not dependent on mouse; destination could be player
            }
        }

        // right click to shoot
        if(Input.GetMouseButtonDown(1)){
            // raycast; drawing vector & what gets hit
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 200)){      // last param is length of ray; shorter is more efficient longer is more accurate
                transform.LookAt(hit.point);
                GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        /* if bullets are a trigger*/
        if (other.gameObject.CompareTag("Enemy")){
            Destroy(other.gameObject);
            if (health <= 5){
                // die
                SceneManager.LoadScene("Dead");
            } else {
                health -= 5;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")){
            if (health <= 5){
                // die
                SceneManager.LoadScene("Dead");
            } else {
                health -= 5;
            }
        }

        /* if bullet is collision
        if (other.gameObject.CompareTag("Bullet")){
            Destroy(other.gameObject);
            if (health <= 5){
                // die
                SceneManager.LoadScene("Dead");
            } else {
                health -= 5;
            }
        }*/
    }
}
