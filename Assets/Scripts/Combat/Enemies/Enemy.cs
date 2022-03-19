using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Player reference
    protected Transform player;
    //Enemy manager reference
    private EnemyManager manager;

    //Reference to Particle System
    [SerializeField] private ParticleSystem preAttackParticle;
    //gameplay attack values
    [SerializeField] protected float distanceFromPlayer;
    [SerializeField] protected float attackWarmUp;
    [SerializeField] protected float postAttack;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackAngle;
    [SerializeField] private float attackRange;


    //take Damage
    [SerializeField] protected GameObject colorModel;
    [SerializeField] protected float colorTimer;
    protected float currentColorTimer;

    //Health
    [SerializeField] protected float health;
    [SerializeField] protected float stunnedCooldown;
    [SerializeField] protected float stunnedDuration;


    //Animator and Navmesh
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Avoider avoider;

    private bool isDead = false;
    private bool isStunned = false;
    private bool canBeStunned = true;

    //to know if enemy is already canAttack
    protected bool attacking = false;


    //keep track of enemy state
    public enum State
    {
        Idle,
        Moving,
        WaintingToAttack,
        Avoiding,
        Attacking,
        Stunned,
        Dead
    };

    public enum EnemyType
    {
        Close,
        Ranged
    };

    State currentState = State.Idle;
    protected EnemyType type = EnemyType.Close;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        if (player == null)
        {
            Debug.LogError("Player Not found");
        }
        transform.LookAt(player.transform.position);
        
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Nav mesh agent is null");
        }
        agent.updateRotation = false;

        avoider = GetComponent<Avoider>();
        if (agent == null)
        {
            Debug.LogError("Avoider not found");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is null");
        }
        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        if (manager == null)
        {
            Debug.LogError("Enemy manager not found");
        }

    }

    protected void FixedUpdate()
    {
        controller();
    }
    public virtual void controller()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > distanceFromPlayer && currentState != State.Attacking && currentState != State.Stunned)
        {
            changeState(State.Moving);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= distanceFromPlayer && currentState != State.Attacking && currentState != State.Stunned)
        {
            /*
            if (manager.permissionToAttack(this))
            {
                changeState(State.Attacking);
            }
            else
            {
                changeState(State.WaintingToAttack);
            }
            */
           
        }
        else if(currentState != State.Attacking && currentState != State.Stunned)
        {
           changeState(State.Idle);
        }

        //do stuff depending on current state
        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("Idle", true);
                animator.SetBool("Moving", false);
                transform.LookAt(player.position);

                break;
            case State.Moving:
                move(player.transform.position);
                break;
            case State.Attacking:

                if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
                {
                    move(player.transform.position);
                }
                else if (!attacking)
                {
                    stop();
                    StartCoroutine(attack());
                }
                break;
            case State.WaintingToAttack:
                {
                   
                    if (avoider.avoidEnemy != null)
                    {
                        Vector3 moveVec = avoider.avoidEnemy.transform.position - transform.position;
                        Vector3 distVec = (player.transform.position - transform.position);
                        moveVec = Vector3.Slerp(distVec.normalized, moveVec.normalized, 3);
                        move(moveVec);
                    }
                    else
                    {
                        stop();
                        animator.SetBool("Idle", true);
                        animator.SetBool("Moving", false);
                    }
                    
                    break;
                }
                
            default:
                break;
        }
       
    }
    public virtual void changeState(State state)
    {
        currentState = state;
    }
    #region move&attack
    public virtual void move(Vector3 position)
    {
        agent.SetDestination(position);
        transform.LookAt(player);
        //move
        
        animator.SetBool("Moving", true);
        animator.SetBool("Idle", false);
       
    }
    public virtual void stop()
    {
        agent.SetDestination(transform.position);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }
    public virtual IEnumerator attack()
    {
        if (!isStunned)
        {
            
            attacking = true;

            //pre attack pause
            preAttackParticle.Play();
            animator.SetBool("Moving", false);
            animator.SetBool("Idle", false);

            yield return new WaitForSeconds(attackWarmUp);
            


            preAttackParticle.Stop();
            //attack
            animator.SetBool("Moving", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Basic Attack", true);
            yield return new WaitForSeconds(0.8f);
            dealDamage();
            //go back to Idle


           
            //post attack pause
            yield return new WaitForSeconds(postAttack);

           
            animator.SetBool("Idle", true);
            animator.SetBool("Basic Attack", false);
            
            changeState(State.Idle);

            yield return new WaitForSeconds(attackCooldown);
            attacking = false;
            manager.doneAttacking();

        }
        
    }
    #endregion
    #region health&damage
    public virtual void takeDamage(float amount)
    {
        if (!isDead)
        {
            health -= amount;
           
            StartCoroutine(takeDamageTimer());
            if (health <= 0)
            {
                isDead = true;
                health = 0;
                death();
            }
            else
            {
                if (canBeStunned && !attacking)
                {
                    isStunned = true;
                    canBeStunned = false;
                    changeState(State.Stunned);
                    animator.SetTrigger("Stunned");
                    StartCoroutine(stunned());
                }
            }
        }
    }
    public virtual IEnumerator stunned()
    {
        if (!isDead)
        {
            
  
            animator.SetBool("Idle", false);
            animator.SetBool("Moving", false);
            animator.SetBool("Basic Attack", false);
            
            yield return new WaitForSeconds(stunnedDuration);
            changeState(State.Idle);
            isStunned = false;
            yield return new WaitForSeconds(stunnedCooldown);

            canBeStunned = true;
            
        }
        
    }
    public virtual IEnumerator takeDamageTimer()
    {
        Color startColor = colorModel.GetComponent<SkinnedMeshRenderer>().material.color;
        Color currentColor = startColor;
        Color endColor = Color.red;
        while (currentColor != endColor)
        {
            currentColor = Color.Lerp(currentColor, endColor, Time.deltaTime / 0.03f);
            colorModel.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
            yield return new WaitForEndOfFrame();
        }
        while (currentColor != startColor)
        {
            currentColor = Color.Lerp(currentColor, startColor, Time.deltaTime / 0.03f);
            colorModel.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
            yield return new WaitForEndOfFrame();
        }

        


    }
    
    public virtual void dealDamage()
    {
        Vector3 enemyPlayer = (player.position - transform.position);
        float angle = Vector3.Angle(transform.forward, enemyPlayer);
        
        if (angle <= attackAngle && Vector3.Distance(player.position,transform.position) <= attackRange)
        {
            player.GetComponent<PlayerStatus>().takeDamage(attackDamage);
        }
    }
    public virtual void death()
    {
        changeState(State.Dead);
        animator.SetTrigger("Death");
        GetComponent<Collider>().enabled = false;
        stop();
        agent.updateRotation = false;
        agent.enabled = false;

    }
    #endregion
    #region getInfo
    public virtual EnemyType getType()
    {
        return type;
    }

    public NavMeshAgent getNavAgent()
    {
        return agent;
    }
   
    public bool isAttacking()
    {
        return attacking;
    }
    public State getState()
    {
        return currentState;
    }
   
    #endregion


}
