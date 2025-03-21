using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private CharacterController controller;
    public GameObject player;
    public int enemySpeed = 3;
    public int enemyHealth = 3;

    public bool canMove = true;

    private Rigidbody enemyRB;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        if (canMove)
        {
            enemyRB.AddForce(lookDirection * enemySpeed);
        }


        if (enemyHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ableToMove(int cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canMove = true;
    }

    public void Damage()
    {
        Debug.Log("I have been damaged");
        canMove = false;
        StartCoroutine(ableToMove(1));

        enemyHealth--;
    }
}
