
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    private bool bigMapActive;

    public bool isBossRoom;



    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // if player has entered the boss room
        if (isBossRoom)
        {
            // then set the view to display on the camera to the player's position
            target = PlayerController.instance.transform;
        }
    }


    void Update()
    {
        // if the target to display on the camera view exists
        if (target != null)
        {
            // then move the camera to the target position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        // if we have pressed the 'M' key to display the mini-map
        if(Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            // and we are not in the boss room
            if (!bigMapActive)
            {
                // then show the mini-map
                ActivateBigMap();
            } 
            
            // otherwise
            else
            {
                // hide the mini-map
                DeactivateBigMap();
            }
        }
    }


    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }


    public void ActivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {

            bigMapActive = true;

            bigMapCamera.enabled = true;

            mainCamera.enabled = false;

            PlayerController.instance.canMove = false;

            Time.timeScale = 0f;

            UIController.instance.mapDisplay.SetActive(false);

            UIController.instance.bigMapText.SetActive(true);
        }
    }


    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false;

            mainCamera.enabled = true;

            PlayerController.instance.canMove = true;

            Time.timeScale = 1f;

            UIController.instance.mapDisplay.SetActive(true);

            UIController.instance.bigMapText.SetActive(false);
        }
    }


} // end of class
