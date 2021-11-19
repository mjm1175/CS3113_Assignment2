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
    public AudioSource alertSound;

    void Start()
    {
        mainCam = Camera.main;      // tag lookup, not instant, that's why cache
        _movement = GetComponent<Movement>();

        PublicVars.doorOpening = doorOpening;
    }

    void Update()
    {
        if (healthText) healthText.text = PublicVars.Health.ToString();
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

        if (PublicVars.Health <= 0)
        {
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
            PublicVars.Health -= 5;  // Player takes 5 damage
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

    void OnTriggerEnter(Collider other)
    {
        //string currRoom = SceneManager.GetActiveScene().name;

        // while player is within the light they take 5 damage every 2 seconds
        if (other.CompareTag("GuardLight"))
        {
            StartCoroutine(healthDecay());
        }

        // win state
        if (other.CompareTag("Fence"))
        {
            SceneManager.LoadScene("Win");
        }

        /* if water bucket are a trigger*/
        if (other.gameObject.CompareTag("Water"))
        {
            drinkingSound.Play();
            foreach (Transform child in other.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            PublicVars.Health += 20;
        }

        // if the player picks up a "power" energy drink
        if (other.gameObject.CompareTag("Power"))
        {
            drinkingSound.Play();
            Destroy(other.gameObject);
            attackDamage += 5;
        }

        // if the player picks up a "poison" energy drink
        if (other.gameObject.CompareTag("Poison"))
        {
            drinkingSound.Play();
            Destroy(other.gameObject);
            poison = true;
        }

        // if the player picks up a "nausea" energy drink
        if (other.gameObject.CompareTag("Nausea"))
        {
            drinkingSound.Play();
            Destroy(other.gameObject);
            nausea = true;
        }

        if (other.gameObject.CompareTag("Boost"))
        {
            drinkingSound.Play();
            Destroy(other.gameObject);
            StartCoroutine(BoostTime());
        }


        if (other.gameObject.CompareTag("Door"))
        {
            if (other.gameObject.GetComponent<Door>().locked == false)
            {
                doorOpening.Play();
            }
        }

    }

    IEnumerator healthDecay()
    {
        deathSound.Play();

        while (true)
        {
            PublicVars.Health -= 5;  // Player takes 5 damage
            yield return new WaitForSecondsRealtime(2);  // every 2 seconds
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GuardLight"))
        {
            StopAllCoroutines();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // reset to corridor, lose all papers
            PublicVars.PaperCount = 0;
            Room.Enter("Corridor");
        }
    }
}

