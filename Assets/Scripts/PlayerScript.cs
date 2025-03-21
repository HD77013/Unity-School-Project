using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerScript : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float speed = 10.0f;
    public Camera cam;
    public Animator animator;
    public float offset = 5f;
    public GameObject sword;
    public bool canHit = true;
    private SwordScript swordScript;

    private void Start()
    {
        swordScript = GetComponentInChildren<SwordScript>();
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

        //Angle between mouse and this object
        float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);

        //Ta daa
        transform.rotation = Quaternion.Euler(new Vector3(0f, angle + offset, 0f));

        


        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetMouseButton(0) && canHit)
        {
            Attack();
            canHit = false;

            StartCoroutine(cooldown());
        }
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(1f);
        canHit = true;
        StopCoroutine(cooldown());
    }

    public IEnumerator stopCanDamage()
    {
        yield return new WaitForSeconds(0.5f);
        swordScript.canDamage = false;
        StopCoroutine(stopCanDamage());
    }

    float AngleBetweenPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.x - b.x, a.z - b.z) * Mathf.Rad2Deg;
    }

    void Attack()
    {
        animator.SetTrigger("PlayerAttack");

        swordScript.canDamage = true;
        StartCoroutine(stopCanDamage());      
    }

}
