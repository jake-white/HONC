using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{
    Planet target = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void StartExplosion() {
        GetComponent<AudioSource>().Play();
        if(target != null) {
            target.Damage(0.3f);
        }
    }

    public void FinishExplosion() {
        Destroy(this.gameObject);
    }

    public void SetTarget(Planet p) {
        target = p;
    }
}
