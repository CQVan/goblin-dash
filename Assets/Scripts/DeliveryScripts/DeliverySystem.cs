using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public static DeliverySystem instance;
    public List<PackageDelivery> deliveries = new List<PackageDelivery>();

    PackageDelivery activeDelivery;

    bool playerReady;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerReady)
        {
            AddDelivery();
        }
    }

    void AddDelivery()
    {
        if (activeDelivery == null)
        {
            activeDelivery = deliveries[Random.Range(0,deliveries.Count)];

        }
        Debug.Log("New Delivery Picked up");
    }

    public int ReadActiveDelivery()
    {
        return activeDelivery.id;
    }


    public void CompleteDelivery(int _id)
    {
        if (activeDelivery == null)
        {

        }
        else if (activeDelivery.id == _id)
        {
            Debug.Log("Delivery number " + activeDelivery.id + " is complete");

            activeDelivery = null;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerReady = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerReady = false;
        }
    }

}
