using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] Animator animator;
    [SerializeField] private GameObject arrow;
    protected GameObject arrowInstance;
    [SerializeField] private Transform arrowPosition;
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            done = true;
            StartCoroutine(PreAttack());
        }
        


    }
    protected IEnumerator PreAttack()
    {
        Vector3 fireDirection = player.transform.position - transform.position;
        animator.SetTrigger("Draw");

        arrowInstance = Instantiate(arrow, arrowPosition);
        Vector3 arrowAngles = arrowInstance.transform.rotation.eulerAngles;
        arrowAngles.y = 180;
        arrowInstance.transform.Rotate(180, 0, 0);


        transform.forward = Quaternion.AngleAxis(45, Vector3.up) * fireDirection;
        yield return new WaitForSeconds(1 - 0.5f);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(OnAttack());
    }
    protected IEnumerator OnAttack()
    {
        animator.SetTrigger("Basic Attack");
        Vector3 force = player.transform.position - transform.position;
        arrowInstance.transform.SetParent(null, true);
        //loose arrow at player

        arrowInstance.GetComponent<Rigidbody>().AddForce(force * Time.deltaTime * 75, ForceMode.Impulse);
        // arrowInstance.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(1);
        done = false;
    }
    
}
