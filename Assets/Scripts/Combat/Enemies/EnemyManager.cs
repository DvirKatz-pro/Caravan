using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class incharge of Enemy group behaviour
/// </summary>
public class EnemyManager : MonoBehaviour
{
    //list of close range enemies and long range enemies
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> rangedEnemies = new List<GameObject>();
    [SerializeField] private GameObject player;

    //nav mesh settings
    [SerializeField] private float attackStoppingDistance;
    [SerializeField] private float rangedAttackStoppingDistance;
    [SerializeField] private int amountAroundPlayer;

    private List<Vector3> positionsAroundPlayer;
    private Dictionary<GameObject, Vector3> takenPositionsAroundPlayer;
    /// <summary>
    /// register an enemy with the manager
    /// </summary>
    public void Registar(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    /// <summary>
    /// unregister an enemy with manager
    /// </summary>
    public void UnRegistar(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            Debug.Log("Combat Done");
        }
    }
    private void Start()
    {
       
    }

    private void Update()
    {
        
    }
    
    /*
    /// <summary>
    /// Enemies will ask the manager if they can attack
    /// </summary>
    public bool PermissionToAttack(EnemyActions enemyActions)
    {
        if (attackingCount< amountCanAttack && !enemyActions.AttackCooldown())
        {
            attackingCount++;
            return true;
        }
        return false;
    }
    /// <summary>
    /// Enemies will tell the manager when they are attacking
    /// </summary>
    public void DoneAttacking()
    {
        attackingCount--;
    }
    */
   
  
}
