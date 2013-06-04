/* LocalPlayerController - Based on EQBThirdPersonController - handles local player movement
 * Created - April 14 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EQBrowser;

public class LocalPlayerController : MonoBehaviour
{
    CharacterColliderController m_colliderControl;
    EQBrowser.Avatar m_avatar;
    CharacterController m_charControl;

    public float m_walkSpeed;
    public float m_runSpeed;
    public float m_gravity;
    public bool isWalking = false;
    public bool m_isHover;

    public float m_minTimeBetweenJumps = 0.3f;

    protected bool m_isJumping;
    protected float m_jumpTimer;
    
    protected CollisionFlags m_collisionFlags;

    bool m_keyJump;
    
    void Awake()
    {
        m_colliderControl = this.gameObject.GetComponent<CharacterColliderController>();
        m_avatar = this.gameObject.GetComponent<EQBrowser.Avatar>();
        m_charControl = this.gameObject.GetComponent<CharacterController>();

        m_avatar.SetCurGroundHeight(this.gameObject.transform.position.y);

        if (m_colliderControl == null)
        {
            Debug.LogError(string.Format("Cannot find Character Collider Controller for {0}", this.gameObject.name));
        }

        if (m_avatar == null)
        {
            Debug.LogError(string.Format("Cannot find Avatar for {0}", this.gameObject.name));
        }
    }

    //void Update()
    //{
    //    if (!m_keyJump)
    //    {
    //        m_keyJump = Input.GetKeyDown(KeyCode.Space);
    //    }
    //}

    //void FixedUpdate()
    void Update()
    {
        if (!m_keyJump)
        {
            m_keyJump = Input.GetKeyDown(KeyCode.Space);
        }

        Vector3 inputMoveDir = Vector3.zero;

        if ((Input.GetMouseButton(0) && Input.GetMouseButton(1)) || //zer0sum: move with mousebuttons
                Input.GetKey(KeyCode.W))
        {
            inputMoveDir += this.gameObject.transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir += -this.gameObject.transform.forward;
        }

        if (Input.GetKey(KeyCode.E))
        {
            inputMoveDir += (this.gameObject.transform.right * 0.75f);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            inputMoveDir += (-this.gameObject.transform.right * 0.75f);
        }

        Vector3 rotation = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            //rotation.y -= 55f * Time.deltaTime;
            //m_avatar.CharTurning(false);

            Turn(-55f, false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //rotation.y += 55f * Time.deltaTime;
            //m_avatar.CharTurning(true);
            Turn(55f, true);
        }

        if (m_avatar.m_isDead || m_avatar.m_isSitting)
        {
            inputMoveDir = Vector3.zero;
            rotation = Vector3.zero;
        }

        Vector3 verticalMoveDir = ApplyGravityAndJump();

        inputMoveDir = inputMoveDir.normalized * m_walkSpeed * Time.deltaTime;
        m_collisionFlags = m_charControl.Move(inputMoveDir + verticalMoveDir);
    }

    public void Turn(float amount, bool clockwise)
    {
        Vector3 rotation = Vector3.zero;

        rotation.y += (amount * Time.deltaTime);
        m_avatar.CharTurning(clockwise);

        this.gameObject.transform.Rotate(rotation, Space.World);
    }

    Vector3 ApplyGravityAndJump()
    {
        Vector3 verticalDir = Vector3.zero;

        if (m_keyJump && !m_isJumping && m_jumpTimer < Time.time)
        {
            //Jump height is 35% of total height
            verticalDir.y += CalculateJumpVerticalSpeed(m_colliderControl.MaxHeight * 0.35f);
            m_isJumping = true;
            m_keyJump = false;
            m_avatar.Jump();
        }

        bool isGrounded = IsGrounded();
        if (isGrounded)
        {
            //Are we grounded and were we jumping? Then we must've just landed
            if (m_isJumping)
            {
                m_jumpTimer = Time.time + m_minTimeBetweenJumps;
            }

            m_avatar.SetCurGroundHeight(this.gameObject.transform.position.y);

            //We're on the ground, so we're not jumping
            m_isJumping = false;            
        }
        else
        {
            m_keyJump = false;
            verticalDir.y -= (m_gravity * Time.deltaTime);
        }

        return verticalDir;
    }

    bool IsGrounded()
    {
        return (m_collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        //From the jump height and gravity we deduce the upwards speed
        //for the character to reach at the apex.
        return Mathf.Sqrt(1f * targetJumpHeight * m_gravity);
    }
}