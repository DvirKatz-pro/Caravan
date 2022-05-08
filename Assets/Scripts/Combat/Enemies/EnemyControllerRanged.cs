﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class incharge of arrow behaviour
/// </summary>
public class EnemyControllerRanged : EnemyController
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Think()
    {
       
        onCooldown = actions.AttackCooldown();

        bool shouldAttack = (!onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        bool shouldWait = (onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);


        if (shouldAttack || actions.GetAction() == EnemyActions.Actions.attacking)
        {
            actions.Stop();
            actions.Attack();
        }
        else if (shouldWait)
        {
            actions.Stop();
        }
        else
        {
            actions.Move(target.position);
        }

    }
}




