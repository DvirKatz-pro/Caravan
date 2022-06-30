using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class incharge of arrow behaviour
/// </summary>
public class ArrowCollision : MonoBehaviour
{
    private float damage = 20;
    /// <summary>
    /// if the player is hit by the arrow then the player will take damage
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage);

            Destroy(this.gameObject);
        }
       
    }
    /// <summary>
    /// set the amount of damage that a player will take
    /// </summary>
    public void SetDamage(float m_damage)
    {
        damage = m_damage;
    }
}
