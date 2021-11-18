using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    Camera mainCam;
    public int attackDamage = 5;
    public TextMeshProUGUI healthText;
    private Movement _movement;
    //private ManualRoomPath mrp;

    bool poison = false;
    bool nausea = false;

    public AudioSource poisonedSound;
    public AudioSource nauseaedSound;
    public AudioSource drinkingSound;
    public AudioSource deathSound;
    public AudioSource doorOpening;

    void Start()
    {
        mainCam = Camera.main;      // tag lookup, not instant, that's why cache
        _movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (healthText) healthText.text = PublicVars.health.ToString();
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

        if (PublicVars.health <= 0){
            SceneManager.LoadScene("Dead");
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

    IEnumerator PoisonTime()
    {
        int ticks = 0;
        int totalTicks = 5;
        poisonedSound.Play();

        while (ticks < totalTicks)
        {
            ticks++;
            PublicVars.health -= 5;  // Player takes 5 damage
            yield return new WaitForSecondsRealtime(1);  // every 1 second
        }
    }

    IEnumerator NauseaTime()
    {
        _movement.SetSpeed(_movement.GetSpeed() / 2);
        nauseaedSound.Play();
        yield return new WaitForSeconds(5);
        _movement.SetSpeed(_movement.GetSpeed() * 2);
    }

    IEnumerator BoostTime()
    {
        _movement.SetSpeed(_movement.GetSpeed() * 2);
        
        yield return new WaitForSeconds(5);
        _movement.SetSpeed(_movement.GetSpeed() / 2);
    }

    /*
    public void setKey(string currRoom, string ifGotKey)
    {
        gotKey[currRoom] = ifGotKey;
    }

    public string getKey(string currRoom)
    {
        return gotKey[currRoom];
    }*/

    void OnTriggerEnter(Collider other)
    {
        //string currRoom = SceneManager.GetActiveScene().name;

        // while player is within the light they take 5 damage every 2 seconds
        if (other.CompareTag("GuardLight")){
            StartCoroutine(healthDecay());
        }   

        // win state
        if (other.CompareTag("Fence")){
            SceneManager.LoadScene("Win");
        }

        /* if water bucket are a trigger*/
        if (other.gameObject.CompareTag("Water"))
        {
            //PublicVars.got_key = true;

            //setKey(currRoom, "true");
            //Destroy(other.gameObject);
            drinkingSound.Play();
            foreach (Transform child in other.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            PublicVars.health += 20;
        }

        // if the player picks up a "power" energy drink
        if (other.gameObject.CompareTag("Power"))
        {
            PublicVars.got_key = true;
            drinkingSound.Play();
            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            attackDamage += 5;
        }

        // if the player picks up a "poison" energy drink
        if (other.gameObject.CompareTag("Poison"))
        {
            PublicVars.got_key = true;
            drinkingSound.Play();
            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            poison = true;
        }

        // if the player picks up a "nausea" energy drink
        if (other.gameObject.CompareTag("Nausea"))
        {
            PublicVars.got_key = true;
            drinkingSound.Play();
            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            nausea = true;
        }

        if (other.gameObject.CompareTag("Boost"))
        {
            PublicVars.got_key = true;
            drinkingSound.Play();
            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            StartCoroutine(BoostTime());
        }

        
        if (other.gameObject.CompareTag("Door"))
        {
            if (other.gameObject.GetComponent<Door>().locked == false)
            {
                doorOpening.Play();
            }
            //doorOpening.Play();
            //LoadNextRoom(currRoom, 0);
        }
        
    }

    IEnumerator healthDecay(){
        deathSound.Play();

        while (true)
        {
            PublicVars.health -= 5;  // Player takes 5 damage
            yield return new WaitForSecondsRealtime(2);  // every 2 seconds
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("GuardLight")){
            StopAllCoroutines();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    //Debug.Log("collding");
        if (other.gameObject.CompareTag("Enemy"))
        {
            // reset to corridor, lose all papers
            PublicVars.paper_count = 0;
            Room.Enter("Corridor");
        }
    }
}

