using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement_variables
    private float x_input;
    private float y_input;
    private Vector2 currDirection;
    public float movespeed;
    #endregion

    #region attack_variables
    public float attackSpeed;
    public float hitBoxTiming;
    public float endAnimatingTiming;
    private float attackTimer;
    private bool isAttacking;
    public float damage;
    #endregion

    #region health_variables
    public float maxHealth;
    private float currHealth;
    public Slider hpSlider;
    #endregion

    #region physics_components
    Rigidbody2D playerRB;
    #endregion

    #region ac
    Animator anim;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        attackTimer = 0;
        isAttacking = false;

        currHealth = maxHealth;
        hpSlider.value = currHealth / maxHealth;
    }

    private void Update()
    {
        if (isAttacking) {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
        Move();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0) {
            isAttacking = true;
            Attack();
        } else {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            Interact(); }
    }
    #endregion

    #region movement_functions
    private void Move()
    {
        anim.SetBool("Moving", true);
        if (x_input > 0) {
            playerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
        }
        else if (x_input < 0)
        {
            playerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
        else if (y_input > 0)
        {
            playerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        }
        else if (y_input< 0)
        {
            playerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else {
            playerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
    }
    #endregion

    #region attack_functions
    private void Attack() {
        Debug.Log("I'm attacking");
        attackTimer = attackSpeed;
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine() {
        playerRB.velocity = Vector2.zero;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(hitBoxTiming);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, Vector2.one, 0f, Vector2.zero, 0);
        foreach(RaycastHit2D hit in hits) { 
            if (hit.transform.CompareTag("Enemy")) {
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
                Debug.Log("I hit an enemy");
            }
        }
        yield return new WaitForSeconds(endAnimatingTiming);
        isAttacking = false;
    }

    #endregion

    #region health_functions
    public void TakeDamage(float value) {
        currHealth -= value;
        hpSlider.value = currHealth / maxHealth;
        Debug.Log("Health is now " + currHealth.ToString());

        if(currHealth <= 0) {
            Die();
         }

    }

    private void Die() {
        Destroy(this.gameObject);
     }

    public void Heal(float value) {
        currHealth += value;
        hpSlider.value = currHealth / maxHealth;
        currHealth = Mathf.Min(currHealth, maxHealth);
        Debug.Log("Health is now " + currHealth.ToString());
    }

    private void Interact() {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, new Vector2(.5f, .5f), 0f, Vector2.zero, 0);
    foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Chest")) {
                hit.transform.GetComponent<Chest>().Interact(); 
                } 
                } }

    #endregion
}

