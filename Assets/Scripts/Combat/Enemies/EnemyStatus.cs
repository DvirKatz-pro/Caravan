using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private float health;
    EnemyActions actions;

    bool isDead = false;

    [SerializeField] protected GameObject colorModel;
    [SerializeField] protected float colorTimer;
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
    public void takeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            actions.onDeath();
        }
        else
        {
            actions.stunned();
        }
        StartCoroutine(takeDamageTimer());

    }
    public virtual IEnumerator takeDamageTimer()
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
