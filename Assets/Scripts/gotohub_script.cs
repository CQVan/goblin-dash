using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotohub_script : MonoBehaviour
{
    private Collider keyCol;
    private Light light;
    private MeshRenderer mr;
    [SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyCol = transform.GetChild(2).GetComponent<Collider>();
        light = transform.GetChild(1).GetComponent<Light>();
        mr = transform.GetChild(2).GetComponent<MeshRenderer>();
        keyCol.enabled = false;
        light.enabled = false;
        mr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("drop_off").Length ==0)
        {
            keyCol.enabled = true;
            light.enabled = true;
            mr.enabled = true;
        }
    }

}
