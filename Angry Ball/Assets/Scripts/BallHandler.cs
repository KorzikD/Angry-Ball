using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;


    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentSpringJoint;

    

    private Camera camera;
    private bool isDragging;
    void Start()
    {
        camera = Camera.main;
        BallSpawn();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null) return;

        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            
            return;
        }

        isDragging = true;

        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition =  camera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;


    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), delay);
        
        
    }

    private void DetachBall()
    {
        currentSpringJoint.enabled = false;
        currentSpringJoint = null;

        Invoke(nameof(BallSpawn), respawnDelay);
    }

    private void BallSpawn()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position,Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentSpringJoint.connectedBody = pivot;
        Destroy(ballInstance, 12f);
    }
}
