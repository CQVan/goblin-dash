using UnityEngine;

public class jump : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpHeight = 2f;

    public float gravity = -9.81f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void playerJump()
    {

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity), rb.linearVelocity.z);


    }


}
