
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    public static BossController instance;

    // array of actions the boss should perform
    public BossAction[] actions;

    // which action the boss is currently performing
    private int currentAction;

    // how long the action lasts
    private float actionCounter;

    // number of shots fired
    private float shotCounter;

    // boss movement direction
    private Vector2 moveDirection;

    // boss rigidbody
    public Rigidbody2D bossRigidbody;

    // boss current health
    public int currentHealth;

    // special effect for boss death and hit damage
    public GameObject deathEffect, hitEffect;

    // holds the level exit object
    public GameObject levelExit;

    public BossSequence[] sequences;

    public int currentSequence;



    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        actions = sequences[currentSequence].actions;

        // set the action counter to the current action
        actionCounter = actions[currentAction].actionLength;

        UIController.instance.bossHealthBar.maxValue = currentHealth;

        UIController.instance.bossHealthBar.value = currentHealth;
    }


    // Update is called once per frame
    void Update()
    {
        // if there are actions to be performed
        if (actionCounter > 0)
        {
            // count down the action counter
            actionCounter -= Time.deltaTime;


            // stop moving the boss
            moveDirection = Vector2.zero;


            // if the current action of the boss is to move
            if (actions[currentAction].shouldMove)
            {
                // if the current action is to chase the player
                if (actions[currentAction].shouldChasePlayer)
                {
                    // get the direction to the player
                    moveDirection = PlayerController.instance.transform.position - transform.position;

                    // and normalise the movement
                    moveDirection.Normalize();
                }

                // if the current action is to move to a patrol point
                if (actions[currentAction].moveToPoint && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > .5f)
                {
                    // get the direction to the patrol point
                    moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;

                    // and normalise the movement
                    moveDirection.Normalize();
                }
            }

            // move the boss in a direction based on the current action
            bossRigidbody.linearVelocity = moveDirection * actions[currentAction].moveSpeed;


            // if the current action of the boss is to shoot at the player
            if (actions[currentAction].shouldShoot)
            {
                // count down the shot counter
                shotCounter -= Time.deltaTime;

                // if the shot counter becomes less then or equal to zero
                if (shotCounter <= 0)
                {
                    // set the shot counter to the time between shots of the current action
                    shotCounter = actions[currentAction].timeBetweenShots;

                    // loop through all of the shot positions of the current action
                    foreach(Transform t in actions[currentAction].shotPoints)
                    {
                        // and fire a bullet
                        Instantiate(actions[currentAction].itemToShoot, t.position, t.rotation);
                    }
                }
            }

        } 
        
        // otherwise
        else
        {
            // perform the next action
            currentAction++;

            // if the next action to be performed is greater than the number of actions
            if (currentAction >= actions.Length)
            {
                // set the current action to 0
                currentAction = 0;
            }

            // reset the action counter
            actionCounter = actions[currentAction].actionLength;
        }
    }


    // damage the boss
    public void TakeDamage(int damageAmount)
    {
        // subtract damage points current health
        currentHealth -= damageAmount;

        // if the current health becomes less than or equal to zero
        if (currentHealth <= 0)
        {
            // disable the boss
            gameObject.SetActive(false);

            // display a death effect at the boss position
            Instantiate(deathEffect, transform.position, transform.rotation);

            // check to see where the player is in the room
            // if the player is less than two units from the level exit position
            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {
                // move the level exit position four units to the right of the player
                // to avoid placing the exit on top of the player
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }

            // activate the exit level object
            levelExit.SetActive(true);

            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        }
        
        else
        {
            if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;

                actions = sequences[currentSequence].actions;

                currentAction = 0;

                actionCounter = actions[currentAction].actionLength;
            }
        }

        UIController.instance.bossHealthBar.value = currentHealth;
    }
}


[System.Serializable]
public class BossAction
{
    [Header("Action")]
    // how long the boss should perform an action before moving to the next action
    public float actionLength;

    // should the boss move
    public bool shouldMove;

    // should the boss chase the player
    public bool shouldChasePlayer;

    // how fast the boss can move
    public float moveSpeed;

    // should the boss move to a patrol point
    public bool moveToPoint;

    // boss patrol points
    public Transform pointToMoveTo;

    // should the boss shoot at player
    public bool shouldShoot;

    // what the boss will be shooting at the player
    public GameObject itemToShoot;

    // the time between each shot
    public float timeBetweenShots;

    // positions the boss will shoot from
    public Transform[] shotPoints;


}


[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;

    public int endSequenceHealth;
}
