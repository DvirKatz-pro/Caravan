using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class incharge of Enemy Descsion and action taking
/// </summary>
public class EnemyController : MonoBehaviour
{
    //Needed Objects
    [SerializeField] protected Transform proxy;
    [SerializeField] protected Transform model;

    //Gameplay related values
    [SerializeField] protected float distanceFromTarget;

    //Needed components
    protected Transform target;
    protected EnemyManager manager;
    protected EnemyActions actions;

    protected bool onCooldown = false;

    protected Vector3 avoidVec = Vector3.zero;

    
   
    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();

        actions = GetComponent<EnemyActions>();

        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        manager.Registar(this.gameObject);
        
    }
    

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (actions.GetAction() != EnemyActions.Actions.stunned && actions.GetAction() != EnemyActions.Actions.dead)
        {
            Think();
        }
    }
    /// <summary>
    /// Enemy thought process and decsion making
    /// </summary>
    virtual protected void Think()
    {
        
        bool shouldAttack = (!onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        bool shouldWait = (onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        if (shouldAttack)
        {
            actions.Attack();
        }
        else if(shouldWait)
        {
            actions.Stop();
        }
        else
        {
            actions.Move(target.position);
        }
        
    }
   
}
