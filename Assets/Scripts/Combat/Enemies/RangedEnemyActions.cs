using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing available Ranged Enemy actions
/// </summary>
public class RangedEnemyActions : EnemyActions
{
    //Needed Gameobjects
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowPosition;
    [SerializeField] private GameObject secoundryAttackParticle;

    //Gameplay related Values
    [SerializeField] private float secoundryAttackDuration;
    [SerializeField] private float arrowSpeed;

    protected Ray sightRay;
    
    protected GameObject arrowInstance;

    protected override void FixedUpdate()
    {
        if (currentAction != Actions.attacking)
        {
            base.FixedUpdate();
        }
    }
    protected override IEnumerator PreAttack()
    {
        SetAnimation("Draw");
        Vector3 fireDirection;
        //Try to Loose where the player will be rather than his current position
        if (player.GetComponent<CharacterAreaController>().GetState() == CharacterAreaController.State.moveing)
        {
            fireDirection = player.forward.normalized * Random.Range(4.5f, 7f) + player.position - model.transform.position;
        }
        else
        {
            fireDirection = player.position - model.transform.position;
        }
        model.transform.forward = fireDirection.normalized;
        //Vector3 rotatedVector = Quaternion.AngleAxis(45, Vector3.up) * fireDirection.normalized;
        //float amont = Vector3.SignedAngle(model.transform.forward, rotatedVector, Vector3.up);
        
        //model.Rotate(0, amont, 0);

        //Loose an arrow
        arrowInstance = Instantiate(arrow, arrowPosition);
        arrowInstance.GetComponent<Collider>().enabled = false;
        arrowInstance.GetComponent<ArrowCollision>().SetDamage(attackDamage);
        Vector3 arrowAngles = arrowInstance.transform.rotation.eulerAngles;
        arrowAngles.y = 180;
        arrowInstance.transform.Rotate(180, 0, 0);

        sightRay.origin = new Vector3(model.transform.position.x, model.transform.position.y + 0.5f, model.transform.position.z);
        //set the Ray direction
        sightRay.direction = fireDirection.normalized * 50;


        yield return new WaitForSeconds(attackWarmUpTime - 0.5f);
        preAttackParticle.Play();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(OnAttack());
    }

   

    protected override IEnumerator OnAttack()
    {
        preAttackParticle.Stop();
        SetAnimation("Basic Attack");
        

        RaycastHit hit;
        if (Physics.Raycast(sightRay, out hit, Mathf.Infinity))
        {
            //We dont want projectiles to hit other enemies but if we are going to we Loose a secoundry attack which will be Loosed "Up" and land on the players position
            if (hit.transform.gameObject.tag == "Enemy")
            {
                Vector3 firePosition;
                if (player.GetComponent<CharacterAreaController>().GetState() == CharacterAreaController.State.moveing)
                {
                    firePosition = player.forward.normalized * Random.Range(4.5f, 7f) + player.position;
                }
                else
                {
                    firePosition = player.transform.position;
                }

                firePosition.y += 0.1f;
                GameObject particleInstance = Instantiate(secoundryAttackParticle, firePosition, Quaternion.identity);
                particleInstance.GetComponent<ParticleSystem>().Play();
                firePosition.y += 10;
                Vector3 force = Vector3.up * arrowSpeed;
                arrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                yield return new WaitForSeconds(secoundryAttackDuration);
                Destroy(arrowInstance);
                
                GameObject newArrowInstance = Instantiate(arrow, firePosition, Quaternion.LookRotation(firePosition));

                force = Vector3.down * arrowSpeed;
                newArrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                particleInstance.GetComponent<ParticleSystem>().Stop();
                Destroy(particleInstance);
            }
            //otherwise we fire normally
            else
            {
                Vector3 force = sightRay.direction.normalized * arrowSpeed;
                arrowInstance.transform.SetParent(null, true);
                //loose arrow at player
                arrowInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                arrowInstance.GetComponent<Collider>().enabled = true;

                yield return new WaitForSeconds(attackDuration);
            }
           


        }
        
        Vector3 enemyPlayer = player.position - model.transform.position;
        Vector3 rotatedVector = Quaternion.AngleAxis(45, Vector3.up) * enemyPlayer.normalized;
        float amont = Vector3.SignedAngle(model.transform.forward, rotatedVector, Vector3.up);

        model.Rotate(0, amont, 0);

        currentAction = Actions.idle;
        SetAnimation("Idle");
        yield return new WaitForSeconds(attackCooldownTime);
        onAttackCooldown = false;

    }

}
