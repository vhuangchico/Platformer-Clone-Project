using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    

    // stores a left/right bound via game objects
    public GameObject leftBoundary;
    public GameObject rightBoundary;
    public int Health;

    public bool goingLeft;

    // stores positions of the left/right boundaries
    private Vector3 leftPos;
    private Vector3 rightPos;

    // Start is called before the first frame update
    void Start()
    {
        leftPos = leftBoundary.transform.position;
        rightPos = rightBoundary.transform.position;

        leftBoundary.transform.parent = null;
        rightBoundary.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }
    private void MoveEnemy()
    {

        Vector3 moveDir;

        if (goingLeft) // going left
        {
            moveDir = Vector3.left;

            // check if enemy has moved to the left of the lefPos
            if (transform.position.x < leftPos.x)
            {
                goingLeft = false;
            }
        }
        else // going right
        {
            moveDir = Vector3.right;

            if (transform.position.x > rightPos.x)
            {
                goingLeft = true;
            }
        }
        transform.Translate(moveDir * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        print("Event triggered");
        if (other.gameObject.GetComponent<PlayerProjectile>())
        {

            print("Enemy hit");
            Health--;
            Destroy(other.gameObject);
            if (Health<=0)
            {
                Destroy(gameObject);
            }
        }
    }
}
