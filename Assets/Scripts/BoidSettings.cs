using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    [HideInInspector] public float xBound, zBound, yBound;
    public float maxSpeed, minSpeed, maxSteer, minDist, boundFactor, flockRadius;
    public float flockCenterFactor;
    public bool normalizeForces;
}
