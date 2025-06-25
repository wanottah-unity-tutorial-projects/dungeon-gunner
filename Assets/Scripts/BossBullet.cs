
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossBullet : MonoBehaviour
{
    // speed of bullet
    public float speed;

    // direction of bullet
    private Vector3 direction;



    void Start()
    {
        //direction = PlayerController.instance.transform.position - transform.position;
        //direction.Normalize();

        // set direction of bullet
        direction = transform.right;
    }


    void Update()
    {
        // move bullet
        transform.position += direction * speed * Time.deltaTime;

        // if the boss has been defeated
        if (!BossController.instance.gameObject.activeInHierarchy)
        {
            // destroy the bullet
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }

        Destroy(gameObject);

        AudioManager.instance.PlaySFX(4);
    }


    // when the bullet goes out of view of the camera
    private void OnBecameInvisible()
    {
        // destroy the bullet
        Destroy(gameObject);
    }


} // end of script
