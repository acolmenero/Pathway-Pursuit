using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region movement_variables
    public float movespeed;
    #endregion

    #region target_variables
    public Transform player;
    #endregion

    #region health_variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region unity_functions
    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();

        currHealth = maxHealth;
    }

    private void Update()
    {
        if (player == null) {
            return;
        }

        Move();
    }
    #endregion

    #region physics_component
    Rigidbody2D enemyRB;
    #endregion

    #region movement_functions
    private void Move()
    {
        Debug.Log("dsf");
            Vector2 direction = player.position - transform.position;
        enemyRB.velocity = direction.normalized * movespeed;
    }
    #endregion

    #region attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region attack_functions
    private void Explode() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach(RaycastHit2D hit in hits) { 
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Hit the player w/ explosion");

                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);

                Instantiate(explosionObj, transform.position, transform.rotation);
                Destroy(this.gameObject);
             }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.transform.CompareTag("Player")) {
            Explode();
         }
    }
    #endregion

    #region health_functions
    public void TakeDamage(float value) {
        currHealth -= value;
        if (currHealth <= 0) {

            Die(); } }

    private void Die() {
        Destroy(this.gameObject);
        }
    #endregion
}
