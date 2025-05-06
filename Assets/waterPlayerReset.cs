using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class waterPlayerReset : MonoBehaviour
{
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Restart();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
