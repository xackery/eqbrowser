/* ThirdPersonController - C# rewrite of the JS version from Unity
 * Created - March 24 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class EQBThirdPersonController : MonoBehaviour
{
    public EQBrowser.Avatar charAvatar;

    //The speed when walking
    public float walkSpeed = 2.0f; 

    //Run speed
    public float runSpeed = 6.0f;

    //How high the character can jump
    public float jumpHeight = 0.5f;

    //Gravity of the character
    public float gravity = 19.6f;

    public float speedSmoothing = 10.0f;
    public float rotateSpeed = 500.0f;

    public bool canJump = true;

    public float jumpRepeatTime = 0.05f;
    public float jumpTimeout = 0.15f;
    public float groundedTimeOut = 0.25f;
    public float inAirControlAcceleration = 2.0f;

    //The camera doesn't start following the target immediately but waits for a split second to avoid too much waving around.
    public float lockCameraTimer = 0.0f;

    //The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    //The current vertical speed
    private float verticalSpeed = 0.0f;
    //The current x-z move speed
    private float moveSpeed = 0.0f;

    //The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    //Are we jumping? (Initiated with jump button and not grounded yet)
    private bool jumping = false;
    private bool jumpingReachedApex = false;

    //Are we moving backwards (this locks the camera to not do a 180 degree spin)
    private bool movingBack = false;
    //Is the user pressing any keys?
    private bool isMoving = false;
    //Last time the jump button was clicked down
    private float lastJumpButtonTime = -10.0f;
    //Last time we performed a jump
    private float lastJumpTime = -1.0f;

    //The height we jumped from (used to determine how long to apply extra jump power after jumping.)
    private float lastJumpStartHeight = 0.0f;

    private Vector3 inAirVelocity = Vector3.zero;

    private float lastGroundedTime = 0.0f;

    private bool isControllable = true;

    private CharacterController controller;

    void Awake()
    {
        moveDirection = transform.TransformDirection(Vector3.forward);

        controller = this.GetComponent<CharacterController>();

        charAvatar = this.gameObject.GetComponent<EQBrowser.Avatar>();

        charAvatar.SetCurGroundHeight(this.gameObject.transform.position.y);

        //TODO: Animation
    }

    void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;
        bool grounded = IsGrounded();

        //Forward vector relative to the camera along the x-z plane
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        //Right vector relative to the camera
        //Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //Are we moving backwards or looking backwards?
        if (v < -0.2f)
            movingBack = true;
        else
            movingBack = false;

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        //Target direction relative to the camera
        Vector3 targetDirection = h * right + v * forward;

        if (Input.GetKey(KeyCode.Q))
        {
            controller.gameObject.transform.Rotate(Vector3.up, -45f * Time.deltaTime);
            charAvatar.CharTurning(false);
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            controller.gameObject.transform.Rotate(Vector3.up, 45f * Time.deltaTime);
            charAvatar.CharTurning(true);
        }

        //Grounded controls
        if (grounded)
        {
            //Lock camera for short period when transitioning moving & standing still
            lockCameraTimer += Time.deltaTime;

            if (isMoving != wasMoving)
            {
                lockCameraTimer = 0.0f;
            }

            //We store speed and direction seperately,
            //so that when the character stands still we still have a valid forward direction
            //moveDirection is always normalized, and we only update it if there is user input.
            if (targetDirection != Vector3.zero)
            {
                //If we are really slow, just snap to the target direction
                if (moveSpeed < walkSpeed * 0.9f && grounded)
                {
                    moveDirection = targetDirection.normalized;
                }
                //Otherwise smoothly turn towards it
                else
                {
                    moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000f);

                    moveDirection = moveDirection.normalized;
                }

                if (grounded)
                {
                    charAvatar.SetCurGroundHeight(this.gameObject.transform.position.y);
                }
            }

            //Smooth the speed based on the current target direction
            float curSmooth = speedSmoothing * Time.deltaTime;

            //Choose target speed
            //We want to support analog input but make sure you can't walk faster diagonally than just forward or sideways
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            //Pick speed modifier
            if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
            {
                targetSpeed *= runSpeed;
            }
            else
            {
                targetSpeed *= walkSpeed;
            }

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
        }
        //In air controls
        else
        {
            //Lock camera while in air
            if (jumping)
            {
                lockCameraTimer = 0.0f;
            }

            if (isMoving)
            {
                inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
            }
        }
    }

    void ApplyJumping()
    {
        //Prevent jumping too fast after each other
        if (lastJumpTime + jumpRepeatTime > Time.time)
        {
            return;
        }

        if (IsGrounded())
        {
            //Jump
            //Only when pressing the button down
            //With a timeout so you can press the button slightly before landing
            if (canJump && Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
                charAvatar.Jump();
            }
        }
    }

    void ApplyGravity()
    {
        if (isControllable) //don't move player at all if not controllable
        {
            //Apply gravity
            bool jumpButton = Input.GetKeyDown(KeyCode.Space);
            
            //When we reach the apex of the jump we send out a message
            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
            {
                jumpingReachedApex = true;
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            if (IsGrounded())
            {
                verticalSpeed = 0.0f;
                charAvatar.SetCurGroundHeight(this.gameObject.transform.position.y);
            }
            else
            {
                verticalSpeed -= gravity * Time.deltaTime;
            }
        }
    }

    float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        //From the jump height and gravity we deduce the upwards speed
        //for the character to reach at the apex.
        return Mathf.Sqrt(2f * targetJumpHeight * gravity);
    }

    void DidJump()
    {
        jumping = true;
        jumpingReachedApex = false;
        lastJumpTime = Time.time;
        lastJumpStartHeight = transform.position.y;
        lastJumpButtonTime = -10f;
    }

    void Update()
    {
        if (!isControllable)
        {
            //kill all inputs if not controllable.
            Input.ResetInputAxes();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastJumpButtonTime = Time.time;
        }

        UpdateSmoothedMovementDirection();

        //Apply gravity
        ApplyGravity();

        //Apply jumping logic
        ApplyJumping();

        //Calculate actual motion
        Vector3 movement = moveDirection * moveSpeed + new Vector3(0f, verticalSpeed, 0f) + inAirVelocity;
        movement *= Time.deltaTime;

        //Move the controller
        collisionFlags = controller.Move(movement);

        //Animation sector

        //End animation sector

        //Set rotation to move direction
        if (IsGrounded())
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            Vector3 xzMove = movement;
            xzMove.y = 0;
            if (xzMove.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(xzMove);
            }
        }

        //We are in jump mode but just became grounded
        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
            inAirVelocity = Vector3.zero;
            if (jumping)
            {
                jumping = false;
                SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.moveDirection.y > 0.01f)
        {
            return;
        }
    }

    float GetSpeed()
    {
        return moveSpeed;
    }

    bool IsJumping()
    {
        return jumping;
    }

    bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    Vector3 GetDirection()
    {
        return moveDirection;
    }

    bool IsMovingBackwards()
    {
        return movingBack;
    }

    float GetLockCameraTimer()
    {
        return lockCameraTimer;
    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    bool HasJumpReachedApex()
    {
        return jumpingReachedApex;
    }

    bool IsGroundedWithTimeout()
    {
        return lastGroundedTime + groundedTimeOut > Time.time;
    }

    void Reset()
    {
        gameObject.tag = "Player";
    }
}