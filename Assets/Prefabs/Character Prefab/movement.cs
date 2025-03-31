using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float currentSpeed = 0f;
    
    
    [SerializeField] private float sneakSpeed = 2.5f;
    

    [Header("Grounded check")]
    [SerializeField] private bool isGrounded;

    [Header("Dash Variables")]
    

    private jump playerJump;
    private Rigidbody rb;

    private void Start()
    {
        currentSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<jump>();
        
    }

    
    void Update()
    {
        #region Movement

        //Get your input direction  and calculate the new vector from the given components of the axis
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");


        rb.linearVelocity = new Vector3(moveX * currentSpeed, rb.linearVelocity.y, moveZ * currentSpeed); //Moves the object in the direction inputed with through the linear velocity of the rigidbody

        #endregion

        #region jump
        //If the player is grounded and they press the space key, they jump
        if ( isGrounded && (Input.GetKeyDown(KeyCode.Space))){
            playerJump.playerJump();
        }
        #endregion

       



        //Sneak Code
        sneak();

    }
    

    private void sneak()
    {
        
        if (isGrounded && (Input.GetKey(KeyCode.LeftControl)))
        {
            currentSpeed = moveSpeed - sneakSpeed;
            
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
