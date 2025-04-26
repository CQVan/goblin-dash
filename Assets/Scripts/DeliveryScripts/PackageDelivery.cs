using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Delivery", menuName = "DeliverySystem")]
public class PackageDelivery : ScriptableObject
{
    public int money;
    public int id;
    public GameObject deliveryPoint;
}
