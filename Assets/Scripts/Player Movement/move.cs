using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class move : MonoBehaviour
{
    [Header("Movement Variables")]

    public float currentSpeed = 0.0f;

    public float moveSpeed = -5.0f;

    private bool isSneaking;

    public float sneakSpeed = 0.5f;

    //Camera Variables
    public Transform cameraTransform;

    public float rotationSpeed = 10.0f;

    private Rigidbody rb;

    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashStartTime;
    private float lastDashTime;
    private Vector3 dashDirection;

    private Animator animator;
    private float goblinSpeed;
    private Vector3 lastPosition;
    private Rigidbody goblinBody;


    void Start()
    {
        currentSpeed = moveSpeed;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        goblinBody = GetComponent<Rigidbody>();

    }



    private void Update()
    {

        // Handling animation cases
        if (goblinBody.linearVelocity.magnitude > 0) // Player is moving
        {
            if (Input.GetKey(KeyCode.Space))
                animator.SetBool("IsJumping", true);
            else
                animator.SetBool("IsJumping", false);

            animator.SetBool("IsMoving", true);

            //Checking if player is crouching while moving
            if (Input.GetKey(KeyCode.LeftControl))
                animator.SetBool("IsCrouching", true);
            else
                animator.SetBool("IsCrouching", false);
        }
        else // Player is Idle
        {
            if (Input.GetKey(KeyCode.Space))
                animator.SetBool("IsJumping", true);
            else
                animator.SetBool("IsJumping", false);

            animator.SetBool("IsMoving", false);

            // Checking if player is crouching while idle
            if (Input.GetKey(KeyCode.LeftControl))
                animator.SetBool("IsCrouching", true);
            else
                animator.SetBool("IsCrouching", false);
        }
    }



    public void playerMovement()
    {
        //Get the horizontal and vertical axes
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Normalizes the vector3 to keep diagonal movement the same speed
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && moveDirection.magnitude > 0.1f && !isDashing)
        {
            StartDash(moveDirection);
        }


        if (isDashing)
        {
            rb.linearVelocity = new Vector3(dashDirection.x * dashSpeed, rb.linearVelocity.y, dashDirection.z * dashSpeed);

            if (Time.time >= dashStartTime + dashDuration)
            {
                isDashing = false;
            }

            return; // Skip regular movement during dash
        }



        if (moveDirection.magnitude >= 0.1f)
        {
            // Get camera's forward direction without the Y component
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            // Calculate movement direction relative to camera
            Vector3 move = cameraForward * moveDirection.z + cameraTransform.right * moveDirection.x;
            move.Normalize();

            // Rotate player towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            //Apply movement (One I created beforehand)
            rb.linearVelocity = new Vector3(move.x * currentSpeed, rb.linearVelocity.y, move.z * currentSpeed);
        }


        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            currentSpeed = moveSpeed * sneakSpeed;
            isSneaking = true;
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            currentSpeed = moveSpeed = 5.0f;
            isSneaking= false;
        }

    }


    public void StartDash(Vector3 inputDirection)
    {
        isDashing = true;
        dashStartTime = Time.time;
        lastDashTime = Time.time;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 move = cameraForward * inputDirection.z + cameraTransform.right * inputDirection.x;
        dashDirection = move.normalized;
    }

}
