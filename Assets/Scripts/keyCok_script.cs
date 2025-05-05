using UnityEngine;
using UnityEngine.SceneManagement;

public class keyCok_script : MonoBehaviour
{
    [SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            SceneManager.LoadScene("Hub World");
        }
    }
}
