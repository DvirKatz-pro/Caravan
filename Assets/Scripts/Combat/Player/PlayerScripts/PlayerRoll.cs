﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing Player Roll to avoid attacks
/// </summary>
public class PlayerRoll : MonoBehaviour
{
    //Roll attributes
    [SerializeField] private float rollSpeed = 5;
    [SerializeField] private float rollTime;
    private float currentRollTime = 0;
    private bool isRolling = false;

    //needed references
    CharacterAreaController controller;
    Rigidbody rb;
    Animator anim;
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterAreaController>();
        if (controller == null)
        {
            Debug.LogError("Character controller is null");
        }
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Character controller is null");
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animation Controller is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Roll()
    {
        //if we hit space and are not already rolling, then start rolling
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            isRolling = true;
            controller.ChangeState(CharacterAreaController.State.roll);
            StartCoroutine(OnRoll());
        }

    }
    private IEnumerator OnRoll()
    {
        rb.velocity = Vector3.zero;
        //animation
        anim.SetBool("Idle", false);
        anim.SetBool("Roll", true);
        anim.SetBool("Moving", false);
        Vector3 forword = transform.forward;
        forword = forword.normalized;
        //move forword for a certain amount of time based on rollTime var
        while (currentRollTime < rollTime)
        {
            //decrease speed every frame for a more natural roll
            float forcePercentage = 1 - (currentRollTime / rollTime);
            Vector3 force = forword * (rollSpeed * forcePercentage) *  Time.deltaTime;
            rb.AddForce(force, ForceMode.Impulse);
            currentRollTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //stop rolling and reset state
        rb.velocity = Vector3.zero;
        currentRollTime = 0;
        anim.SetBool("Idle", true);
        anim.SetBool("Roll", false);
        controller.ChangeState(CharacterAreaController.State.idle);
        ///yield return new WaitForSeconds(0.2f);
        isRolling = false;

    }
}
