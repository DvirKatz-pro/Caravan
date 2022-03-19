using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBasicAttack : MonoBehaviour
{
    //components we need
    CharacterAreaController controller;
    Animator anim;
    private CharacterMovement movement;

    //combo related values
    private float comboResetTimer = 0;
    [SerializeField] private float attackPauseTime = 1f;
    [SerializeField] private float comboPauseTime = 2f;
    [SerializeField] private float comboResetTime = 1.5f;

    //combo and the maxCombo
    private int combo = 0;
    private readonly int maxCombo = 4;

    //Attack values
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackHitAngle;

    [SerializeField] private GameObject sword;

    //reference to attack particles
    ////[SerializeField] private GameObject attackParticleGameobject;
    //private ParticleSystem attackParticle;

    //check if the player can attack
    private bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterAreaController>();
        if (controller == null)
        {
            Debug.LogError("Character controller is null");
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animation Controller is null");
        }
        /*
        if (attackParticleGameobject == null)
        {
            Debug.LogError("Basic Attack Particle Gameobject is null");
        }
        attackParticle = attackParticleGameobject.GetComponent<ParticleSystem>();
        if (attackParticle == null)
        {
            Debug.LogError("Basic Attack Particle component is null");
        }
        */
        movement = GetComponent<CharacterMovement>();
        if (movement == null)
        {
            Debug.LogError("CharacterMovement not found");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (combo > 0 && combo < maxCombo)
        {
            comboReset();
        }
    }
    public void handleBasicAttack()
    {
        //if we are trying to attack and we can attack, then we attack
        if (Input.GetMouseButton(0) && canAttack)
        {
            
            //disable
            canAttack = false;
            //reset the combo timer
            comboResetTimer = comboResetTime;
            controller.changeState(CharacterAreaController.State.basicAttack);
            //up our combo
            combo++;
            movement.rotate();
            anim.SetInteger("Combo", combo);
           //attackParticle.Play();
            checkDamage();
            //pause depending on current combo
            if (combo >= maxCombo)
            {
                StartCoroutine(attackPause(comboPauseTime,true));
            }
            else
            {
                StartCoroutine(attackPause(attackPauseTime,false));
            }
            
        }
    }
    //how long should we pause for
    IEnumerator attackPause(float pauseTime,bool shouldReset)
    {
        //can we attack and should we reset our combo
        yield return new WaitForSeconds(pauseTime);
       // attackParticle.Stop();
        if (shouldReset)
        {
            reset();
        }
        else
        {
            canAttack = true;
            controller.changeState(CharacterAreaController.State.idle);
        }
        
    }
    //reset everything 
    private void reset()
    {
        combo = 0;
        anim.SetInteger("Combo", combo);
        anim.SetTrigger("Reset");
        
        controller.changeState(CharacterAreaController.State.idle);
        canAttack = true;
        comboResetTimer = 0;
    }
    //reset the combo back to 0 after a certain amount of time
    private void comboReset()
    {
        if (comboResetTimer > 0)
        {
            comboResetTimer -= Time.deltaTime;
        }
        else
        {
            if (combo > 0)
            {
                reset();
            }
            
        }
    }

    private void checkDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider c in hitColliders)
        {
            if (c.gameObject.tag.Equals("Enemy"))
            {
                
                Vector3 targetDir = c.transform.position - transform.position;
                targetDir = targetDir.normalized;
                float angle = Vector3.Dot(targetDir, transform.forward);
                if (angle > attackHitAngle)
                {
                    c.transform.root.gameObject.GetComponent<EnemyStatus>().takeDamage(attackDamage);
                }
            }
        }
    }
            /*
            public void handleBasicAttack()
            {
                if (Input.GetMouseButton(0))
                {
                    GameObject[,] nodes = gridNode.getNodes();
                    GameObject currentNode = controller.getCurrentNode();
                    Vector2 currentNodePosition = new Vector2(currentNode.GetComponent<Node>().getGridX(), currentNode.GetComponent<Node>().getGridY());

                    controller.changeState(CharacterAreaController.State.basicAttack);

                    Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
                    Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    Vector2 mousePlayer = mouseOnScreen - positionOnScreen;
                    float angle = Vector2.SignedAngle(Vector2.up, mousePlayer);
                    CharacterAreaController.Directions attackDirection = controller.getDirection(angle);
                    controller.setDirection(attackDirection);

                    switch (attackDirection)
                    {
                        case CharacterAreaController.Directions.North:
                            nodes[(int)currentNodePosition.x+1, (int)currentNodePosition.y+1].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;
                        case CharacterAreaController.Directions.NorthEast:
                            nodes[(int)currentNodePosition.x, (int)currentNodePosition.y + 1].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;

                        case CharacterAreaController.Directions.East:
                            nodes[(int)currentNodePosition.x - 1, (int)currentNodePosition.y + 1].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;


                        case CharacterAreaController.Directions.SouthEast:
                            nodes[(int)currentNodePosition.x - 1, (int)currentNodePosition.y].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;


                        case CharacterAreaController.Directions.South:
                            nodes[(int)currentNodePosition.x - 1, (int)currentNodePosition.y - 1].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;

                        case CharacterAreaController.Directions.SouthWest:
                            nodes[(int)currentNodePosition.x, (int)currentNodePosition.y - 1].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;

                        case CharacterAreaController.Directions.West:
                            nodes[(int)currentNodePosition.x + 1, (int)currentNodePosition.y - 1].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;



                        case CharacterAreaController.Directions.NorthWest:
                            nodes[(int)currentNodePosition.x + 1, (int)currentNodePosition.y].GetComponent<SpriteRenderer>().color = nodeAttackColor;
                            break;
                        default:
                            break;


                    }


                    controller.changeState(CharacterAreaController.State.idle);


                }

            }
            */
        }
