using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    public float walkspeed;
    public float sprintspeed;
    public float slideSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    bool crouching;

    [Header("Keybings")]
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slode Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Shooting")]
    [SerializeField] Animator anim;
    public GameObject bullet;
    public GameObject gun;
    public int ammo;
    public float baseReloadSpeed;
    [SerializeField] float reloadSpeed;
    public float baseShootDelay;
    [SerializeField] float shootDelay;
    [SerializeField] bool reloading;
    [SerializeField] bool shotFired;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public bool sliding;

    // Start is called before the first frame update
    void Start()
    {
        reloadSpeed = baseReloadSpeed;
        shootDelay = baseShootDelay;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKey(JumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        // crouching
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
        }

        //shooting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(ammo != 0 && shotFired == false && reloading == false)
            {
                anim.SetTrigger("recoil");
                ammo--;
                shotFired = true;
                Instantiate(bullet, gun.transform.position, gun.transform.rotation);
            }
            else if(ammo == 0 && reloading == false)
            {
                anim.SetTrigger("reload");
                reloading = true;
            }
        }
        //force reload
        if(Input.GetKey(KeyCode.R) && ammo != 6 && reloading == false)
        {
            anim.SetTrigger("reload");
            reloading = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //reload
        if(reloading == true)
        {
            reloadSpeed -= 1f * Time.deltaTime;
            if(reloadSpeed <= 0)
            {
                reloadSpeed = baseReloadSpeed;
                ammo = 6;
                reloading = false;
            }
        }

        //recoil
        if(shotFired == true)
        {
            shootDelay -= 1f * Time.deltaTime;
            if(shootDelay <= 0)
            {
                shootDelay = baseShootDelay;
                shotFired = false;
            }
        }

        //groundCheck
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        myInput();
        SpeedControl();
        StateHandler();

        //Apply Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void StateHandler()
    {
        // Mode Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if(OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintspeed;
            }
        }

        // Mode = crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode = Sprinting
        if(grounded && Input.GetKey(SprintKey) && !crouching)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintspeed;
        }

        // Mode = Walking
        else if (grounded && !crouching)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkspeed;
        }

        // Mode = Air
        else
        {
            state = MovementState.air;
        }

        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && speed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothySlowPlayer());
        }
        else
        {
            speed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothySlowPlayer()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - speed);
        float startValue = speed;

        while (time < difference)
        {
            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            yield return null;
        }

        speed = desiredMoveSpeed;
    }


    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * speed * 20f, ForceMode.Force);
            
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // on Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        //in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn grav off while on slope
        rb.useGravity = !OnSlope();
    }
    private void SpeedControl()
    {
        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > speed)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }

        //limit speed on ground
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            //limit velocity when it goes over
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }



    }
    private void Jump()
    {
        exitingSlope = true;

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void resetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }
    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
