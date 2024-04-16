using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Author: Huang,Vincent
/// Updated: 04/11/24
/// Script controls player movement.
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    public int Health = 100;
    public float speed = 10f;
    public float jumpForce = 10f;
    public float projectSpeed = 2f;
    private Rigidbody rb;
    private Vector3 startPos;
    public bool facingLeft = false;
    public bool shootLeft;
    public GameObject playerCirno;
    public GameObject projPrefab;


    // UI Stuff

    public TMP_Text healthText;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        startPos = transform.position;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        healthText.text = "Health: " + Health.ToString();
        Move();
        Jumping();
        SpawnProj();
    }

    // Player shoots a projectile when X is pressed
    private void SpawnProj()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject newProj = Instantiate(projPrefab, transform.position, transform.rotation);
            newProj.GetComponent<PlayerProjectile>().goingLeft = shootLeft;
        }
    }

    private void Move()
    {
        // check for user input to move left
        if (Input.GetKey(KeyCode.A))
        {

           if(!HitLeftWall())
            {
                if (!facingLeft)
                {
                    playerCirno.transform.rotation = Quaternion.Euler(0, 90, 0);
                    facingLeft = true;
                    shootLeft = true;
                    //  transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
        // check for user input to move right
        if (Input.GetKey(KeyCode.D))
        {
            if (!HitRightWall())
            {
                if (facingLeft)
                {
                    playerCirno.transform.rotation = Quaternion.Euler(0, -90, 0);
                    facingLeft = false;
                    shootLeft = false;
                    //  transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    private void Jumping()
    {
        // check for user input to jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnGround())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }


    private bool HitLeftWall()
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 originOffset = new Vector3(0, 0.9f, 0);
        float playerWidth = 0.5f;

        bool hitLeftWall = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.left, out hit, playerWidth))
        {
            hitLeftWall = true;
        }
        if (Physics.Raycast(raycastOrigin + originOffset, Vector3.left, out hit, playerWidth))
        {
            hitLeftWall = true;
        }
        if (Physics.Raycast(raycastOrigin - originOffset, Vector3.left, out hit, playerWidth))
        {
            hitLeftWall = true;
        }
        return hitLeftWall;
    }
    private bool HitRightWall()
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 originOffset = new Vector3(0, 0.9f, 0);
        float playerWidth = 0.5f;

        bool hitRightWall = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.right, out hit, playerWidth))
        {
            hitRightWall = true;
        }
        if (Physics.Raycast(raycastOrigin + originOffset, Vector3.right, out hit, playerWidth))
        {
            hitRightWall = true;
        }
        if (Physics.Raycast(raycastOrigin - originOffset, Vector3.right, out hit, playerWidth))
        {
            hitRightWall = true;
        }
        return hitRightWall;
    }
    private bool OnGround()
    {
        bool onGround = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.3f))
        {
            onGround = true;
        }

        return onGround;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Health--;
        }
    }
}
