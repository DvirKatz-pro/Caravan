using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class Detailing available Enemy actions
/// </summary>
public class EnemyActions : MonoBehaviour
{
    //Needed objects
    [SerializeField] protected Transform proxy;
    [SerializeField] protected Transform model;
    [SerializeField] protected ParticleSystem preAttackParticle;

    //Gameplay related values
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float attackWarmUpTime;
    [SerializeField] protected float attackTravelDistance;
    [SerializeField] protected float attackTravelDuration;
    [SerializeField] protected float attackKnockbackDistance;
    [SerializeField] protected float attackKnockbackDuration;
    [SerializeField] protected float attackKnockbackSpeed;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected float postAttackTime;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackCooldownTime;
    [SerializeField] protected float attackAngle;
    [SerializeField] protected float attackRange;

    [SerializeField] protected float stunnedDuration;
    [SerializeField] protected float stunnedCooldown;

    [SerializeField] protected float strafeTime;

    //Needed components
    protected EnemyController controller;
    protected EnemyStatus status;

    protected Animator animator;
    protected NavMeshAgent agent;
    protected NavMeshObstacle obstacle;
    private NavMeshPath path;

    private EnemyManager manager;

    protected Transform player;

    //Track actions
    protected bool attacking = false;
    protected bool isStunned = false;
    protected bool onAttackCooldown = false;
    private float elapsed = 0.0f;


    /// <summary>
    /// The enums of actions an enemy might take
    /// </summary>
    public enum Actions
    {
        idle,
        moveing,
        attacking,
        stunned,
        strafing,
        dead
    }

    protected Actions currentAction = Actions.idle;

    // Start is called before the first frame update
    protected virtual void Start()
    {

        agent = proxy.GetComponent<NavMeshAgent>();
        obstacle = proxy.GetComponent<NavMeshObstacle>();
        animator = model.GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        status = GetComponent<EnemyStatus>();
        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        player = GameObject.Find("Player").transform;

        path = new NavMeshPath();
        elapsed = 0.0f;

    }

    protected virtual void FixedUpdate()
    {
        if (Vector3.Distance(proxy.position, model.position) >= 0.5f && currentAction == Actions.moveing)
        {
            Vector3 movement = proxy.position - model.position;
            //because the navmesh agent and model are seperate objects, the model has to move accourding to the agent
            model.GetComponent<CharacterController>().Move(movement.normalized * Time.deltaTime * movementSpeed);
            model.rotation = proxy.rotation;
            proxy.transform.LookAt(player.position);
        }
 
    }
    #region Actions
    /// <summary>
    /// given a position, the enemy will move to that position
    /// </summary>
    public void Move(Vector3 position)
    {
        
        currentAction = Actions.moveing;
        if (agent.enabled == false)
        {
            obstacle.enabled = false;
            proxy.position = model.position;
            agent.enabled = true;
        }
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            bool pathFound = NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);
            if (pathFound)
            {
                agent.SetPath(path);
                SetAnimation("Moving", true);
            }
            else
            {
                Stop();
            }
        }
        
  
    }
    /// <summary>
    /// given a position, the enemy will move to that position. The currentAction will not change and animation will not play
    /// </summary>
    public void MoveRaw(Vector3 position)
    {
        if (agent.enabled == false)
        {
            obstacle.enabled = false;
            agent.gameObject.transform.position = model.position;
            agent.enabled = true;
        }
        agent.SetDestination(position);
    }

    public void MoveModel(Vector3 movement)
    {
        model.GetComponent<CharacterController>().Move(movement);
    }
    public void MoveModel(Vector3 movement,float speed)
    {
        model.GetComponent<CharacterController>().Move(movement * Time.deltaTime * speed);
    }
    /// <summary>
    /// The enemy will stop at its current position
    /// </summary>
    public void Stop()
    {
        if (agent.enabled == true)
        {
            proxy.position = model.position;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.enabled = false;
            obstacle.enabled = true;
        }
        SetAnimation("Moving", false);

    }
    
    public IEnumerator MoveOverTime(Vector3 moveDirection, float distance, float moveDuration)
    {
        Vector3 startValue = model.position;
        Vector3 endValue = model.position + moveDirection * distance;
        Debug.DrawLine(startValue, endValue,Color.green);
        float timeElapsed = 0;
        Vector3 valueToLerp;
        while (timeElapsed < moveDuration)
        {
            valueToLerp = Vector3.Lerp(startValue, endValue, timeElapsed / moveDuration);
            MoveModel(valueToLerp - model.transform.position);
            proxy.position = model.transform.position;
            timeElapsed += Time.deltaTime;
            Debug.DrawLine(startValue, endValue, Color.green);
            yield return null;
        }
        valueToLerp = endValue;
        MoveRaw(valueToLerp - model.transform.position);
        proxy.position = model.transform.position;
    }

    /// <summary>
    /// Class detailing the conditions for whitch the enemy can attack
    /// </summary>
    public void Attack()
    {
        if (currentAction != Actions.attacking && currentAction != Actions.stunned && !onAttackCooldown)
        {
            currentAction = Actions.attacking;
            onAttackCooldown = true;

            StartCoroutine(PreAttack());
        }
    }
    /// <summary>
    /// Actions to be done before the enemy attacks
    /// </summary>
    protected virtual IEnumerator PreAttack()
    {
        model.transform.LookAt(player);
        proxy.transform.LookAt(player);
        preAttackParticle.Play();
        SetAnimation("Idle");
        yield return new WaitForSeconds(attackWarmUpTime);
        StartCoroutine(OnAttack());
    }
    /// <summary>
    /// Actions to be done during the attack itself
    /// </summary>
    protected virtual IEnumerator OnAttack()
    {
        preAttackParticle.Stop();
        SetAnimation("Basic Attack");
        Vector3 attackTravelPos = model.transform.position + model.transform.forward * attackTravelDistance;
        
        DealDamage();
        StartCoroutine(MoveOverTime(model.transform.forward, attackTravelDistance, attackTravelDuration));
        Stop();
        yield return new WaitForSeconds(attackDuration);
        

        currentAction = Actions.idle;
        SetAnimation("Idle");
        yield return new WaitForSeconds(attackCooldownTime);
        onAttackCooldown = false;

    }
    /// <summary>
    /// Check if we have hit the player or not
    /// </summary>
    public virtual void DealDamage()
    {
        Vector3 enemyPlayer = (player.position - model.transform.position);
        float angle = Vector3.Angle(model.transform.forward, enemyPlayer);
        if (angle <= attackAngle && Vector3.Distance(player.position, model.transform.position) <= attackRange)
        {
            player.GetComponent<PlayerStatus>().TakeDamage(attackDamage);
            StartCoroutine(player.GetComponent<CharacterMovement>().MoveOverTime(transform.forward.normalized, attackKnockbackDistance,attackKnockbackDuration));
        }
    }
    /// <summary>
    /// Check if the enemy can become stunned
    /// </summary>
    public void Stunned()
    {
        if (!isStunned && currentAction != Actions.attacking)
        {
            isStunned = true;
            SetAnimation("Moving", false);
            SetAnimation("Stunned",true);
            currentAction = Actions.stunned;
            StartCoroutine(OnStunned());
        }
    }
    /// <summary>
    /// the actions to be taken once the enemy is stunned
    /// </summary>
    IEnumerator OnStunned()
    {
        yield return new WaitForSeconds(stunnedDuration);
        currentAction = Actions.idle;
        SetAnimation("Stunned", false);
        yield return new WaitForSeconds(stunnedCooldown);
        isStunned = false;
    }
    /// <summary>
    /// Disable the enemy upon death
    /// </summary>
    public void OnDeath()
    {
        SetAnimation("Death",true);
        model.GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        obstacle.enabled = false;
        currentAction = Actions.dead;
        controller.enabled = false;
        status.enabled = false;
        this.enabled = false;
        manager.UnRegistar(this.gameObject);
    }
    #endregion
    #region Info and general methods
    protected void SetAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    private void SetAnimation(string name,bool state)
    {
        animator.SetBool(name,state);
    }

    public Actions GetAction()
    {
        return currentAction;
    }
    public bool AttackCooldown()
    {
        return onAttackCooldown;
    }
    #endregion



}
