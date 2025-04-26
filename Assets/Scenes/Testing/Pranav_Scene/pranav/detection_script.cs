using UnityEngine;
using UnityEngine.SceneManagement;

public class detection_script : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIT");
        }
    }
}
