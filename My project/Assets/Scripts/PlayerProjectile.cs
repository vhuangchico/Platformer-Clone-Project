using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (goingLeft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
            transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private IEnumerator DespawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
