using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{

    

    [Header("Variables")]
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool isDashing;

    [Header("Dash Variables")]

    private move playerMove;
    private jump playerJump;
    private dash playerDash;
    

    private void Start()
    {
        //currentSpeed = moveSpeed;
        
        playerJump = GetComponent<jump>();
        playerMove = GetComponent<move>();
        playerDash = GetComponent<dash>();
    }

    
    void Update()
    {
        #region Movement
        playerMove.playerMovement();
        
        
        #endregion

        #region jump
        //If the player is grounded and they press the space key, they jump
        if ( isGrounded && (Input.GetKeyDown(KeyCode.Space))){
            playerJump.playerJump();
        }
        #endregion



        #region Dash
        /*
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
            playerDash.Dash();
            
        }


        
        if (playerDash.dashCooldownTimer > 0)
        {
            playerDash.dashCooldownTimer -= Time.deltaTime;
        }
        */
        #endregion

    }



    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;

        else
        {
            isGrounded = false;
        }
    }

    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            isGrounded = false;
        }
            
    }
    
}
