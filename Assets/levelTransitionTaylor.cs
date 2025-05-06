using UnityEngine;
using UnityEngine.SceneManagement;

public class levelTransitionTaylor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("Taylor_lvl");
    }
}
