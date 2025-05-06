using UnityEngine;
using UnityEngine.SceneManagement;

public class levelTransitionPran1 : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("Pranav_lvl_test");
    }


}
