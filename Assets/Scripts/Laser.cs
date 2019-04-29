using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 50, lifetime;
    float startOfLife = 0;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = PlayerController.instance.GetComponent<Rigidbody>().velocity + transform.forward * speed;
        startOfLife = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > startOfLife + lifetime) {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Asteroid") {
            Destroy(this.gameObject);
            other.GetComponent<Asteroid>().Explode();
            PlayerController.instance.AddElement(0.1f, 0.1f, 0.1f, 0.1f);
        }
        else if(other.tag == "Planet") {
            Destroy(this.gameObject);
            other.GetComponent<Planet>().Damage(0.05f);
        }
    }
}
