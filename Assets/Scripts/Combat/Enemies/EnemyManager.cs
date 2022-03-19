using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    //list of close range enemies and long range enemies
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> rangedEnemies = new List<GameObject>();
    [SerializeField] private GameObject player;

    //nav mesh settings
    [SerializeField] private float attackStoppingDistance;
    [SerializeField] private float rangedAttackStoppingDistance;
    [SerializeField] private int amountCanAttack;
    private int attackingCount = 0;
    
    //register an enemy with the manager
    public void registar(GameObject enemy)
    {
        
        enemies.Add(enemy);
        
    }
    //unregister an enemy with manager
    public void unRegistar(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            Debug.Log("Combat Done");
        }
    }

    private void Update()
    {
        //sort the close ranged enemies based on distance from player
        //closeEnemies.Sort(distanceComparetor);
    }

    public bool permissionToAttack(EnemyActions enemyActions)
    {
        if (attackingCount< amountCanAttack && !enemyActions.attackCooldown())
        {
            attackingCount++;
            return true;
        }
        return false;
    }
    public void doneAttacking()
    {
        attackingCount--;
    }
   
    int distanceComparetor(GameObject a, GameObject b)
    {
        float distamceA = Vector3.Distance(a.transform.position, player.transform.position);
        float distamceB = Vector3.Distance(b.transform.position, player.transform.position);
        return distamceA.CompareTo(distamceB);
    }
    Vector3 GetRandomTargetPos(float minRadius, float maxRadius)
    {
        Vector2 rndPos = Random.insideUnitCircle * (maxRadius - minRadius);
        rndPos += rndPos.normalized * minRadius;
        return new Vector3(player.transform.position.x + rndPos.x, player.transform.position.y, player.transform.position.z + rndPos.y);
    }
  
}
