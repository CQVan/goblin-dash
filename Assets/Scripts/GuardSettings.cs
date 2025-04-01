using UnityEngine;

[CreateAssetMenu(fileName = "GuardSettings", menuName = "Scriptable Objects/GuardSettings")]
public class GuardSettings : ScriptableObject
{
    [Header("Detection Settings")]
    public float detectionStartDistance = 5.0f;
    public float detectionAngle = 45.0f;
    public float detectionRate = 2.0f;
    [Tooltip("The detection rate of the guard when the player is sneaking while being detected")]
    public float sneakingDetectionRate = 1.0f;
    [Tooltip("The detection distance of the guard when the player is sneaking while being detected")]
    public float sneakingDetectionDistance = 3.5f;
}
