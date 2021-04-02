using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    //References
    private CharacterController controler;
    private Animator anim;

    //start get all components right 
    private void Start()
    {
        controler = GetComponent<CharacterController>();
        //Get animator component in children 
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //Movement called each frame
        Move();

        //Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
    }

    //Functions
    private void Move()
    {
        //check if the player is in the ground 
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Get values from axes vertical in Unity 1 or -1
        float moveZ = Input.GetAxis("Vertical");

        //transform in worldAxis
        moveDirection = new Vector3(0, 0, moveZ);
        //Transform in playerAxis
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            //State movement check
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                //Walk
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                //Run
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                //Idle
                Idle();
            }

            //To make sure we move the character in the speed we wan't with out his will be really realy slow
            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        //delta time is very usefull here because it will move by the time and not by the frames
        controler.Move(moveDirection * Time.deltaTime);

        //Aply gravity to our character
        velocity.y += gravity * Time.deltaTime;
        controler.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        //anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        //Set the value of layer to 1
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1f);
        anim.SetTrigger("Attack");

        //Whait few time
        yield return new WaitForSeconds(0.9f);
        //Set the value of layer again to 0 
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0f);
    }

}
