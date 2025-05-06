using UnityEngine;
using UnityEngine.SceneManagement;

public class levelTransitionBen : MonoBehaviour
{
    
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("Level 2 - Ben");
    }

}
