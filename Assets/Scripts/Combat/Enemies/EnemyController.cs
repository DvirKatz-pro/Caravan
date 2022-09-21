using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] protected Vector2 timeToRallyMinMax;

    //Needed components
    protected Transform target;
    protected EnemyManager manager;
    protected EnemyActions actions;

    protected bool onCooldown = false;

    protected Vector3 avoidVec = Vector3.zero;

    private float rallyWaitTime;
    private float currentRallyWaitTime;

    private bool isRallying = false;
    public Vector3 RallyPos { get; set; }
    
   
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
        
        bool shouldAttack = (!onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        if (shouldAttack)
        {
            actions.Stop();
            actions.Attack();
        }
        else
        {
            if (!isRallying)
            {
                MoveToRally(RallyPos);
            }
        }
      
        
    }
    /// <summary>
    /// disable navMesh carving - used to solve arrow shooting position bug as carving would push arrows out of pos
    /// </summary>
    public void DisableCarving()
    {
        proxy.GetComponent<NavMeshObstacle>().carving = false;
    }
    /// <summary>
    /// enable navMesh carving - used to solve arrow shooting position bug as carving would push arrows out of pos
    /// </summary>
    public void EnableCarving()
    {
        proxy.GetComponent<NavMeshObstacle>().carving = true;
    }
    public void MoveToRally(Vector3 rallyPos)
    {
        
            isRallying = true;
            rallyWaitTime = Random.Range(timeToRallyMinMax.x, timeToRallyMinMax.y);

            StartCoroutine(OnMoveToRally(rallyPos));
    }

    private IEnumerator OnMoveToRally(Vector3 rallyPosition)
    {
        while (currentRallyWaitTime < rallyWaitTime)
        {
            actions.Move(rallyPosition);
            currentRallyWaitTime += Time.deltaTime;
            yield return null;
        }
        isRallying = false;
        currentRallyWaitTime = 0;

    }

}
