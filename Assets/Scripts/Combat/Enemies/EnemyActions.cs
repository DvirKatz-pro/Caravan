using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] protected Transform proxy;
    [SerializeField] protected Transform model;

    [SerializeField] protected ParticleSystem preAttackParticle;

    protected EnemyController controller;
    protected EnemyStatus status;

    protected Animator animator;
    protected NavMeshAgent agent;
    protected NavMeshObstacle obstacle;

    private EnemyManager manager;

    protected Transform player;

    [SerializeField] protected float attackWarmUpTime;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected float postAttackTime;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackCooldownTime;
    [SerializeField] protected float attackAngle;
    [SerializeField] protected float attackRange;

    [SerializeField] protected float stunnedDuration;
    [SerializeField] protected float stunnedCooldown;

    [SerializeField] protected float strafeTime;
    private float currentStrafeTime;

    protected bool attacking = false;
    protected bool isStunned = false;
    protected bool onAttackCooldown = false;



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
        
    }
    protected virtual void FixedUpdate()
    {
        model.position = Vector3.Lerp(model.position, proxy.position, Time.deltaTime * 2);
        model.rotation = proxy.rotation;
        proxy.transform.LookAt(player.position);
 
    }
    
    public void move(Vector3 position)
    {
        currentAction = Actions.moveing;
        if (agent.enabled == false)
        {
            obstacle.enabled = false;
            agent.enabled = true;
            agent.gameObject.transform.position = model.position + model.transform.forward;
            
            
        }
        setAnimation("Moving");
        agent.SetDestination(position);
        
       
        
        
    }
    
    public void stop()
    {
        if (agent.enabled == true)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.enabled = false;
            obstacle.enabled = true;
        }

    }
    public void attack()
    {
        if (currentAction != Actions.attacking && currentAction != Actions.stunned && !onAttackCooldown)
        {
            currentAction = Actions.attacking;
            onAttackCooldown = true;

            StartCoroutine(preAttack());
        }
    }
    /*
    public void strafe()
    {
        if (currentAction != Actions.strafing)
        {
            setAnimation("Moving");
            int leftOrRight = Random.Range(0, 2);
            StartCoroutine(onStrafe(leftOrRight));

        }
            
        
    }
    */
    /*
    IEnumerator onStrafe(int leftOrRight)
    {
        
        while (currentStrafeTime < strafeTime)
        {
            setAnimation("Moving");

            if (leftOrRight == 0)
            {
                agent.Move(Vector3.left * Time.deltaTime);
            }
            else
            {
                agent.Move(Vector3.right * Time.deltaTime);
            }
            currentStrafeTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        currentStrafeTime = 0;
        setAnimation("Idle");
        yield return new WaitForSeconds(5);
        currentAction = Actions.idle;

    }
    */
    IEnumerator onWait()
    {
        yield return new WaitForSeconds(1000);
    }
    protected virtual IEnumerator preAttack()
    {
        model.transform.LookAt(player);
        proxy.transform.LookAt(player);
        preAttackParticle.Play();
        setAnimation("Idle");
        yield return new WaitForSeconds(attackWarmUpTime);
        StartCoroutine(attackEnum());
    }
    protected virtual IEnumerator attackEnum()
    {
        preAttackParticle.Stop();

        setAnimation("Basic Attack");

        yield return new WaitForSeconds(0.75f);
        dealDamage();
        stop();
        yield return new WaitForSeconds(attackDuration - 0.75f);
        

        currentAction = Actions.idle;
        setAnimation("Idle");
        yield return new WaitForSeconds(attackCooldownTime);
        onAttackCooldown = false;

    }

    public virtual void dealDamage()
    {
        Vector3 enemyPlayer = (player.position - model.transform.position);
        float angle = Vector3.Angle(model.transform.forward, enemyPlayer);
        Debug.Log(angle);
        if (angle <= attackAngle && Vector3.Distance(player.position, model.transform.position) <= attackRange)
        {
            player.GetComponent<PlayerStatus>().takeDamage(attackDamage);
        }
    }

    public void stunned()
    {
        if (!isStunned && currentAction != Actions.attacking)
        {
            isStunned = true;
            setAnimation("Stunned",true);
            currentAction = Actions.stunned;
            StartCoroutine(stunnedAction());
        }
    }

    IEnumerator stunnedAction()
    {
        yield return new WaitForSeconds(stunnedDuration);
        currentAction = Actions.idle;
        setAnimation("Stunned", false);
        setAnimation("Idle");
        yield return new WaitForSeconds(stunnedCooldown);
        isStunned = false;
    }

    public void onDeath()
    {
        setAnimation("Death");
        model.GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        obstacle.enabled = false;
        currentAction = Actions.dead;
        controller.enabled = false;
        status.enabled = false;
        this.enabled = false;
        manager.unRegistar(this.gameObject);
    }

    protected void setAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    private void setAnimation(string name,bool state)
    {
        animator.SetBool(name,state);
    }

    public Actions getAction()
    {
        return currentAction;
    }
    public bool attackCooldown()
    {
        return onAttackCooldown;
    }



}
