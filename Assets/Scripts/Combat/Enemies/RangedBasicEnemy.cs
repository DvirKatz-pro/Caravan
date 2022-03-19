using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBasicEnemy : Enemy
{
    //References needed to shoot arrow
    protected Ray sightRay;
    [SerializeField] private float rotateTime;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private Transform arrowPosition;
    [SerializeField] private ParticleSystem preFireParticle;
    [SerializeField] private GameObject secoundryAttackParticle;
    [SerializeField] private GameObject arrow;
    private float rotateTimer;

    public override void Start()
    {
        //set this enemy type
        base.type = EnemyType.Ranged;
        base.Start();
    }
    #region attack
    public override IEnumerator attack()
    {
        //disable movement and get enemy->player vector
        
        agent.enabled = false;
        transform.LookAt(player);
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPlayer = playerPosition - transform.position;
        
        sightRay.origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

       
        //set the Ray direction
        sightRay.direction = enemyPlayer.normalized * (agent.stoppingDistance);
        RaycastHit hit;
        
       
        attacking = true;
        
        //animate arrow draw
        animator.SetBool("Idle", false);
        animator.SetBool("Arrow Draw", true);
        //rotate so that the enemy is aiming at the player
        Vector3 rotatedVector = Quaternion.AngleAxis(45, Vector3.up) * enemyPlayer.normalized;
        float amont = Vector3.SignedAngle(transform.forward, rotatedVector,Vector3.up);
        transform.Rotate(0, amont, 0);
        //instantiate an arrow and set it
        GameObject arrowInstance = Instantiate(arrow, arrowPosition);
        arrowInstance.GetComponent<Collider>().enabled = false;
        arrowInstance.GetComponent<ArrowCollision>().setDamage(attackDamage);
        Vector3 arrowAngles = arrowInstance.transform.rotation.eulerAngles;
        arrowAngles.y = 180;
        arrowInstance.transform.Rotate(180, 0, 0);
        //pre fire pause
        yield return new WaitForSeconds(attackWarmUp - 0.5f);
        preFireParticle.Play();
        yield return new WaitForSeconds(0.5f);
        //attack

        animator.SetBool("Arrow Draw", false);
        animator.SetBool("Basic Attack", true);
        //check raycast
        if (Physics.Raycast(sightRay, out hit, Mathf.Infinity))
        {
            //if we hit an enemy then the archer will fire "up" and the arrow will come down on top of the player
            if (hit.transform.gameObject.tag == "Enemy")
            {

                GameObject particleInstance = Instantiate(secoundryAttackParticle, playerPosition, Quaternion.identity);
                particleInstance.GetComponent<ParticleSystem>().Play();
                playerPosition.y += 10;
                Vector3 force = Vector3.up * arrowSpeed;
                arrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                yield return new WaitForSeconds(1);
                Destroy(arrowInstance);

                GameObject newArrowInstance = Instantiate(arrow, playerPosition, Quaternion.LookRotation(playerPosition));

                force = Vector3.down * arrowSpeed;
                newArrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                particleInstance.GetComponent<ParticleSystem>().Stop();
                Destroy(particleInstance);
            }
            //otherwise we fire normally
            else
            {
                Vector3 force = enemyPlayer.normalized * arrowSpeed;
                arrowInstance.transform.SetParent(null, true);
                //loose arrow at player
                arrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                arrowInstance.GetComponent<Collider>().enabled = true;
            }
        }
        //if the raycast didnt hit anything fire anyway
        else
        {
            Vector3 force = enemyPlayer.normalized * arrowSpeed;
            arrowInstance.transform.SetParent(null, true);
            //loose arrow at player
            arrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            arrowInstance.GetComponent<Collider>().enabled = true;
        }
                
            //post attack pause

        yield return new WaitForSeconds(postAttack);

        //go back to Idle
        animator.SetBool("Idle", true);
        animator.SetBool("Basic Attack", false);
        changeState(State.Idle);
        amont = Vector3.SignedAngle(transform.forward, enemyPlayer.normalized, Vector3.up);
        transform.Rotate(0, amont, 0);
        yield return new WaitForSeconds(attackCooldown);
                
        attacking = false;
        agent.enabled = true;

            
            
        

    }
    #endregion
    public IEnumerator rotate(float amount)
    {
        float amountOverTime = amount * (Time.deltaTime/rotateTime);
        while (rotateTimer < rotateTime)
        {
            transform.Rotate(new Vector3(0,amountOverTime,0));
            yield return new WaitForEndOfFrame();
            rotateTimer += Time.deltaTime;
        }
        rotateTimer = 0;
    }
   
    

}
