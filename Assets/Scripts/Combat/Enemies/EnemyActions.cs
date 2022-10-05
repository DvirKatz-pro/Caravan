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

    [SerializeField] protected float MaxRallyDistanceFromPlayer;
    [SerializeField] protected float strafeTime;

    //Needed components
    protected EnemyController controller;
    protected EnemyStatus status;
    private Rigidbody rb;

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
    protected bool isRallying = false;
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

        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        status = GetComponent<EnemyStatus>();
        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        rb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player").transform;

        path = new NavMeshPath();
        elapsed = 0.0f;

    }

    #region Actions
    /// <summary>
    /// given a position, the enemy will move to that position
    /// </summary>
    public void Move(Vector3 position)
    {
        currentAction = Actions.moveing;
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            obstacle.enabled = false;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, 100, NavMesh.AllAreas))
            {
                agent.avoidancePriority = 50;
                StartCoroutine(MoveAgent(hit.position));
            }
            else
            {
                Stop();
            }
        }
    }
    private IEnumerator MoveAgent(Vector3 position)
    {
        yield return null;
        elapsed -= 1.0f;
        agent.enabled = true;
        SetAnimation("Moving", true);
        agent.SetDestination(position);
    }
    /// <summary>
    /// given a position, the enemy will move to that position. The currentAction will not change and animation will not play
    /// </summary>
    public void MoveRaw(Vector3 position)
    {
        if (agent.enabled == false)
        {
            obstacle.enabled = false;
            agent.enabled = true;
        }
        agent.SetDestination(position);
    }

    public void MoveModel(Vector3 movement)
    {
        MoveModel(movement, 1);
    }
    public void MoveModel(Vector3 movement,float speed)
    {
        rb.velocity = (movement * Time.deltaTime * speed);
    }
    /// <summary>
    /// The enemy will stop at its current position
    /// </summary>
    public void Stop()
    {
        if (agent.enabled == true)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.enabled = false;
            obstacle.enabled = true;
        }
        agent.avoidancePriority = 45;
        SetAnimation("Moving", false);
        SetAnimation("Idle");
    }
    
    public IEnumerator MoveOverTime(Vector3 moveDirection, float distance, float moveDuration)
    {
        Vector3 startValue = transform.position;
        Vector3 endValue = transform.position + moveDirection.normalized * distance;
        Debug.DrawLine(startValue, endValue,Color.green);
        float timeElapsed = 0;
        Vector3 valueToLerp;
        while (timeElapsed < moveDuration)
        {
            valueToLerp = Vector3.Lerp(startValue, endValue, timeElapsed / moveDuration);
            MoveModel(valueToLerp - transform.position,4000);
            timeElapsed += Time.deltaTime;
            Debug.DrawLine(startValue, endValue, Color.green);
            yield return null;
        }
        valueToLerp = endValue;
        MoveModel(valueToLerp - transform.position,4000);
        Stop();
        MoveModel(valueToLerp - transform.position, 0);
    }

    public void MoveToRally(Vector3 rallyPos,float rallyWaitTime)
    {
        isRallying = true;
        StartCoroutine(OnMoveToRally(rallyPos,rallyWaitTime));
    }
    private IEnumerator OnMoveToRally(Vector3 rallyPos,float rallyWaitTime)
    {
        while (rallyWaitTime > 0)
        {
            if (Vector3.Distance(transform.position, player.position) > MaxRallyDistanceFromPlayer || Vector3.Distance(transform.position, player.position) <= attackRange + 1.0f)
            {
                rallyWaitTime = 0;
            }
            if (Vector3.Distance(transform.position, rallyPos) > 0.5f)
            {
                Move(rallyPos);
            }
            else
            {
                rallyPos = transform.position;
                Stop();
            }
            rallyWaitTime -= Time.deltaTime;
            yield return null;
        }
        isRallying=false;
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
        transform.LookAt(player);
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
        StartCoroutine(MoveOverTime(transform.forward, attackTravelDistance, attackTravelDuration));
        yield return new WaitForSeconds(0.5f);
        DealDamage();
        Stop();
        yield return new WaitForSeconds(attackDuration - 0.5f);
        

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
        Vector3 enemyPlayer = (player.position - transform.position);
        float angle = Vector3.Angle(transform.forward, enemyPlayer);
        if (angle <= attackAngle && Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            player.GetComponent<PlayerStatus>().TakeDamage(attackDamage);
            StartCoroutine(player.GetComponent<CharacterMovement>().MoveOverTime(transform.forward, attackKnockbackDistance,attackKnockbackDuration));
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
        GetComponent<Collider>().enabled = false;
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
    public bool GetAttackCooldown()
    {
        return onAttackCooldown;
    }
    public bool IsRallying()
    {
        return isRallying;
    }
    #endregion



}
