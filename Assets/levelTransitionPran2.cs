using UnityEngine;
using UnityEngine.SceneManagement;

public class levelTransitionPran2 : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("Pranav_lvl2_test");
    }
}
