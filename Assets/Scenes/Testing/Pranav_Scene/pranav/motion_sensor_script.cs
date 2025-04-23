using System.Collections;
using UnityEngine;

public class motion_sensor_script : MonoBehaviour
{
    [SerializeField] private GameObject sensor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(toggleSensor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator toggleSensor()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            OnOff();
        }
    }

    private void OnOff()
    {
        if (sensor.activeSelf)
        {
            sensor.SetActive(false);
        }
        else
        {
            sensor.SetActive(true);
        }
        
    }
}
