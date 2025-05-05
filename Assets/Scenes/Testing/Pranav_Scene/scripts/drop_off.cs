using System.Collections;
using TMPro;
using UnityEngine;

public class drop_off : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject player;
    private MeshRenderer rend;
    private Collider col;
    private Transform ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.enabled = false;
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        ps = transform.GetChild(0);
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
            rend.enabled = false;
            col.enabled = false;
            ps.gameObject.SetActive(false);
        }
    }

    private IEnumerator showCompleted()
    {
        text.enabled=true;
        yield return new WaitForSeconds(5f);
        text.enabled=false;
        Destroy(gameObject);
    }
}
