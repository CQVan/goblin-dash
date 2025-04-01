using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{

    [Header("Movement Variables")]
    //[SerializeField] private float moveSpeed = 5.0f;
    //[SerializeField] private float currentSpeed = 0f;
    
    
    //[SerializeField] private float sneakSpeed = 2.5f;
    

    [Header("Grounded check")]
    [SerializeField] private bool isGrounded;

    [Header("Dash Variables")]

    private move playerMove;
    private jump playerJump;
    private sneak playerSneak;
    private Rigidbody rb;

    private void Start()
    {
        //currentSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<jump>();
        playerMove = gameObject.GetComponent<move>();
        playerSneak = gameObject.GetComponent<sneak>(); 
    }

    
    void Update()
    {
        #region Movement
        playerMove.playerMove();
        
        
        #endregion

        #region jump
        //If the player is grounded and they press the space key, they jump
        if ( isGrounded && (Input.GetKeyDown(KeyCode.Space))){
            playerJump.playerJump();
        }
        #endregion

       /*
        if (isGrounded)
        {
            playerSneak.playerSneak();
        }
       */

        //Sneak Code
        //sneakPlayer();

    }
    
    /*
    private void sneakPlayer()
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
    */

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
