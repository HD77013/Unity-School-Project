using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private CharacterController controller;
    public GameObject player;
    private PlayerScript playerScript;
    public int enemySpeed = 3;
    public int enemyHealth = 3;
    [SerializeField]private bool canDamagePlayer = true;

    public bool canMove = true;

    private Rigidbody enemyRB;
    // Start is called before the first frame update
    void Start()
    {      
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        enemyRB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        enemyRB.AddForce(lookDirection * enemySpeed);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && canDamagePlayer){
            Debug.Log("Player is damaged");

            playerScript.playerHealth--;
            canDamagePlayer = false;
            StartCoroutine(damageCooldown());
        }
    }

    private IEnumerator damageCooldown()
    {
        yield return new WaitForSeconds(2);
        canDamagePlayer = true;
        StopCoroutine(damageCooldown());
    }


    public void Damage()
    {
        Debug.Log("I have been damaged");
        canMove = false;
        StartCoroutine(ableToMove(1));

        enemyHealth--;
    }
}
