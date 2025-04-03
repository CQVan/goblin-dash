using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class move : MonoBehaviour
{
    [Header("Movement Variables")]

    public float currentSpeed = 0.0f;

     public float moveSpeed = 5.0f;



     public float sneakSpeed = 0.5f;

     public bool controlCheck = true;

    

    private Rigidbody rb;
    

    

    void Start()
    {
        currentSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
    }

    

    

    
    public void playerMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");


        rb.linearVelocity = new Vector3(moveX * currentSpeed, rb.linearVelocity.y, moveZ * currentSpeed); //Moves the object in the direction inputed with through the linear velocity of the rigidbody

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            currentSpeed = moveSpeed * sneakSpeed;
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            currentSpeed = moveSpeed = 5.0f;
            
        }
    }
    

}
