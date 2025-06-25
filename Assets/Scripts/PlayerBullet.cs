
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;

    public int damageToGive = 50;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.linearVelocity = transform.right * speed;
    }


    // if the player bullet hits a solid object
    private void OnTriggerEnter2D(Collider2D other)
    {     
        // show an impact effect where the bullet hits
        Instantiate(impactEffect, transform.position, transform.rotation);

        // and destroy the bullet
        Destroy(gameObject);

        // play a sound when the player bullet hits an object
        AudioManager.instance.PlaySFX(4);

        // if the player bullet hits the enemy
        if (other.tag == "Enemy")
        {
            // damage the enemy
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }

        // if the player bullet hits the boss
        if(other.tag == "Boss")
        {
            // damage the boss
            BossController.instance.TakeDamage(damageToGive);

            // show an impact effect on the boss
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
