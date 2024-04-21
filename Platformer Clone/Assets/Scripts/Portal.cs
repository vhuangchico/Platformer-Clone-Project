using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject teleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = teleportPoint.transform.position;
    }
}
