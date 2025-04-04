using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{

    

    [Header("Grounded check")]
    [SerializeField] private bool isGrounded;

    [Header("Dash Variables")]

    private move playerMove;
    private jump playerJump;
    
    

    private void Start()
    {
        //currentSpeed = moveSpeed;
        
        playerJump = GetComponent<jump>();
        playerMove = gameObject.GetComponent<move>();
        
    }

    
    void FixedUpdate()
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
