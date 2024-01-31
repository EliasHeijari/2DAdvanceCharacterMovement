using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{ 
    [BoxGroup("Movement")]
    [SerializeField] private float speed = 130f;
    [BoxGroup("Movement")]
    [SerializeField] private float jumpForce = 8f;

    [BoxGroup("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [BoxGroup("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.1f;
    [BoxGroup("Ground Check")]
    [Required("Ground Check Transform From Player Bottom")]
    [SerializeField] private Transform groundCheckTransform;
    private bool isGrounded;
    private bool isFacingRight;
    Rigidbody2D rigidBody;
    public Vector2 velocity { get; set; }

    // Wall Jump Movement
    [BoxGroup("Wall Jump")]
    [SerializeField] private Transform wallCheck;
    [BoxGroup("Wall Jump")]
    [SerializeField] private LayerMask wallJumpLayer;
    [BoxGroup("Wall Jump")]
    [SerializeField] private float wallJumpForce = 10f;
    [BoxGroup("Wall Jump")]
    [SerializeField] private float wallJumpGravity = -2f;
    [BoxGroup("Wall Jump")]
    [SerializeField] private float wallCheckRadius = 0.1f;
    private bool isOnWall = false;
   
    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Use In FixedUpdate, Get Inputs from Game Input script and uses rigidbody to move this gameobject
    /// </summary>
    public void HandleMovement(){
        float horizontal = GameInput.HorizontalInput();

        // Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (!isGrounded){
            velocity = new Vector2(velocity.x, rigidBody.velocity.y);
        }

        if (Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallJumpLayer) && !isGrounded){
            isOnWall = true;
            velocity = new Vector2(velocity.x, wallJumpGravity);
            if (Input.GetKey(KeyCode.S)) velocity = new Vector2(0, rigidBody.velocity.y);
            if (GameInput.JumpPressed())
            {
                if (isFacingRight) wallJumpForce = -wallJumpForce;
                else wallJumpForce = Mathf.Abs(wallJumpForce);
                velocity = new Vector2(wallJumpForce, Mathf.Abs(wallJumpForce));
            }
        }
        else{
            isOnWall = false;
        }

        if (!isOnWall && isGrounded)    
            // horizontal input to velocity
            velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, velocity.y);

        // Jump to velocity.y
        if (GameInput.JumpPressed() && isGrounded)
        {
            velocity = new Vector2(velocity.x, rigidBody.velocity.y + jumpForce);
        }

        

        // Update velocity, velocity depending on player inputs
        rigidBody.velocity = velocity;

        // Change Player Rotation, look left or right depending on the direction of movement (Velocity)
        switch(velocity.x){
            case > 0:
                // Player Looks Right
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
                isFacingRight = true;
                break;
            case < 0:
                // Player Looks Left
                transform.rotation = Quaternion.Euler(transform.rotation.x, -180f, transform.rotation.z);
                isFacingRight = false;
                break;
        }
    }
}
