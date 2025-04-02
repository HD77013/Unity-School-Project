using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;

    private Vector3 playerVelocity;

    public float speed = 2;

    public float turnSmoothTime = 0.1f;
    public float offset = 2f;

    public Transform cam;

    public Vector3 moveDir;

    public Animator animator;

    public GameObject sword;
    public bool canHit = true;
    private SwordScript swordScript;

    public float dashTime;
    public float dashSpeed;

    public float plrOffset;
  
    private bool hasPowerup = false;
    bool ActivatePowerup = false;

    public int playerHealth = 5;

    private void Start()
    {
        swordScript = GetComponentInChildren<SwordScript>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }


        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
        

        transform.rotation = Quaternion.Euler(new Vector3(0f, angle + offset, 0f));



        if (Input.GetMouseButton(0) && canHit)
        {
            animator.SetTrigger("PlayerAttack");
            canHit = false;

            StartCoroutine(cooldown());
        }

        if (Input.GetMouseButton(1) && hasPowerup)
        {

            animator.SetBool("ActivatePowerup", true);


            
        }
        if (Input.GetMouseButtonUp(1) && hasPowerup)
        {

            animator.SetBool("ActivatePowerup", false);
            animator.SetTrigger("RemovePowerup");

            //hasPowerup = false;

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
        }
    }

    public void startDash()
    {
        StartCoroutine(dash());
        Debug.Log("Player has dashed");
    }

    IEnumerator dash()
    {
        float startTime = Time.time;
        Vector3 plrRot = -transform.right;

        while (Time.time < startTime + dashTime)
        {
            controller.Move(plrRot * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
