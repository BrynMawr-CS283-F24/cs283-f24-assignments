using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMotionController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    public float movementSpeed = 100.0f;
    public float turningSpeed = 150.0f;

    public float gravity = -9.81f;
    private Vector3 velocity;
    private float gravityMultiplier = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        float forwarMovement = 0f;

        // Check for W key (move forward)
        if (Input.GetKey(KeyCode.W))
        {
            forwarMovement = movementSpeed;
            animator.SetFloat("speed", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwarMovement  = -movementSpeed;
            animator.SetFloat("speed", 1);

        }
        else
        {
            animator.SetFloat("speed", 0);  // Idle animation
        }

        // Left and Right Turning
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -turningSpeed * Time.deltaTime);  // Turn left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, turningSpeed * Time.deltaTime);   // Turn right
        }

        //forward movement
        Vector3 moveDirection = transform.forward * forwarMovement;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void ApplyGravity()
    {
        // Apply gravity to the y-component of the velocity
        if (characterController.isGrounded)
        {
            velocity.y = 0f; // Reset velocity when grounded
        }
        else
        {
            velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
    }
}
