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
    public Transform spawnPoint;
    public GameObject bulletPrefab;
    public int bulletForce = 200;
    public int attackDamage = 5;
    public TextMeshProUGUI healthText;
    private Movement _movement;
    //private ManualRoomPath mrp;

    bool poison = false;
    bool nausea = false;

    //public bool gotKey = false;

    /*
    // Dictionary of each corridor and their respective rooms
    public Dictionary<string, List<string>> corridorList = new Dictionary<string, List<string>>();
    public Dictionary<string, string> roomList = new Dictionary<string, string>();
    // A dictionary with each scene and their respective keys
    public Dictionary<string, string> gotKey = new Dictionary<string, string>();

    public static List<string> l1 = new List<string> { "Room3", "Room6", "Room4" };
    public static List<string> l2 = new List<string> { "Room1", "Room8", "Room7" };
    public static List<string> l3 = new List<string> { "Room2", "Room9", "Room10" };
    public static List<string> l4 = new List<string> { "Room5", "Room12", "Room11" };
    */
    void Start()
    {
        mainCam = Camera.main;      // tag lookup, not instant, that's why cache
        _movement = GetComponent<Movement>();
        //mrp = GetComponent<ManualRoomPath>();

        /*
        corridorList.Add("Corridor", l1); // new List<string> { "Room3", "Room6", "Room4" });
        corridorList.Add("Corridor2", new List<string> { "Room1", "Room8", "Room7" });
        corridorList.Add("Corridor3", new List<string> { "Room2", "Room9", "Room10" });
        corridorList.Add("Corridor4", new List<string> { "Room5", "Room12", "Room11" });

        foreach (KeyValuePair<string, List<string>> rooms in corridorList)
        {
            //Debug.Log(rooms.Value[0]);
            roomList.Add(rooms.Value[0], rooms.Key);
            roomList.Add(rooms.Value[1], rooms.Key);
            roomList.Add(rooms.Value[2], rooms.Key);

            gotKey.Add(rooms.Value[0], "false");
            gotKey.Add(rooms.Value[1], "false");
            gotKey.Add(rooms.Value[2], "false");

            gotKey.Add(rooms.Key, "true");
        }

        foreach (KeyValuePair<string, string> rooms in gotKey)
        {
            //Debug.Log(rooms.Key);
            //Debug.Log(rooms.Value);
        }
        Debug.Log(SceneManager.GetActiveScene().name);*/
        //Debug.Log(corridorList[SceneManager.GetActiveScene().name][0]);
        //Debug.Log(corridorList[SceneManager.GetActiveScene().name][1]);
        //Debug.Log(corridorList[SceneManager.GetActiveScene().name][2]);
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

    IEnumerator PoisonTime()
    {
        int ticks = 0;
        int totalTicks = 5;

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
        /* if bullets are a trigger*/
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            if (PublicVars.health <= 5)
            {
                // die
                SceneManager.LoadScene("Dead");
            }
            else
            {
                PublicVars.health -= 5;
            }
        }

        string currRoom = SceneManager.GetActiveScene().name;

        /* if water bucket are a trigger*/
        if (other.gameObject.CompareTag("Water"))
        {
            //PublicVars.got_key = true;

            //setKey(currRoom, "true");
            //Destroy(other.gameObject);
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

            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            attackDamage += 5;
        }

        // if the player picks up a "poison" energy drink
        if (other.gameObject.CompareTag("Poison"))
        {
            PublicVars.got_key = true;

            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            poison = true;
        }

        // if the player picks up a "nausea" energy drink
        if (other.gameObject.CompareTag("Nausea"))
        {
            PublicVars.got_key = true;

            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            nausea = true;
        }

        if (other.gameObject.CompareTag("Boost"))
        {
            PublicVars.got_key = true;

            //setKey(currRoom, "true");
            Destroy(other.gameObject);
            StartCoroutine(BoostTime());
        }

        /*
        if (other.gameObject.CompareTag("Door"))
        {
            LoadNextRoom(currRoom, 0);
        }

        //Debug.Log((currRoom));
        //if (getKey(currRoom) == "true")
        if (gotKey[currRoom] == "true")
            {
            if (other.gameObject.CompareTag("Door1"))
            {
                LoadNextRoom(currRoom, 1);
            }
            if (other.gameObject.CompareTag("Door2"))
            {
                LoadNextRoom(currRoom, 2);
            }
            if (other.gameObject.CompareTag("Door3"))
            {
                LoadNextRoom(currRoom, 3);
            }
        }*/
        /*
        if (other.gameObject.CompareTag("DoorBk"))
        {
            LoadNextRoom(currRoom, -1);
        }*/
        
    }

    private void OnCollisionEnter(Collision other)
    {
    //Debug.Log("collding");
    if (other.gameObject.CompareTag("Enemy"))
        {
        if (PublicVars.health <= 5)
            {
                // die
                SceneManager.LoadScene("Dead");
            }
            else
            {
                PublicVars.health -= 5;
            }
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

