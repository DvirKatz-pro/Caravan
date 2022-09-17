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
    private List<GameObject> charactersToAvoid = new List<GameObject>();
    [SerializeField] private GameObject player;

    [SerializeField] private int amountShouldAttackPlayer;
    [SerializeField] private float enemyRallyCircleRadius;
    [SerializeField] private float playerAvoidanceCircleRadius;

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
       charactersToAvoid.Add(player);
    }

    private void Update()
    {
        
    }

    public Vector3 CoordinateEnemy(GameObject enemy)
    {
        enemies.Sort(delegate (GameObject firstEnemy, GameObject secondEnemy)
        {
            return Vector2.Distance(player.transform.position, firstEnemy.transform.position)
            .CompareTo(
              Vector2.Distance(player.transform.position, secondEnemy.transform.position));
        });
        int enemyIndex = -1;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == enemy)
            {
                enemyIndex = i;
                break;
            }
        }

        if (enemyIndex < amountShouldAttackPlayer)
        {
            return player.transform.position;
        }
        Vector3 averagePointOfCharacters = getAveragePointOfCharacters();
        Vector3 rallyPosition = averagePointOfCharacters + Random.insideUnitSphere * enemyRallyCircleRadius;
        if (IsPointInsideCircle(averagePointOfCharacters, enemyRallyCircleRadius, rallyPosition) 
            && IsPointInsideCircle(player.transform.position, playerAvoidanceCircleRadius, rallyPosition))
        {
            rallyPosition = MovePointOutsidePlayerAvoidanceCircle(averagePointOfCharacters,player.transform.position,playerAvoidanceCircleRadius,rallyPosition);
        }
        return rallyPosition;
    }
    public Vector3 getAveragePointOfCharacters()
    {
        Vector3 averagePos = player.transform.position;

        foreach (GameObject g in enemies)
        {
            averagePos += g.transform.position;
        }
        averagePos = averagePos / (enemies.Count + 1);
        return averagePos;
    }

    public bool IsPointInsideCircle(Vector3 circleCenter, float circleRadius, Vector3 position)
    {
        if (Vector3.Distance(circleCenter, position) > circleRadius)
        {
            return false;
        }
        return true;
    }

    private Vector3 MovePointOutsidePlayerAvoidanceCircle(Vector3 averagePointOfCharacters,Vector3 playerPosition,float playerAvoidanceCircleRadius, Vector3 rallyPosition)
    {
        while (IsPointInsideCircle(playerPosition, playerAvoidanceCircleRadius, rallyPosition))
        {
            Vector3 circleCenterToRally = averagePointOfCharacters - rallyPosition;
            float vectorMagnitute = circleCenterToRally.magnitude * 0.75f;
            rallyPosition = circleCenterToRally.normalized * vectorMagnitute;
            rallyPosition += averagePointOfCharacters;
        }
        return rallyPosition;
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
