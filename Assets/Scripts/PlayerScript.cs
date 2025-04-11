using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;

    private Vector3 _playerVelocity;

    public float speed = 2;

    public float turnSmoothTime = 0.1f;
    public float offset = 2f;

    public Transform cam;

    public Vector3 moveDir;

    public Animator animator;

    public GameObject sword;
    public bool canHit = true;
    private SwordScript _swordScript;

    public float dashTime;
    public float dashSpeed;

    public float plrOffset;
  
    private bool _hasPowerup = false;
    bool _ActivatePowerup = false;

    public int playerHealth = 5;

    public GameObject powerupPrefab;
    public PowerupHitbox powerupHitbox;

    private void Start()
    {
        _swordScript = GetComponentInChildren<SwordScript>();
        controller = gameObject.GetComponent<CharacterController>();
        powerupHitbox = GameObject.Find("PowerupHB").GetComponent<PowerupHitbox>();
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

            StartCoroutine(Cooldown());
        }

        if (Input.GetMouseButton(1) && _hasPowerup) {
            animator.SetBool("ActivatePowerup", true);
        }
        
        if (Input.GetMouseButtonUp(1) && _hasPowerup)
        {
            animator.SetBool("ActivatePowerup", false);
            animator.SetTrigger("RemovePowerup");
 
            _hasPowerup = false;

        }

    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canHit = true;
        StopCoroutine(Cooldown());
    }

    public IEnumerator StopCanDamage()
    {
        yield return new WaitForSeconds(0.5f);
        _swordScript.canDamage = false;
        StopCoroutine(StopCanDamage());
    }



    float AngleBetweenPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.x - b.x, a.z - b.z) * Mathf.Rad2Deg;
    }

    void Attack()
    {
        _swordScript.canDamage = true;
        StartCoroutine(StopCanDamage());      
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Powerup"))
        {
            _hasPowerup = true;
            Destroy(other.gameObject);
        }
    }

    public void StartDash()
    {
        Instantiate(powerupPrefab, transform.position, Quaternion.LookRotation(transform.right));

        StartCoroutine(Dash());
        Debug.Log("Player has dashed");
    }

    IEnumerator Dash()
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
