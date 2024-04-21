using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPortal : MonoBehaviour
{
    public GameObject teleportPoint;
    public GameObject player;
    public PlayerMovement playerMovement;
    private void Start()
    {

        playerMovement = player.GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if(player.GetComponent<PlayerMovement>().isHeavy == true && player.GetComponent<PlayerMovement>().gotJump == true)
        {
            other.transform.position = teleportPoint.transform.position;
        }
    }
}
