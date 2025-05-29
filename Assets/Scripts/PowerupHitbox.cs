using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHitbox : MonoBehaviour
{
    public PlayerScript playerScript;

    public bool powerupCanDamage;

    [SerializeField] private int touchingEnemy = 0;
    private int _enemiesTouched;
    
    public LayerMask collisionLayerMask;

    public List<EnemyScript> enemies;
    // Start is called before the first frame update
    void Start()
    {
        powerupCanDamage = true;
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        _enemiesTouched = 0;

        StartCoroutine(DeleteTime());
    }

    IEnumerator DeleteTime()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.gameObject);
        powerupCanDamage = false;
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

    }
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy Collision"))
        {


            if (!enemies.Contains(collision.GetComponent<EnemyScript>()))
            {
                touchingEnemy++;
                enemies.Add(collision.GetComponent<EnemyScript>());


            }


            

            if (touchingEnemy > _enemiesTouched && powerupCanDamage)
            {


                foreach (EnemyScript script in enemies)
                {
                    script.PowerupDamage();
                }
                


                



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
            }



        }
    }
}

