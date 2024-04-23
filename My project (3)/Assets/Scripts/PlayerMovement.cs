using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Huang,Vincent
/// Updated: 04/11/24
/// Script controls player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{

    // Arrays

    // Assigned
    private Rigidbody rb;
    private Vector3 startPos;
    public GameObject playerCirno;
    public GameObject projPrefab;
    public GameObject heavyProjPrefab;
    public MeshRenderer cirnoImmunity;

    // Checks
    public bool isImmune;
    public bool isHeavy;
    public bool canShoot;
    public bool facingLeft;
    public bool shootLeft;
    public bool gotJump;
    public bool canFlick;
    public bool isFinal;

    // floats and ints
    public float speed = 10f;
    public float jumpForce = 10f;
    public float projectSpeed = 2f;
    public float shootCooldown = 0.5f;
    public int Health = 100;
    public int MaxHealth;
    public int healthGained;
    public int DamageRec;
    public int immuneTime = 25;
    public float flickTime = 0.1f;
    public int enemyCount;


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
        playerCirno.transform.rotation = Quaternion.Euler(0, 90, 0);
        facingLeft = true;
        shootLeft = true;
        isImmune = false;
        canShoot = true;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Checks if shooting is on cooldown.
            if (canShoot)
            {
                if (isHeavy) // if isHeavy is true, has players shoot heavy bullet projectiles. otherwise, shoot the default projectiles.
                {
                    GameObject newProj = Instantiate(heavyProjPrefab, transform.position, transform.rotation);
                    newProj.GetComponent<PlayerProjectile>().goingLeft = shootLeft;
                    canShoot = false;
                }
                else
                {
                    GameObject newProj = Instantiate(projPrefab, transform.position, transform.rotation);
                    newProj.GetComponent<PlayerProjectile>().goingLeft = shootLeft;
                    canShoot = false;
                }
                StartCoroutine(ShootTimer(shootCooldown)); // Starts shooting cooldown countdown.
            }
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
                }
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    private void Jumping()
    {
        // check for user input to jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (OnGround())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    // Checks if player hit a wall on their left
    private bool HitLeftWall() // 
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 originOffset = new Vector3(0, 0.1f, 0);
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
    // check if player hit a wall on their right
    private bool HitRightWall()
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 originOffset = new Vector3(0, 0.1f, 0);
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
    // checks if player is on the ground.
    private bool OnGround()
    {
        bool onGround = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f))
        {
            onGround = true;
        }

        return onGround;
    }
    // checks for trigger events if player collides with enemies or pickups.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            DamageRec = 15;
            if (isImmune)
            {
                DamageRec = 0;
            }
            Immunity();
            healthCheck();
        }
        if (other.gameObject.GetComponent<HardEnemy>())
        {
            DamageRec = 35;
            if (isImmune)
            {
                DamageRec = 0;
            }
            Immunity();
            healthCheck();
        } 
        // checks for collision w/ health pickup
        if (other.gameObject.GetComponent<HealthPickup>())
        {
            if (Health < MaxHealth) // will only heal player if they're not at full health. 
            {
                Health = Health + healthGained;
                Destroy(other.gameObject);
            }
        }
        // checks for collision w/ heavy bullet pickup
        if(other.gameObject.GetComponent<HeavyPickup>())
        {
            isHeavy = true;
            Destroy(other.gameObject);
        }
        // checks for collision w/ max health pickup
        if (other.gameObject.GetComponent<MaxPickup>())
        {
            MaxHealth = MaxHealth + 100; // increases max health by 100
            Health = MaxHealth; // sets the player's current health to MaxHealth, fully healing them with their new max hp.
            Destroy(other.gameObject);
        }
        if (other.gameObject.GetComponent<JumpPickup>())
        {
            jumpForce = jumpForce * 2;
            gotJump = true;
            Destroy(other.gameObject);
        }
    }

    // Damages the player if called, and loads Game Over Screen if health falls to 0 or below.
    private void healthCheck()
    {
        Health = Health - DamageRec;
        if (Health <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
    // makes player immune when damaged by enemy.
    private void Immunity()
    {
        if (!isImmune) // checks to see if player is already immune. Makes player immune if not.
        {
            isImmune = true;
            StartCoroutine(ImmuneTimer(immuneTime));
        }
    }
    // when timer ends, makes player vulnerable to damage again.
    private IEnumerator ImmuneTimer(float time)
    {
        Renderer[] rs = GetComponentsInChildren<Renderer>(); // gets all child meshrenderers of the 
        for (int i = 0; i < 25; i++) // Flickers for the duration of immuneTime;
        {
            foreach (Renderer r in rs)
            {
                r.enabled = false;
            }
            yield return new WaitForSeconds(flickTime);
            foreach (Renderer r in rs)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(flickTime); // each loop goes for about 0.2 seconds. Needs to loop 25 times to reach 5 seconds. 
        }    
        isImmune = false;
    }
    // when timer ends, lets player shoot again.
    private IEnumerator ShootTimer(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
    // checks if the player has gone through the final portal. Begins an enemy count if they are.
    private void checkFinal()
    {
        if (isFinal)
        {
            enemyCount--;
        }
        if (enemyCount <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }
}
