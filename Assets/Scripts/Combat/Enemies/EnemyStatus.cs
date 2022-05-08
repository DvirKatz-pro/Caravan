using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class incharge of handleing the current state of the Enemy
/// </summary>
public class EnemyStatus : MonoBehaviour
{
    //Needed Gameobjects
    [SerializeField] protected GameObject colorModel;
    //Gameplay related values
    [SerializeField] private float health;
    [SerializeField] protected float colorTimer;
    //Needed componets
    EnemyActions actions;

    bool isDead = false;
    protected float currentColorTimer;

    // Start is called before the first frame update

    void Start()
    {
        actions = GetComponent<EnemyActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            actions.OnDeath();
        }
        else
        {
            actions.Stunned();
        }
        StartCoroutine(TakeDamageTimer());

    }
    /// <summary>
    /// When this enemy takes damage It will flash red for a short amount of time
    /// </summary>
    public virtual IEnumerator TakeDamageTimer()
    {
        
        Color startColor = colorModel.GetComponent<SkinnedMeshRenderer>().material.color;
        Color currentColor = startColor;
        Color endColor = Color.red;
        while (currentColor != endColor)
        {
            currentColor = Color.Lerp(currentColor, endColor, Time.deltaTime / 0.03f);
            colorModel.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
            yield return new WaitForEndOfFrame();
        }
        while (currentColor != startColor)
        {
            currentColor = Color.Lerp(currentColor, startColor, Time.deltaTime / 0.03f);
            colorModel.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
            yield return new WaitForEndOfFrame();
        }
    }


}
