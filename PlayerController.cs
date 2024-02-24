
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;


public class PlayerControler : MonoBehaviour
{


    [Header("Horizontal Movement Settings")]

    private Rigidbody2D rb;
    private Animator anim;
    private float xAxis;
    private float gravity;
    private bool canDash;
    private bool Dashed;
    [SerializeField] private float Walkspeed = 1;// default setting walkspeed=1


    /// ////////////////////////////////////////////////////////////
  

    [Header(" Settings Air Jump and Buffer Jump")]

    private int JumpBufferCounter;

    [SerializeField] int JumpBufferFrame;

    [SerializeField] private int AirJumpMax;// khai bao so buoc nhay tren khong toi da co the thuc hien

    private int airJumpcounter = 0; // dem so jump tren khong

    [SerializeField] private float JumpForceZ = 45;


    
    /////////////////////////////////////////////////////////////////
  

    [Header("Ground Checking")]

    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float groundcheckY = 0.2f;
    [SerializeField] private float groundcheckX = 0.5f;
    [SerializeField] private LayerMask Whatground;

    PlayerSecondary playerState;


    /// //////////////////////////////////////////////////////////////


    [Header("Dashing Settings")]

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;







    public static PlayerControler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        playerState = GetComponent<PlayerSecondary>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;


    }
    void Update()
    {
        GetInputs();
        UpdateJump();
        if(playerState.dashing)
        {
            return;
        }
        flip();
        Move();
        Jump();
        StartDash();

    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        //lib co san
    }


    void flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y); // Set X-axis to -1 for left-facing
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y); // Set X-axis to 1 for right-facing
        }
    }


    private void Move()
    {
        rb.velocity = new Vector2(Walkspeed * xAxis, rb.velocity.y);
        flip();
        //lenh move left right
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
        //tao condictional
    }


    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !Dashed) 
        {
            StartCoroutine(Dash());
            Dashed = true;
        }
        if (Grounded())
        {
            Dashed = false;
        }
    }
    IEnumerator Dash()
    {
        canDash = false;

        playerState.dashing = true;

        anim.SetTrigger("Dashing");

        rb.gravityScale = 0;

        rb.velocity = new Vector2(transform.localScale.x * dashSpeed , 0 );

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = gravity;

        playerState.dashing =false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }



    public bool Grounded()
    {
        if (Physics2D.Raycast(GroundCheck.position, Vector2.down, groundcheckY, Whatground)
            || Physics2D.Raycast(GroundCheck.position + new Vector3(groundcheckX, 0, 0), Vector2.down, groundcheckY, Whatground)
            || Physics2D.Raycast(GroundCheck.position + new Vector3(-groundcheckX, 0, 0), Vector2.down, groundcheckY, Whatground))
        {
            return true;
        }
        else
        {
            return false;
        }

        // funncion ktra player co cham dat hay khong dua vao raycast la he oxy duoc dat duoi chan player
        // neu raycast va cham voi layer whatground tra ve true, tuc la cham dat
        // nguoc lai tra ve false
        // vector3 use to check funcion grounded()
    }



    void Jump()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);


            playerState.jumping = false;

            // tuy chinh toc do nhay, ngan chan gia toc khi nha nut space
        }
        if (!playerState.jumping)
        {

            if (JumpBufferCounter > 0 && Grounded())
            {

                rb.velocity = new Vector3(rb.velocity.x, JumpForceZ);

                playerState.jumping = true;

            }
            else if (!Grounded() && airJumpcounter < AirJumpMax && Input.GetButton("Jump"))
            {
                playerState.jumping = true;

                airJumpcounter++;

                rb.velocity = new Vector3(rb.velocity.x, JumpForceZ);


                // neu k mat dat, so jump nho hon so jump max va an nut jump, co the nhay lan nua
                // hoan thanh bien airjumpcounter ++
            }
        }
        anim.SetBool("Jumping", !Grounded());
    }
    void UpdateJump()

    //su dung de goi ham update buoc nhay
    // su dung de tang do chinh xac cho cu nhay, su dung de tranh quan tinh do di chuyen walking
    //ham frame cho phep sai so bang so khung hinh toi da co the sai


    {
        if (Grounded())
        {
            playerState.jumping = false;// neu tren mat dat thi dat bang 0
            airJumpcounter = 0;
        }
        if (Input.GetButtonDown("Jump"))
        {
            JumpBufferCounter = JumpBufferFrame;
        }
        else
        {
            JumpBufferCounter--;
        }
    }
}



