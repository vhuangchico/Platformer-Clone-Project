using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Huang, Vincent
/// Updated: 4/20/24
/// Bullet fired by player goes left or right, despawning if it doesn't hit anything after 5 seconds. 
/// </summary>
/// 

public class PlayerProjectile : MonoBehaviour
{
    public float speed;
    public bool goingLeft;
    public float despawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnTimer(despawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        // bullet travels left if goingleft is true, right if false.
        if (goingLeft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        checkWall();
    }
// checks to see if projectile hits a wall. If it does, destroys projectile.
    private void checkWall()
    {
        if (HitLeftWall())
        {
            print("hit left wall");
            Destroy(gameObject);
        }
        if (HitRightWall())
        {
            print("hit right wall");
            Destroy(gameObject);
        }
    }

    private bool HitLeftWall() // Checks if projectile hit a wall on their left.
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 originOffset = new Vector3(0, 0.1f, 0);
        float playerWidth = 0.1f;

        bool hitLeftWall = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.left, out hit, playerWidth))
        {
            hitLeftWall = true;
        }
        return hitLeftWall;
    }
    // check if projectile hit a wall on their right
    private bool HitRightWall()
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 originOffset = new Vector3(0, 0.1f, 0);
        float playerWidth = 0.1f;

        bool hitRightWall = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.right, out hit, playerWidth))
        {
            hitRightWall = true;
        }
        return hitRightWall;
    }

    // Bullet despawns after 5 seconds. 
    private IEnumerator DespawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
