using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Transform target;
    protected EnemyManager manager;

    [SerializeField] protected Transform proxy;
    [SerializeField] protected Transform model;


    [SerializeField] protected float distanceFromTarget;


    protected EnemyActions actions;


    protected bool onCooldown = false;

    protected Vector3 avoidVec = Vector3.zero;

    
   
    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();

        actions = GetComponent<EnemyActions>();

        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        manager.registar(this.gameObject);
        
    }
    

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (actions.getAction() != EnemyActions.Actions.stunned && actions.getAction() != EnemyActions.Actions.dead)
        {
            think();
        }
    }

    virtual protected void think()
    {
        
        
       
        bool shouldAttack = (!onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        bool shouldWait = (onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        if (shouldAttack)
        {
            actions.attack();
        }
        else if(shouldWait)
        {
            actions.stop();
        }
        else
        {
            actions.move(target.position);
        }
        
    }


    
    
   
}
