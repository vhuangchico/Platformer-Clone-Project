using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Huang, Vincent
/// Updated: 4/20/24
/// Hard enemy that goes torwards the player's direction and is defeated by shooting it.
/// </summary>
public class HardEnemy : MonoBehaviour
{
    public float speed = 5;


    // stores a left/right bound via game objects
    public Transform player;
    public int Health;

    public bool goingLeft;
    private Rigidbody rb;

    // stores positions of the left/right boundaries
    private Vector3 leftPos;
    private Vector3 rightPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }
    private void MoveEnemy()
    {

        Vector3 moveDir;
        if (player.position.x < transform.position.x) // if the player's x position is lower than the enemy's, it'll go left and vice versa.
        {
            moveDir = Vector3.left;
        }
        else // going right
        {
            moveDir = Vector3.right;
        }
        transform.Translate(moveDir * speed * Time.deltaTime);
    }
        
     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerProjectile>()) // checks for collision with a projectile.
        {
            if (other.gameObject.tag == "Heavy") // if projectile has heavy tag, enemy takes 3 damage. otherwise takes 1 damage.
            {
                Health = Health - 3;
            }
            else
            {
                Health--;
            }
            Destroy(other.gameObject); // destroys the projectile after trigger.

            if (Health <= 0) // enemy is destroyed when health reaches 0.
            {
                Destroy(gameObject);
            }
        }
    }

}
