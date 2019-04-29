using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public Transform originalcam, angledcam, supernovacam;
    public static PlayerController instance = null;
    public GameObject ship, laserPrefab, raycastPoint;
    public ParticleSystem hydrogenParticles, oxygenParticles, nitrogenParticles, carbonParticles, starField;
    public ElementButton hydrogenButton, oxygenButton, nitrogenButton, carbonButton;
    public GameObject targetedObject = null;
    bool interacting = false;
    Rigidbody rb;
    Planet currentPlanet = null, lastPlanet = null;
    float hydrogen = 1.0f, oxygen = 1.0f, nitrogen = 1.0f, carbon = 1.0f;
    public float transferSpeed = 0.005f, lasercooldown = 2000;
    float lastFired;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }
    void Start() {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        cam.transform.SetPositionAndRotation(originalcam.position, originalcam.rotation);
        lastFired = -lasercooldown;
    }
    
    void FixedUpdate() {
        if(targetedObject != null) {
            Vector3 markerPos = targetedObject.transform.position;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(markerPos);
            Debug.Log(screenPoint);
            if(screenPoint.z > 0) {
                UIController.instance.SetMarker(screenPoint);
            }
            else {
                UIController.instance.UnSetMarker();
            }
        }
        else {
            UIController.instance.UnSetMarker();
        }

        RaycastHit hit;
        if (Physics.Raycast(raycastPoint.transform.position, transform.forward, out hit, Mathf.Infinity)) {
            if(hit.collider.tag == "InteractionCollider") {
                UIController.instance.SetCockpitName(hit.collider.transform.parent.GetComponent<Planet>().GetName());
                UIController.instance.CockpitNameUp();
            }
            else {
                UIController.instance.CockpitNameDown();
            }
        }
        else {
            UIController.instance.CockpitNameDown();
        }

        if (!interacting) {
            float yawInput, pitchInput, rollInput, thrustInput, warpInput;
            yawInput = Mathf.Clamp(Input.GetAxis("Yaw"), -1, 1) * .007f;
            pitchInput = Mathf.Clamp(Input.GetAxis("Pitch"), -1, 1) * .007f;
            rollInput = Mathf.Clamp(Input.GetAxis("Roll"), -1, 1) * .007f;
            thrustInput = Mathf.Clamp(Input.GetAxis("Thrust"), -1, 1) * 5.0f;
            warpInput = ((Mathf.Clamp(Input.GetAxis("Warp"), 0, 1) * 25.0f) + 1);
            if(warpInput > 1) {
                if(!starField.isPlaying) {
                    starField.Play();
                }
            }
            else {
                if(starField.isPlaying) {
                    starField.Stop();
                }
            }
            thrustInput *= warpInput;
            rb.AddRelativeTorque(-pitchInput, yawInput, -rollInput);
            rb.AddRelativeForce(0, 0, thrustInput);
        }

        if (interacting) {
            CheckElementalInputs();
        }
        else {
            if (Input.GetAxis("Fire") > 0 && Time.time > lastFired + lasercooldown) {
                GameObject newLaser = Instantiate(laserPrefab);
                newLaser.transform.position = transform.position;
                newLaser.transform.rotation = transform.rotation;
                lastFired = Time.time;
            }
        }
    }

    public void CheckElementalInputs() {
        if(currentPlanet != null) {
            if (hydrogenButton.IsClicked()) {
                if (hydrogen > 0 && currentPlanet.GetHydrogen(true) < 1.0f) {
                    hydrogen -= transferSpeed;
                    currentPlanet.AddElement(transferSpeed, 0, 0, 0);
                    if(!hydrogenParticles.isPlaying) {
                        hydrogenParticles.Play();
                    }
                }
                else {
                    hydrogenParticles.Stop();
                }
            }
            else {
                hydrogenParticles.Stop();
            }
            if (oxygenButton.IsClicked()) {
                if (oxygen > 0 && currentPlanet.GetOxygen(true) < 1.0f) {
                    oxygen -= transferSpeed;
                    currentPlanet.AddElement(0, transferSpeed, 0, 0);
                    if (!oxygenParticles.isPlaying) {
                        oxygenParticles.Play();
                    }
                }
                else {
                    oxygenParticles.Stop();
                }
            }
            else {
                oxygenParticles.Stop();
            }
            if (nitrogenButton.IsClicked()) {
                if (nitrogen > 0 && currentPlanet.GetNitrogen(true) < 1.0f) {
                    nitrogen -= transferSpeed;
                    currentPlanet.AddElement(0, 0, transferSpeed, 0);
                    if (!nitrogenParticles.isPlaying) {
                        nitrogenParticles.Play();
                    }
                }
                else {
                    nitrogenParticles.Stop();
                }
            }
            else {
                nitrogenParticles.Stop();
            }
            if (carbonButton.IsClicked()) {
                if (carbon > 0 && currentPlanet.GetCarbon(true) < 1.0f) {
                    carbon -= transferSpeed;
                    currentPlanet.AddElement(0, 0, 0, transferSpeed);
                    if (!carbonParticles.isPlaying) {
                        carbonParticles.Play();
                    }
                }
                else {
                    carbonParticles.Stop();
                }
            }
            else {
                carbonParticles.Stop();
            }
        }
    }

    public void Interact(Planet p) {
        currentPlanet = p;
        cam.transform.SetPositionAndRotation(angledcam.position, angledcam.rotation);
        UIController.instance.SetPlanetNameText(p.GetName());
        interacting = true;
        GetComponent<Rigidbody>().isKinematic = true;
        starField.Stop();
        currentPlanet.Interact(this);
        Cursor.lockState = CursorLockMode.None;
        transform.LookAt(p.transform);
        transform.position = Vector3.MoveTowards(transform.position, currentPlanet.transform.position, 1);
    }

    public void Leave(Planet p) {
        p.SetName(UIController.instance.GetPlanetNameText());
        cam.transform.SetPositionAndRotation(originalcam.position, originalcam.rotation);
        GetComponent<Rigidbody>().isKinematic = false;
        currentPlanet.Leave(this);
        Cursor.lockState = CursorLockMode.Locked;
        lastPlanet = currentPlanet;
        currentPlanet = null;
        interacting = false;
        GameController.instance.CheckEvents();
    }

    public void ForceLeave() {
        transform.position = Vector3.MoveTowards(transform.position, currentPlanet.transform.position, -10);
    }

    public Planet GetCurrentPlanet() {
        return currentPlanet;
    }

    public Planet GetLastPlanet() {
        return lastPlanet;
    }

    public float GetHydrogen() {
        return hydrogen;
    }

    public float GetOxygen() {
        return oxygen;
    }

    public float GetNitrogen() {
        return nitrogen;
    }

    public float GetCarbon() {
        return carbon;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "InteractionCollider") {
            Interact(other.transform.parent.GetComponent<Planet>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "InteractionCollider") {
            Leave(other.transform.parent.GetComponent<Planet>());
        }
    }

    public void FinalSequence() {
        interacting = true;
        rb.isKinematic = true;
        cam.transform.parent = null;
        cam.transform.SetPositionAndRotation(supernovacam.position, supernovacam.rotation);
    }

    public void AddElement(float h, float o, float n, float c) {
        this.hydrogen += h;
        this.oxygen += o;
        this.nitrogen += n;
        this.carbon += c;
        this.hydrogen = Mathf.Clamp(this.hydrogen, 0, 1.0f);
        this.oxygen = Mathf.Clamp(this.oxygen, 0, 1.0f);
        this.nitrogen = Mathf.Clamp(this.nitrogen, 0, 1.0f);
        this.carbon = Mathf.Clamp(this.carbon, 0, 1.0f);
    }

    public void SetMarker(GameObject target) {
        targetedObject = target;
    }

}
