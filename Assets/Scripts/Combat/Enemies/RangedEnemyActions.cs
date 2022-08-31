using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    protected override void Update()
    {
        if (currentAction != Actions.attacking)
        {
           base.Update();
        }
        
    }

    #region Attack
    
    protected override IEnumerator PreAttack()
    {
        Vector3 fireDirection = player.position - model.transform.position;
        SetAnimation("Draw");
        //Try to Loose where the player will be rather than his current position
        if (player.GetComponent<CharacterAreaController>().GetState() == CharacterAreaController.State.moveing)
        {
            fireDirection = player.forward.normalized * Random.Range(4.5f, 7f) + player.position - model.transform.position;
        }

        //create an arrow
        controller.DisableCarving();
        arrowInstance = Instantiate(arrow, arrowPosition);
        arrowInstance.GetComponent<ArrowCollision>().SetDamage(attackDamage);
        Vector3 arrowAngles = arrowInstance.transform.rotation.eulerAngles;
        arrowAngles.y = 180;
        arrowInstance.transform.Rotate(180, 0, 0);

        model.transform.forward = Quaternion.AngleAxis(45, Vector3.up) * fireDirection;

        sightRay.origin = new Vector3(model.transform.position.x, model.transform.position.y + 0.5f, model.transform.position.z);
        //set the Ray direction
        sightRay.direction = fireDirection * 50;


        yield return new WaitForSeconds(1 - 0.5f);
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
            Vector3 force = Vector3.zero;
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

                GameObject particleInstance = Instantiate(secoundryAttackParticle, firePosition, Quaternion.identity);
                particleInstance.GetComponent<ParticleSystem>().Play();
                firePosition.y += 10;
                force = Vector3.up;
                arrowInstance.GetComponent<Rigidbody>().AddForce(force * Time.deltaTime * arrowSpeed, ForceMode.Impulse);
                controller.EnableCarving();
                yield return new WaitForSeconds(secoundryAttackDuration);
                Destroy(arrowInstance);
                
                GameObject newArrowInstance = Instantiate(arrow, firePosition, Quaternion.LookRotation(firePosition));
                force = Vector3.down;
                newArrowInstance.GetComponent<Rigidbody>().AddForce(force * Time.deltaTime * arrowSpeed, ForceMode.Impulse);
                controller.EnableCarving();
                particleInstance.GetComponent<ParticleSystem>().Stop();
                Destroy(particleInstance);
            }
            //otherwise we fire normally
            else
            {
                
                force = sightRay.direction;
                arrowInstance.transform.SetParent(null, true);
                //loose arrow at player
                arrowInstance.GetComponent<Rigidbody>().AddForce(force * Time.deltaTime * arrowSpeed, ForceMode.Impulse);
                controller.EnableCarving();
                yield return new WaitForSeconds(attackDuration);
            }
           


        }

        model.transform.forward = Quaternion.AngleAxis(-45, Vector3.up) * sightRay.direction;
        
        currentAction = Actions.idle;
        SetAnimation("Idle");
        yield return new WaitForSeconds(attackCooldownTime);
        onAttackCooldown = false;
    }
    
    #endregion
    
}
