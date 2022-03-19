using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider : MonoBehaviour
{
	public Transform avoidEnemy;
	public void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Enemy")
		{
			avoidEnemy = collider.transform;
		}
	}

	public void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Enemy")
		{
			avoidEnemy = null;
		}
	}
}
