using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float battery = 100.0f;
    public Camera cam;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawInput, pitchInput, rollInput, thrustInput;
        yawInput = Mathf.Clamp(Input.GetAxis("Yaw"), -1, 1) * .007f;
        pitchInput = Mathf.Clamp(Input.GetAxis("Pitch"), -1, 1) * .007f;
        rollInput = Mathf.Clamp(Input.GetAxis("Roll"), -1, 1) * .007f;
        thrustInput = Mathf.Clamp(Input.GetAxis("Thrust"), -1, 1) * 1f;
        rb.AddRelativeTorque(-pitchInput, yawInput, -rollInput);
        rb.AddRelativeForce(0, 0, thrustInput);
    }

    public void Interact(Planet p)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "InteractionCollider")
        {
            Interact(other.transform.parent.GetComponent<Planet>());
        }
    }

}
