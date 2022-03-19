using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerRanged : EnemyController
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void think()
    {
       
        onCooldown = actions.attackCooldown();

        bool shouldAttack = (!onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);
        bool shouldWait = (onCooldown && Vector3.Distance(target.position, model.position) <= distanceFromTarget);


        if (shouldAttack || actions.getAction() == EnemyActions.Actions.attacking)
        {
            actions.stop();
            actions.attack();
        }
        else if (shouldWait)
        {
            actions.stop();
        }
        else
        {
            actions.move(target.position);
        }

    }
}




