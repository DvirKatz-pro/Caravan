using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class incharge of Enemy Descsion and action taking
/// </summary>
public class EnemyController : MonoBehaviour
{
    //Gameplay related values
    [SerializeField] protected float distanceFromTarget;
    [SerializeField] protected Vector2 timeToRallyMinMax;
    [SerializeField] protected bool canBeKnockedBack;
    public bool permissionToAttack { get; set; }
    protected Vector3 avoidVec = Vector3.zero;
    public Vector3 RallyPos { get; set; }
    public bool onCooldown { get; set; }

    //Needed components
    protected Transform target;
    protected EnemyManager manager;
    protected EnemyActions actions;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();

        actions = GetComponent<EnemyActions>();

        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        manager.Registar(this.gameObject);

        onCooldown = false;
        
    }
    

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (actions.GetAction() == EnemyActions.Actions.idle || actions.GetAction() == EnemyActions.Actions.moveing)
        {
            Think();
        }
    }
    /// <summary>
    /// Enemy thought process and decsion making
    /// </summary>
    virtual protected void Think()
    {
        
        bool shouldAttack = (!actions.GetAttackCooldown() && Vector3.Distance(target.position, transform.position) <= distanceFromTarget);
        if (shouldAttack)
        {
            actions.Stop();
            actions.Attack();
        }
        else
        {
            if (permissionToAttack)
            {
                actions.Move(target.position);
            }
            else if (!actions.IsRallying() && RallyPos != Vector3.zero)
            {
                float rallyWaitTime = Random.Range(timeToRallyMinMax.x, timeToRallyMinMax.y);
                actions.MoveToRally(RallyPos, rallyWaitTime);
            }
            
        }
      
        
    }
    public bool GetCanBeKnockedBack()
    {
        return canBeKnockedBack;
    }
    /// <summary>
    /// disable navMesh carving - used to solve arrow shooting position bug as carving would push arrows out of pos
    /// </summary>
    public void DisableCarving()
    {
        GetComponent<NavMeshObstacle>().carving = false;
    }
    /// <summary>
    /// enable navMesh carving - used to solve arrow shooting position bug as carving would push arrows out of pos
    /// </summary>
    public void EnableCarving()
    {
        GetComponent<NavMeshObstacle>().carving = true;
    }

}
