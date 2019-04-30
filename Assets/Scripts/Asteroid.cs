using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    GameObject target = null;
    public float speed = 10;
    public bool isComet = false, isEvent = false;
    bool forceStarted = false;
    public GameObject asteroidExplosion, mesh;
    public ParticleSystem tail, head;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && isComet && !forceStarted) {
            Vector3 direction = target.transform.position - transform.position;
            transform.LookAt(2* transform.position - target.transform.position);
            rb.AddRelativeForce(direction.normalized * speed, ForceMode.Impulse);
            forceStarted = true;
        }
        else if(!isEvent && isComet && !forceStarted) {
            transform.LookAt(2 * transform.position - transform.forward);
            rb.AddRelativeForce(-transform.forward * speed, ForceMode.Impulse);
            forceStarted = true;

        }
        else if(target != null && !forceStarted) {
            Vector3 direction = target.transform.position - transform.position;
            rb.AddRelativeForce(direction.normalized * speed, ForceMode.Impulse);
            rb.AddRelativeTorque(20, 20, 20, ForceMode.Impulse);
            forceStarted = true;
        }
    }

    public void SetTarget(GameObject newTarget) {
        target = newTarget;
    }

    private void OnCollisionEnter(Collision collision) {        
        if (collision.collider.tag == "Planet") {
            ExplodePlanet(collision.collider.GetComponent<Planet>());
        }
    }

    public void Explode() {
        GameObject explosion = Instantiate(asteroidExplosion);
        explosion.transform.position = transform.position;
        rb.isKinematic = true;
        Destroy(GetComponent<SphereCollider>());
        Destroy(mesh);
        Destroy(this.gameObject, 4.0f);
        if(isComet) {
            tail.Stop();
            head.Stop();
        }
        if(isEvent) {
            UIController.instance.ClearScreens();
        }
    }
    public void ExplodePlanet(Planet p) {
        GameObject explosion = Instantiate(asteroidExplosion);
        explosion.transform.position = transform.position;
        explosion.GetComponent<AsteroidExplosion>().SetTarget(p);
        UIController.instance.ClearScreens();
        rb.isKinematic = true;
        Destroy(GetComponent<SphereCollider>());
        Destroy(mesh);
        Destroy(this.gameObject, 4.0f);
        if (isComet) {
            tail.Stop();
            head.Stop();
        }
    }
}
