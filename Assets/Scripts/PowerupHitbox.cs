using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHitbox : MonoBehaviour
{
    public PlayerScript playerScript;

    private bool powerupCanDamage;

    [SerializeField] private int touchingEnemy = 0;
    private int enemiesTouched;

    public List<EnemyScript> enemies;
    public List<Rigidbody> enemiesRB;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        enemiesTouched = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyScript script in enemies.ToArray())
        {
            if (script == null)
            {
                enemies.Remove(script);
            }
        }

        foreach (Rigidbody rb in enemiesRB.ToArray())
        {
            if (rb == null)
            {
                enemiesRB.Remove(rb);
            }
        }

    }
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Enemy"))
        {


            if (!enemies.Contains(collision.GetComponent<EnemyScript>()))
            {
                touchingEnemy++;
                enemies.Add(collision.GetComponent<EnemyScript>());
                enemiesRB.Add(collision.GetComponent<Rigidbody>());


            }



            Vector3 plrCoordinates = (collision.transform.position - transform.position).normalized;

            if (touchingEnemy > enemiesTouched && powerupCanDamage)
            {


                foreach (EnemyScript script in enemies)
                {
                    script.Damage();
                }


                foreach (Rigidbody rb in enemiesRB)
                {
                    rb.AddForce(plrCoordinates * 10, ForceMode.Impulse);
                }


                playerScript.StopCoroutine(playerScript.stopCanDamage());

                powerupCanDamage = false;

            }



        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            if (enemies.Contains(other.GetComponent<EnemyScript>()))
            {
                touchingEnemy--;
                enemies.Remove(other.GetComponent<EnemyScript>());
                enemiesRB.Remove(other.GetComponent<Rigidbody>());
            }



        }
    }
}

