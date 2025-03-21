using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private CharacterController controller;
    public GameObject player;
    public int enemySpeed = 1;
    public int enemyHealth = 3;
    public Vector3 plrCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        plrCoordinates = (transform.position - player.transform.position).normalized;

        controller.Move(lookDirection * Time.deltaTime * enemySpeed);

        if (enemyHealth < 1)
        {
           gameObject.SetActive(false);
        }
    }

    public void Damage()
    {
        Debug.Log("I have been damaged");

        enemyHealth--;
    }
}
