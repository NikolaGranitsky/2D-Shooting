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
    
    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    private bool m_wasCrouching = false;

    public BoolEvent OnCrouchEvent;

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
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        //CrouchCollider = GetComponent<CircleCollider2D>();
        //BoxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        jumpNumber = 0;
    }

    void Move(float MoveAxis, bool jump, bool crouch)
    {
        bool wasGrounded;
        wasGrounded = isGrounded;

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
                OnCrouchEvent.Invoke(true);
            }
            CrouchCollider.enabled = false;
            MoveAxis *= crouchSpeed;
        }
        else if(!crouch)
        {
            if(m_wasCrouching)
            {
                m_wasCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
            CrouchCollider.enabled = true;
            
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
        }
        else UpperBodyCollider.offset = new Vector2(0f, 0.1300541f);
        Vector2 targetVelocity = new Vector2(MoveAxis * movementSpeed, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity,MovementDamping);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (!wasGrounded)
        {
            if (Mathf.Abs(rb.velocity.y) > 0) animator.SetBool("Jumping", true);
            else animator.SetBool("Jumping", false);
        }
        else { animator.SetBool("Jumping", false); animator.SetBool("Falling", false); }
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

        if (Input.GetButtonDown("Shoot")) shootingController.Shoot();



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
}
