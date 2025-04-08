using System.Collections;
using UnityEngine;

public class door_unlock : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject player;
    [SerializeField] private float time = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool opening = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player && opening == false) 
        {
            opening = true;
            StartCoroutine(doorUnlock());
            gameObject.GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator doorUnlock()
    {
        Quaternion start = door.transform.rotation;
        Quaternion end = start * Quaternion.Euler(0f, -90f, 0f);

        float curTime = 0f;
        while(curTime < time)
        {
            door.transform.rotation = Quaternion.Slerp(start, end, curTime/time);
            curTime += Time.deltaTime;
            yield return null;
        }
        door.transform.rotation = end;
    }
}
