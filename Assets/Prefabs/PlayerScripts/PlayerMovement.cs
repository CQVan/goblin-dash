using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller; //Grabbing variables of movement from the character controller like the Move variable

    //Character speed and walking speed
    [SerializeField] public float speed = 12f; //Creates the speed of our player
    [SerializeField] public float walkSpeed = 6f;

    //Jump a& Gravity
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode walkKey = KeyCode.LeftShift;

    //Variables handing the ground check, layer of ground must have the "ground" tag in order for it to work
    public Transform groundCheck;
    public float groundDistance = 0.4f; //Sets the height of the isGrounded CheckSphere
    public LayerMask groundMask; //Anything within the groundMask will make isGrounded to be true

    [SerializeField] Vector3 velcoity; //constantly applies velocity to our player

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool inAir;

    //Start of wall running code //////////////////////////////////////////////////////////////////////////////////////////////////////
    
    //Wallrunning variables
    
    public Transform leftRunWallCheck;
    public Transform rightRunWallCheck;
    public float wallDistance = 0.4f;
    public LayerMask wallMask;

    public float maxWallrunTime, maxWallSpeed;
    bool isWallRight, isWallLeft;
    [SerializeField] bool isWallRunningLeft;
    [SerializeField] bool isWallRunningRight;
    [SerializeField] bool usingGravity;

    

    private void wallRunInput() //Controls the inputs necessary for a wall run
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            startWallRunLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            startWallRunRight();
        }
    }
    private void startWallRunLeft() //When the player is wall running on their left
    {
        inAir = false;
        usingGravity = false;
        isWallRunningLeft = true;

        if (!isWallRunningLeft)
        {
            stopWallRun();
        }

    }
    private void startWallRunRight() //When the player is wall running on their right
    {
        inAir = false;
        usingGravity = false;
        isWallRunningRight = true;

        if (!isWallRunningRight)
        {
            stopWallRun();
        }
    }
    private void stopWallRun() //Activates when the wall run stops
    {
        usingGravity = true;
        isWallRunningLeft = false;
        isWallRunningRight = false;
    }
    
    
    //WallRun code ends here //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Function handling players movement
    private void movePlayer()
    {
        //Get the inputs on the x and z axis. 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //Moves our character along the red and blue axis of our player object

        controller.Move(move * speed * Time.deltaTime);
    }

    //Function handling players jump
    private void playerJump()
    {
        if (Input.GetKey(jumpKey) && isGrounded)
        {
            velcoity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            
        }
    }

    //Function handling players walk
    private void playerWalk()
    {
        if (isGrounded && Input.GetKeyDown(walkKey))
        {
            speed = walkSpeed;
        }
        else if (Input.GetKeyUp(walkKey))
        {
            speed = 12f;
        }
    }

    // Update 
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Creates a sphere that checks if anything touches our ground mask and counts that as being on the ground

        //Creates a sphere that checks if anything touches a wall with the layer "RunWall" and counts the player as being on the wall. either left or right side.
        isWallRunningLeft = Physics.CheckSphere(leftRunWallCheck.position, wallDistance, wallMask);
        isWallRunningRight = Physics.CheckSphere(rightRunWallCheck.position, wallDistance, wallMask);
        
        if (isGrounded && velcoity.y < 0)
        {
            velcoity.y = -2f;
            inAir = false;
            isWallRunningLeft = false;
            isWallRunningRight = false;
            usingGravity = false;
        }
        else 
        {
            inAir = true;
            usingGravity = true;
        }

        

        movePlayer(); //Player movement function call

        playerWalk(); //Player walk call
      
        playerJump(); //Jump

        wallRunInput();



        //Apply gravity
        velcoity.y += gravity * Time.deltaTime; //Adds gravity to the y component of our velocity
        controller.Move(velcoity * Time.deltaTime);
        

    }
}
