using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossFight : MonoBehaviour
{
    public float timeBtwShots = 1;
    private int bulletForce = 100;
    public GameObject projectile;
    public GameObject player;
    public GameObject projectileStartingPos;
    int health = 200;
    public TextMeshProUGUI healthText;
    
    void Start()
    {
        StartCoroutine(ShootAtPlayer());
    }
    
    private void LateUpdate() {
        healthText.text = health.ToString();
    }

    IEnumerator ShootAtPlayer(){
        while(true){
            yield return new WaitForSeconds(timeBtwShots);       // time between shots
            // middle shot
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
            GameObject newBullet = Instantiate(projectile, projectileStartingPos.transform.position, lookRotation);
            //GameObject newBullet = Instantiate(projectile, projectileStartingPos.transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(player.transform.position * bulletForce);
            //newBullet.transform.Translate(Vector2.up * bulletForce * Time.deltaTime, Space.World);

            /* cone shape not working
            // left shot
            Vector3 cone1 = lookRotation.eulerAngles + new Vector3(0, 0, 30); 
            newBullet = Instantiate(projectile);
            newBullet.transform.position = projectileStartingPos.transform.position;
            newBullet.transform.eulerAngles = cone1;
            newBullet.GetComponent<Rigidbody>().AddForce(cone1 * bulletForce);

            // right shot
            Vector3 cone2 = lookRotation.eulerAngles + new Vector3(0, 0, -30); 
            newBullet = Instantiate(projectile);
            newBullet.transform.position = projectileStartingPos.transform.position;
            newBullet.transform.eulerAngles = cone2;
            newBullet.GetComponent<Rigidbody>().AddForce(cone2 * bulletForce); */

        }
    }

    /*private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Bullet")){
            Destroy(other.gameObject);
            if (health <= 5){
                // die
                healthText.text = "DEAD";
            } else {
                health -= 5;
            }
        }
    }*/
}
