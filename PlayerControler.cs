
 using UnityEngine;
using UnityEngine.Animations;

public class PlayerControler : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float Walkspeed = 1;// default setting walkspeed=1
  
    private float xAxis;

    [Header("Ground Checking")]
    [SerializeField] private float JumpForceZ = 45;
    [SerializeField] private Transform GroundCheck ;
    [SerializeField] private float groundcheckY = 0.2f;
    [SerializeField] private float groundcheckX = 0.5f;
    [SerializeField] private LayerMask Whatground;
    [SerializeField] private Animation Anim;
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        GetInputs();
        Move();
        Jump();

    }
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        //lib co san
    }
    private void Move()
    {
        rb.velocity = new Vector2(Walkspeed * xAxis , rb.velocity.y);
        //lenh move left right
        anim.SetBool ("Walking", rb.velocity.x != 0 && Grounded());
        //tao condictional
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
    }
    void Jump()
    {
        if(Input.GetButtonUp("Jump")&& rb.velocity.y > 0)
        {
            rb.velocity =new Vector2(rb.velocity.x,0);
            // tuy chinh toc do nhay, ngan chan gia toc khi nha nut space
        }
        if(Input.GetButtonDown("Jump")&& Grounded() )
        {
            rb.velocity = new Vector3(rb.velocity.x, JumpForceZ);
        }
        anim.SetBool("Jumping", !Grounded());
    }
}


