using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Huang, Vincent
/// Updated: 4/20/24
/// Destroys projectiles when triggered
/// </summary>
public class Wall : MonoBehaviour
{
   private void onTriggerEnter (Collider other)
    {
        if(other.gameObject.GetComponent<PlayerProjectile>())
        {
            Destroy(other.gameObject);
        }
    }
}
