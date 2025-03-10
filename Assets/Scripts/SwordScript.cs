using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public PlayerScript playerScript;

    [SerializeField] private int touchingEnemy = 0;
    public bool canDamage;
    private int enemiesTouched;

    public List<EnemyScript> enemies;

    // Start is called before the first frame update
    void Start()
    {

        playerScript = gameObject.GetComponentInParent<PlayerScript>();
        enemiesTouched = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Enemy"))
        {

            
            if (!enemies.Contains(collision.GetComponent<EnemyScript>()))
            {
                touchingEnemy++;
                enemies.Add(collision.GetComponent<EnemyScript>());
            }
            



            if (touchingEnemy > enemiesTouched && canDamage)
            {


                foreach (EnemyScript script in enemies) {
                    script.Damage();
                }

                playerScript.StopCoroutine(playerScript.stopCanDamage());

                canDamage = false;

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
            }

        }
    }
}
