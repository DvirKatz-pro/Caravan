using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class Detailing Player Status
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    //UI elements
    [SerializeField] private Image hurtImage;
    [SerializeField] private Image healthBar;

    //player status vars
    [SerializeField] private float health = 100;
    private float currentHealth = 100;
    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / health;
        //StartCoroutine(DamageScreen());

    }
    /*
    /// <summary>
    /// make the screen flash red when player is hit
    /// </summary>
    private IEnumerator DamageScreen()
    {
        hurtImage.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        while (hurtImage.color != Color.clear)
        {
            hurtImage.color = Color.Lerp(hurtImage.color, Color.clear, Time.deltaTime * 8);
            yield return new WaitForEndOfFrame();
        }
    }
    */
}
