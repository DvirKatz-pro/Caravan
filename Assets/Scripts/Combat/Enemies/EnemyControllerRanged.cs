using System.Collections;
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
       
        onCooldown = actions.GetAttackCooldown();

        bool shouldAttack = (!onCooldown && Vector3.Distance(player.transform.position, transform.position) <= distanceFromTarget);
        bool shouldWait = (onCooldown && Vector3.Distance(player.transform.position, transform.position) <= distanceFromTarget);


        if (shouldAttack)
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
            actions.Move(player.transform.position);
        }
       

    }
   
}




