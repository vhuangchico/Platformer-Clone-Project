using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Huang, Vincent
/// Updated: 4/20/24
/// Bullet fired by player goes left or right, despawning if it doesn't hit anything after 5 seconds. 
/// </summary>

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
    }
 
    // Bullet despawns after 5 seconds. 
    private IEnumerator DespawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
