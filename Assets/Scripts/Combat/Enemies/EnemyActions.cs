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
    [Range(0.01f, 1f)]
    [SerializeField] protected float attackDot;
    [SerializeField] protected float attackRange;

    [SerializeField] protected float stunnedDuration;
    [SerializeField] protected float stunnedCooldown;

    [SerializeField] protected float MaxRallyDistanceFromPlayer;
    [SerializeField] protected float strafeTime;

    //Needed components
    protected EnemyController controller;
    protected EnemyStatus status;
    protected Animator animator;
    protected NavMeshAgent agent;
    private EnemyManager manager;

    //PlayerComponents
    protected GameObject player;
    protected CharacterMovement playerMovement;
    protected PlayerStatus playerStatus;
    protected CharacterAreaController playerAreaController;

    //Track actions
    protected bool attacking = false;
    protected bool isStunned = false;
    protected bool onAttackCooldown = false;
    protected bool isRallying = false;
    protected bool isMoving = false;

    private IEnumerator OnMoveOverTimeRoutine;


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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterController characterController = hit.gameObject.GetComponent<CharacterController>();
        if (characterController == null)
        {
            return;
        }
        //Stop moving if hitting another characterController
        StopCoroutine(OnMoveOverTimeRoutine);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        status = GetComponent<EnemyStatus>();
        manager = EnemyManager.Instance;
        player = controller.GetPlayer();
        playerMovement = player.GetComponent<CharacterMovement>();
        playerStatus = player.GetComponent<PlayerStatus>();
        playerAreaController = player.GetComponent<CharacterAreaController>();

    }
    protected virtual void Update()
    {
        //Look at the player until he rolls away
        if (controller.isTracking && playerAreaController.GetState() != CharacterAreaController.State.roll)
        {
            transform.LookAt(player.transform.position);
        }
        else if (controller.isTracking && playerAreaController.GetState() == CharacterAreaController.State.roll)
        {
            controller.isTracking = false;
        }
    }

    #region Actions
    /// <summary>
    /// given a position, the enemy will move to that position
    /// </summary>
    public void Move(Vector3 position)
    {
        NavMeshHit hit;
        //get nearst avilable position on the navmesh 
        if (NavMesh.SamplePosition(position, out hit, 100, NavMesh.AllAreas))
        {
            agent.ResetPath();
            currentAction = Actions.moveing;
            agent.avoidancePriority = 50;
            SetAnimation("Moving", true);
            agent.SetDestination(position);
        }
    }

    public void MoveOverTime(Vector3 direction, float distance, float duration)
    {
        OnMoveOverTimeRoutine = OnMoveOverTime(direction,distance,duration);
        StartCoroutine(OnMoveOverTimeRoutine);
    }

    /// <summary>
    /// The enemy will move without animation and without navmesh in a direction for a certain distance over time
    /// </summary>
    public IEnumerator OnMoveOverTime(Vector3 direction, float distance, float duration)
    {
        yield return null;
        Vector3 startValue = transform.position;
        Vector3 endValue = transform.position + direction.normalized * distance;
        float timeElapsed = 0;
        Vector3 valueToLerp;
        while (timeElapsed < duration)
        {
            valueToLerp = Vector3.Lerp(startValue, endValue, (timeElapsed / duration));
            agent.velocity = valueToLerp - transform.position;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        agent.velocity = valueToLerp - transform.position;
        agent.ResetPath();
        yield return null;
    }
    /// <summary>
    /// The enemy will stop at its current position
    /// </summary>
    public void Stop()
    {
        agent.enabled = true;
        agent.avoidancePriority = 45;
        agent.SetDestination(transform.position);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        
        SetAnimation("Moving", false);
        SetAnimation("Idle");
    }

    /// <summary>
    /// The enemy will move to a rally position and will not rally to another position for a certain amount of time
    /// </summary>
    public void MoveToRally(Vector3 rallyPos,float rallyWaitTime)
    {  
        isRallying = true;
        StartCoroutine(OnMoveToRally(rallyPos,rallyWaitTime));
    }
    private IEnumerator OnMoveToRally(Vector3 rallyPos,float rallyWaitTime)
    {
        yield return null;
        while (rallyWaitTime > 0 && currentAction != Actions.attacking && !controller.permissionToAttack)
        {
            //if the enemy is too far to the player, he will stop moving to current rally position
            if (Vector3.Distance(transform.position, player.transform.position) > MaxRallyDistanceFromPlayer)
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
            controller.isTracking = true;
            Stop();
            StartCoroutine(PreAttack());
        }
    }
    /// <summary>
    /// Actions to be done before the enemy attacks
    /// </summary>
    protected virtual IEnumerator PreAttack()
    {
        transform.LookAt(player.transform);
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
        MoveOverTime(transform.forward, attackTravelDistance, attackTravelDuration);
        DealDamage();
        yield return new WaitForSeconds(attackDuration);
        
        currentAction = Actions.idle;
        SetAnimation("Idle");
        controller.isTracking = false;
        yield return new WaitForSeconds(attackCooldownTime);
        onAttackCooldown = false;

    }
    /// <summary>
    /// Check if we have hit the player or not
    /// </summary>
    public virtual void DealDamage()
    {
        Vector3 enemyPlayer = (transform.position - player.transform.position);
        float enemyPlayerAngle = Vector3.Dot(enemyPlayer.normalized, player.transform.forward.normalized);
        if (enemyPlayerAngle >= attackDot && Vector3.Distance(player.transform.position, transform.position) <= attackRange && playerAreaController.canBeHit)
        {
            playerStatus.TakeDamage(attackDamage);
            StartCoroutine(playerMovement.MoveOverTime(transform.forward, attackKnockbackDistance,attackKnockbackDuration));
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
