using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public PlayerScript playerScript;

    [SerializeField] private int touchingEnemy = 0;
    public bool canDamage;
    private int _enemiesTouched;

    public LayerMask collisionLayerMask;

    public List<EnemyScript> enemies;
    public List<Rigidbody> enemiesRb;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = gameObject.GetComponentInParent<PlayerScript>();
        _enemiesTouched = 0;
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

        foreach (Rigidbody rb in enemiesRb.ToArray())
        {
            if (rb == null)
            {
                enemiesRb.Remove(rb);
            }
        }

    }
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy Collision"))
        {

            
            if (!enemies.Contains(collision.GetComponent<EnemyScript>()))
            {
                touchingEnemy++;
                enemies.Add(collision.GetComponent<EnemyScript>());
                enemiesRb.Add(collision.GetComponent<Rigidbody>());


            }



            Vector3 plrCoordinates = (collision.transform.position - transform.position).normalized;

            if (touchingEnemy > _enemiesTouched && canDamage)
            {


                foreach (EnemyScript script in enemies) {
                    script.Damage();
                }


                foreach (Rigidbody rb in enemiesRb)
                {
                    rb.AddForce(plrCoordinates * 10, ForceMode.Impulse);
                }


                playerScript.StopCoroutine(playerScript.StopCanDamage());

                canDamage = false;

            }



        }


        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy Collision"))
        {

            if (enemies.Contains(other.GetComponent<EnemyScript>()))
            {
                touchingEnemy--;
                enemies.Remove(other.GetComponent<EnemyScript>());
                enemiesRb.Remove(other.GetComponent<Rigidbody>());
            }



        }
    }
}
