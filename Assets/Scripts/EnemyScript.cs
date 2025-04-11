using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private CharacterController _controller;
    public GameObject player;
    private PlayerScript _playerScript;
    public int enemySpeed = 3;
    public int enemyHealth = 3;
    [SerializeField]private bool canDamagePlayer = true;

    public bool canMove = true;

    private Rigidbody _enemyRb;
    // Start is called before the first frame update
    void Start()
    {      
        player = GameObject.Find("Player");
        _playerScript = player.GetComponent<PlayerScript>();
        _enemyRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        if (canMove)
        {
            _enemyRb.AddForce(lookDirection * enemySpeed);
        }


        if (enemyHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AbleToMove(int cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canMove = true;
        canDamagePlayer = false;
    }

    private IEnumerator PowerupEffect()
    {
        canDamagePlayer = false;
        canMove = false;
        yield return new WaitForSeconds(3f);
        enemyHealth -= 3;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && canDamagePlayer){
            Debug.Log("Player is damaged");

            _playerScript.playerHealth--;
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
        canDamagePlayer = false;
        canMove = false;
        StartCoroutine(AbleToMove(1));

        enemyHealth--;
    }
    
    public void PowerupDamage()
    {
        Debug.Log("I have been damaged by powerup");

        StartCoroutine(PowerupEffect());


    }
}
