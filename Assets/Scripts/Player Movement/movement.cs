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

    [Header("Dash Variables")]
    public ParticleSystem DustparticleSystem;
    private Vector3 previousPosition;

    private void Start()
    {
        //currentSpeed = moveSpeed;
        
        playerJump = GetComponent<jump>();
        playerMove = GetComponent<move>();
        playerDash = GetComponent<dash>();

        DustparticleSystem = GetComponent<ParticleSystem>();
        previousPosition = transform.position;
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

        //Controlls whether the particle system turns on and off within relation to the players position
        if (transform.position != previousPosition && isGrounded)
        {
            if (!DustparticleSystem.isPlaying)
            {
                DustparticleSystem.Play();
            }
        }
        else
        {
            if (DustparticleSystem.isPlaying)
            {
                DustparticleSystem.Stop();
            }
        }
        previousPosition = transform.position;
    }



    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;

        
    }

    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            isGrounded = false;
        }
        isGrounded = false;
            
    }
    
}
