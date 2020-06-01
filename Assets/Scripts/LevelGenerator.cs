using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private List<BoidControl> boids = new List<BoidControl>();
    public BoidControl boid;

    public int numBoids;
    public BoidSettings settings;

    void Start()
    {
        GameObject ground = GameObject.Find("Plane");
        settings.xBound = ground.GetComponent<MeshFilter>().mesh.bounds.size.x / 2 * ground.transform.localScale.x;
        settings.zBound = ground.GetComponent<MeshFilter>().mesh.bounds.size.z / 2 * ground.transform.localScale.z;
        settings.yBound = 10f;

        for (int i = 0; i < numBoids; i++) {
            BoidControl newBoid = Instantiate(boid);
            newBoid.Init(settings);
            boids.Add(newBoid);
        }
    }

    void Update()
    {
        for(int i = 0; i < boids.Count; i++) {
            boids[i].flockCenter = MoveToCenter(boids[i], boids);
            boids[i].spacerForce = MoveAwayFromOthers(boids[i], boids);
            boids[i].flockForce = FlockWithOthers(boids[i], boids);
            boids[i].boundForce = BoundPosition(boids[i]);
        }
        
        for(int i = 0; i < boids.Count; i++) boids[i].UpdateBoid();
    }

    Vector3 MoveToCenter(BoidControl boid, List<BoidControl> boids) {
        Vector3 center = Vector3.zero;
        for(int i = 0; i < boids.Count; i++) {
            if(!boids[i].Equals(boid)) center += boids[i].transform.position;
        }
        return center / (boids.Count - 1);
    }

    Vector3 MoveAwayFromOthers(BoidControl boid, List<BoidControl> boids) {
        Vector3 c = Vector3.zero;
        for(int i = 0; i < boids.Count; i++) {
            if(!boids[i].Equals(boid) && Vector3.Distance(boids[i].transform.position, boid.transform.position) < settings.minDist)
                c -= (boids[i].transform.position - boid.transform.position);
        }
        return c;
    }

    Vector3 FlockWithOthers(BoidControl boid, List<BoidControl> boids)
    {
        Vector3 vel = Vector3.zero;
        for(int i = 0; i < boids.Count; i++)
        {
            if (!boids[i].Equals(boid) && Vector3.Distance(boids[i].transform.position, boid.transform.position) < settings.flockRadius) 
                vel += boids[i].vel;
        }
        return vel / (boids.Count - 1);
    }

    Vector3 BoundPosition(BoidControl boid)
    {
        Vector3 v = Vector3.zero;

        if (boid.transform.position.x < -settings.xBound) v = new Vector3(settings.boundFactor, v.y, v.z);
        else if (boid.transform.position.x > settings.xBound) v = new Vector3(-settings.boundFactor, v.y, v.z);
        if (boid.transform.position.z < -settings.zBound) v = new Vector3(v.x, v.y, settings.boundFactor);
        else if (boid.transform.position.z > settings.zBound) v = new Vector3(v.x, v.y, -settings.boundFactor);
        if (boid.transform.position.y < -settings.yBound) v = new Vector3(v.x, settings.boundFactor, v.z);
        else if (boid.transform.position.y > settings.yBound) v = new Vector3(v.x, -settings.boundFactor, v.z);

        return v;
    }
}
