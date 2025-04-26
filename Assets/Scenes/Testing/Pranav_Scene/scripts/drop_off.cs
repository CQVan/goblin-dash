using System.Collections;
using TMPro;
using UnityEngine;

public class drop_off : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == player)
        {
            StartCoroutine(showCompleted());
        }
    }

    private IEnumerator showCompleted()
    {
        text.enabled=true;
        yield return new WaitForSeconds(5f);
        text.enabled=false;
    }
}
