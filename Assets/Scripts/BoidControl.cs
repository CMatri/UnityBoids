using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidControl : MonoBehaviour
{
    [HideInInspector] public Vector3 flockCenter;
    [HideInInspector] public Vector3 spacerForce;
    [HideInInspector] public Vector3 flockForce;
    [HideInInspector] public Vector3 boundForce;
    [HideInInspector] public Vector3 vel;
    [HideInInspector] public Vector3 acc;
    
    private BoidSettings settings;
    
    public void Init(BoidSettings settings) {
        this.settings = settings;
        transform.position = new Vector3((int) Random.Range(-settings.xBound, settings.xBound), (int)Random.Range(-settings.yBound, settings.yBound), (int) Random.Range(-settings.zBound, settings.zBound));    
        vel = Vector3.zero;
        acc = Vector3.zero;
    }

    public void UpdateBoid()
    {
        if (settings.normalizeForces)
        {
            ApplyNormalizedForce(flockCenter);
            ApplyNormalizedForce(spacerForce);
            ApplyNormalizedForce(flockForce);
            ApplyNormalizedForce(boundForce);
        } 
        else 
        {
            acc += flockCenter * settings.flockCenterFactor;
            acc += spacerForce;
            acc += flockForce;
            acc += boundForce;
        }

        vel += acc * Time.deltaTime;
        vel = vel.normalized * Mathf.Clamp(vel.magnitude, settings.minSpeed, settings.maxSpeed);
        transform.localPosition += vel * Time.deltaTime;
    }

    private void ApplyNormalizedForce(Vector3 v)
    {
        acc += Vector3.ClampMagnitude(v.normalized * settings.maxSpeed - vel, settings.maxSteer);
    }
}
