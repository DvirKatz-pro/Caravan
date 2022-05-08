﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class Detailing Player basic attack
/// </summary>
public class PlayerBasicAttack : MonoBehaviour
{
    //components we need
    private CharacterAreaController controller;
    private Animator anim;
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
    private void Start()
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
    private void Update()
    {
        if (combo > 0 && combo < maxCombo)
        {
            ComboReset();
        }
    }
    public void BasicAttack()
    {
        //if we are trying to attack and we can attack, then we attack
        if (Input.GetMouseButton(0) && canAttack)
        {
            
            //disable 
            canAttack = false;
            //reset the combo timer
            comboResetTimer = comboResetTime;
            controller.ChangeState(CharacterAreaController.State.basicAttack);
            //up our combo
            combo++;
            movement.Rotate();
            anim.SetInteger("Combo", combo);
           //attackParticle.Play();
            checkDamage();
            //pause depending on current combo
            if (combo >= maxCombo)
            {
                StartCoroutine(AttackPause(comboPauseTime,true));
            }
            else
            {
                StartCoroutine(AttackPause(attackPauseTime,false));
            }
            
        }
    }
    // <summary>
    /// how long should we pause for to know if we keep the combo or reset ourselves to idle
    /// </summary>
    private IEnumerator AttackPause(float pauseTime,bool shouldReset)
    {
        //can we attack and should we reset our combo
        yield return new WaitForSeconds(pauseTime);
       // attackParticle.Stop();
        if (shouldReset)
        {
            Reset();
        }
        else
        {
            canAttack = true;
            controller.ChangeState(CharacterAreaController.State.idle);
        }
        
    }
    // <summary>
    /// Reset Combo and go back to Idle
    /// </summary>
    private void Reset()
    {
        combo = 0;
        anim.SetInteger("Combo", combo);
        anim.SetTrigger("Reset");
        
        controller.ChangeState(CharacterAreaController.State.idle);
        canAttack = true;
        comboResetTimer = 0;
    }
    private void ComboReset()
    {
        if (comboResetTimer > 0)
        {
            comboResetTimer -= Time.deltaTime;
        }
        else
        {
            if (combo > 0)
            {
                Reset();
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
                    c.transform.root.gameObject.GetComponent<EnemyStatus>().TakeDamage(attackDamage);
                }
            }
        }
    }  
}
