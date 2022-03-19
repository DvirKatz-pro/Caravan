using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{
    private float damage = 20;
    //if the player is hit by the arrow then the player will take damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStatus>().takeDamage(damage);

            Destroy(this.gameObject);
        }
       
    }
    //set the amount of damage that a player will take
    public void setDamage(float m_damage)
    {
        damage = m_damage;
    }
}
