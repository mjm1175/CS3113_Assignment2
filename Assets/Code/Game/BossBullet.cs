using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
public float speed;
 
    private Transform player;
    //private Transform player2;
    //private Transform player3;
 
 
    private Vector3 target;
    private Vector3 target2;
    private Vector3 target3;
    public GameObject projectileDestination;
    //public GameObject projectileDestination2;
    //public GameObject projectileDestination3;
    float dirX, dirY;
    public Rigidbody rb;
    public float shootForce;
    private int destPoint;
 
 
 
 
 
    private void Start()
    {
   
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("ProjectileDestination").transform;
        //player2 = GameObject.FindGameObjectWithTag("ProjectileDestination2").transform;
        //player3 = GameObject.FindGameObjectWithTag("ProjectileDestination3").transform;
        target = new Vector3(player.position.x, player.position.y, player.position.z);
 
        direction = (player.position - transform.position).normalized;
    }
 
 
    private Vector3 direction;
    private void FixedUpdate()
    {
   
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        transform.position += direction * speed;
 
 
        if (transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z)
        {
            DestroyProjectile();
        }
 
 
 
 
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        DestroyProjectile();
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectile();
        }
    }
 
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
