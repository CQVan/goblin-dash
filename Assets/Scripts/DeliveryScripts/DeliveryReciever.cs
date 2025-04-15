using UnityEngine;

public class DeliveryReciever : MonoBehaviour
{
    public int deliveryId;
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && 
            DeliverySystem.instance != null)
        {
            DeliverySystem.instance.CompleteDelivery(deliveryId);        
        }
    }
}
