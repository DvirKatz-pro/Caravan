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
        CoordinateEnemyies();
    }

    public void CoordinateEnemyies()
    {
        enemies.Sort(delegate (GameObject firstEnemy, GameObject secondEnemy)
        {
            return Vector2.Distance(player.transform.position, firstEnemy.transform.position)
            .CompareTo(
              Vector2.Distance(player.transform.position, secondEnemy.transform.position));
        });
       
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyActions enemyActions = enemies[i].GetComponent<EnemyActions>();
            if (i < amountShouldAttackPlayer && (enemyActions.GetAction() == EnemyActions.Actions.idle || enemyActions.GetAction() == EnemyActions.Actions.moveing))
            {
                enemyActions.Move(player.transform.position);
            }
            else if (i >= amountShouldAttackPlayer && (enemyActions.GetAction() == EnemyActions.Actions.idle || enemyActions.GetAction() == EnemyActions.Actions.moveing))
            {
                float torusMidPointRadius = (enemyRallyCircleRadius - playerAvoidanceCircleRadius) * 0.5f;
                float circleCenterToTorusCenterRadius = playerAvoidanceCircleRadius + torusMidPointRadius;
                Vector3 randPointTorus = GetRandomPositionInTorus(circleCenterToTorusCenterRadius, torusMidPointRadius);
                if (Vector3.Distance(enemies[i].transform.position, player.transform.position) > Vector3.Distance(enemies[i].transform.position, randPointTorus))
                {
                    enemyActions.Stop();
                }
                else
                {
                    enemies[i].GetComponent<EnemyController>().RallyPos = randPointTorus;
                }
            }
        }

       
    }
    

    public bool IsPointInsideCircle(Vector3 circleCenter, float circleRadius, Vector3 position)
    {
        if (Vector3.Distance(circleCenter, position) > circleRadius)
        {
            return false;
        }
        return true;
    }

    Vector3 GetRandomPositionInTorus(float circleCenterToTorusCenterRadius, float torusMidPointRadius)
    {
        float rndAngle = Random.value * (2 * Mathf.PI);

        // determine position
        float randX = Mathf.Sin(rndAngle);
        float randZ = Mathf.Cos(rndAngle);

        Vector3 torusCenterPos = new Vector3(randX, 0, randZ);
        torusCenterPos *= circleCenterToTorusCenterRadius;

        Vector3 randomPointInTorus = torusCenterPos + Random.insideUnitSphere * torusMidPointRadius + player.transform.position;

        return (randomPointInTorus);
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
