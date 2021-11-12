using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    Camera mainCam;
    public Transform spawnPoint;
    public GameObject bulletPrefab;
    public int bulletForce = 200;
    public int attackDamage = 5;
    public Text healthText;

    private Movement _movement;

    public int health = 100;

    bool poison = false;
    bool nausea = false;

    public bool gotKey = false;

    void Start()
    {
        mainCam = Camera.main;      // tag lookup, not instant, that's why cache
        _movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (healthText) healthText.text = health.ToString();
        // left click to walk
        if (Input.GetMouseButtonDown(0))
        {
            // raycast; drawing vector & what gets hit
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 200))
            {      // last param is length of ray; shorter is more efficient longer is more accurate
                // uses AI to navigate around mesh to this point
                _movement.SetDestination(hit.point);
                // can be used for enemies too; not dependent on mouse; destination could be player
            }
        }

        // right click to shoot
        if (Input.GetMouseButtonDown(1))
        {
            // raycast; drawing vector & what gets hit
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 200))
            {      // last param is length of ray; shorter is more efficient longer is more accurate
                transform.LookAt(hit.point);
                GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            }
        }

        if (poison == true)
        {
            StartCoroutine(PoisonTime());
            poison = false;
        }

        if (nausea == true)
        {
            StartCoroutine(NauseaTime());
            nausea = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        /* if bullets are a trigger*/
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            if (health <= 5)
            {
                // die
                SceneManager.LoadScene("Dead");
            }
            else
            {
                health -= 5;
            }
        }

        /* if water bucket are a trigger*/
        if (other.gameObject.CompareTag("Water"))
        {
            PublicVars.got_key = true;
            //Destroy(other.gameObject);
            foreach (Transform child in other.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            health += 20;
        }

        // if the player picks up a "power" energy drink
        if (other.gameObject.CompareTag("Power"))
        {
            PublicVars.got_key = true;
            Destroy(other.gameObject);
            attackDamage += 5;
        }

        // if the player picks up a "poison" energy drink
        if (other.gameObject.CompareTag("Poison"))
        {
            PublicVars.got_key = true;
            Destroy(other.gameObject);
            poison = true;
        }

        // if the player picks up a "nausea" energy drink
        if (other.gameObject.CompareTag("Nausea"))
        {
            PublicVars.got_key = true;
            Destroy(other.gameObject);
            nausea = true;
        }

        if (other.gameObject.CompareTag("Boost"))
        {
            PublicVars.got_key = true;
            Destroy(other.gameObject);
            StartCoroutine(BoostTime());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("collding");
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (health <= 5)
            {
                // die
                SceneManager.LoadScene("Dead");
            }
            else
            {
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

    IEnumerator PoisonTime()
    {
        int ticks = 0;
        int totalTicks = 5;

        while (ticks < totalTicks)
        {
            ticks++;
            health -= 5;  // Player takes 5 damage
            yield return new WaitForSecondsRealtime(1);  // every 1 second
        }
    }

    IEnumerator NauseaTime()
    {
        _movement.SetSpeed(_movement.GetSpeed() / 2);
        yield return new WaitForSeconds(5);
        _movement.SetSpeed(_movement.GetSpeed() * 2);
    }

    IEnumerator BoostTime()
    {
        _movement.SetSpeed(_movement.GetSpeed() * 2);
        yield return new WaitForSeconds(5);
        _movement.SetSpeed(_movement.GetSpeed() / 2);
    }

}
