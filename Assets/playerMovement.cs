using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class playerMovement : MonoBehaviour
{
    [SerializeField]private Collider2D CrouchCollider;
    [SerializeField]private Collider2D UpperBodyCollider;
    [Range(1f, 100f)] [SerializeField] private float movementSpeed = 10;
    [Range(0f, 1f)] [SerializeField] private float MovementDamping;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int jumpCount = 1;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform groundCheck;
    
    
    
    private bool m_wasCrouching = false;

    

    private Animator animator;

    private Vector2 m_Velocity = Vector3.zero;

    public Shooting shootingController;
    
    private Rigidbody2D rb;
    private bool crouching;
    private bool isGrounded;
    private bool jump;
    private bool facingRight;
    int jumpNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        shootingController = GetComponent<Shooting>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

        //CrouchCollider = GetComponent<CircleCollider2D>();
        //BoxCollider = GetComponent<BoxCollider2D>();
        jumpNumber = 0;
    }

    void Move(float MoveAxis, bool jump, bool crouch)
    {
        bool wasGrounded;
        wasGrounded = isGrounded;
        if (MoveAxis > 0 && facingRight)
        {
            Flip();
        }
        else if (MoveAxis < 0 && !facingRight)
        {
            Flip();
        }

        if (!crouching)
        {
            if (Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, Ground) != null)
            {
                crouch = true;
            }
        }
        

        if(crouch)
        {
            if(!m_wasCrouching)
            {
                m_wasCrouching = true;

            }
            CrouchCollider.enabled = false;
            MoveAxis *= crouchSpeed;
        }
        else if(!crouch)
        {
            if(m_wasCrouching)
            {
                m_wasCrouching = false;
            }
            CrouchCollider.enabled = true;
            shootingController.FirePoint.localPosition -= new Vector3(0f, -0.7f, 0f);

        }

        if(jump && jumpNumber < jumpCount)
        {
            rb.AddForce(new Vector2(0f,jumpForce),ForceMode2D.Impulse);
            isGrounded = false;
            jumpNumber++;
            Debug.Log(jumpNumber);
        }

        if (MoveAxis > 0 && facingRight)
        {
            Flip();
        }
        else if(MoveAxis < 0 && !facingRight)
        {
            Flip();
        }

        if (!CrouchCollider.enabled)
        {
            UpperBodyCollider.offset = new Vector2(0f, -0.61f);
            shootingController.FirePoint.localPosition = new Vector3(1, -0.62f, 0f);
        }
        else
        {
            UpperBodyCollider.offset = new Vector2(0f, 0.1300541f);
            shootingController.FirePoint.localPosition = new Vector3(0.881f, 0.168f, 0f);
        }

        


        Vector2 targetVelocity = new Vector2(MoveAxis * movementSpeed, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity,MovementDamping);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (!wasGrounded)
        {
            if (Mathf.Abs(rb.velocity.y) > 0) animator.SetBool("Jumping", true);
            else animator.SetBool("Jumping", false);
        }
        else  animator.SetBool("Jumping", false);
        animator.SetBool("Crouching", !CrouchCollider.enabled);
        wasGrounded = false;
        
    }

    private void Update()
    {

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jumping", true);
        }
        
        if(Input.GetButtonDown("Crouch"))
        {
            crouching = true;
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            crouching = false;
        }

        if (Input.GetButtonDown("Shoot"))
        {
            animator.SetBool("Shooting", true);
            shootingController.Shoot();
            Invoke("Wait",0.5f);
        }
        



    }

    void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(groundCheck.position, 0.06f, Ground))
        {
            isGrounded = true;
            jumpNumber = 0;
        }

        Move(Input.GetAxisRaw("Horizontal"),jump,crouching);

        jump = false;

    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
 public void Wait()
    {
        animator.SetBool("Shooting", false);
    }
}
