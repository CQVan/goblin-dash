using UnityEngine;

public class sneak : MonoBehaviour
{

    public float sneakSpeed = .5f;

    private move playerMove;
    private float CurrentSpeed;
    private float MoveSpeed;


    /*
    private void Start()
    {
        playerMove = GetComponent<move>();
        CurrentSpeed = GetComponent<move>().currentSpeed;
        MoveSpeed = GetComponent<move>().moveSpeed;
    }

    public void playerSneak()
    {
        if ((Input.GetKey(KeyCode.LeftControl)))
        {
            CurrentSpeed = MoveSpeed * sneakSpeed;

        }
        else
        {
            CurrentSpeed = MoveSpeed;
        }
    }
    */
}
