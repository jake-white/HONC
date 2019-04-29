using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Rigidbody rb;
    float hydrogen, oxygen, nitrogen, carbon;
    public GameObject ocean, atmosphere, continent;
    public float maxLevels = 1.0f, rotationModifier = 1.0f;
    public Color initialColor, finalColor;
    string planetName;
    public bool isDemo, isMoon;
    bool interacting = false;
    bool eventTriggered = false, onceFinished = false;
    float timeEventTriggered = 0;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
        UpdateAppearance();
        if(isDemo) {
            hydrogen = oxygen = nitrogen = carbon = maxLevels;
        }
        UpdateAppearance();
    }
    
    void FixedUpdate() {
        transform.Rotate(0, rotationModifier*Time.deltaTime, 0);
    }

    public void AddElement(float h, float o, float n, float c) {
        bool alreadyfinished = false;
        if (IsFinished()) {
            alreadyfinished = true;
        }
        this.hydrogen += h;
        this.oxygen += o;
        this.nitrogen += n;
        this.carbon += c;

        this.hydrogen = Mathf.Clamp(this.hydrogen, 0, maxLevels);
        this.oxygen = Mathf.Clamp(this.oxygen, 0, maxLevels);
        this.nitrogen = Mathf.Clamp(this.nitrogen, 0, maxLevels);
        this.carbon = Mathf.Clamp(this.carbon, 0, maxLevels);
        UpdateAppearance();
        if(IsFinished() && !alreadyfinished) {
            //play a sfx or smth
            onceFinished = true;
        }
    }

    void UpdateAppearance() {
        float atmosphereValue = ((GetOxygen(true) + GetNitrogen(true)) / 2) / 10;
        float oceanValue = 0.95f + (0.05f * (GetHydrogen(true) * GetOxygen(true)));

        float colorValue = GetHydrogen(true) * GetOxygen(true) * GetNitrogen(true) * GetCarbon(true);
        float redNeeded = (finalColor.r - initialColor.r) * colorValue;
        float greenNeeded = (finalColor.g - initialColor.g) * colorValue;
        float blueNeeded = (finalColor.b - initialColor.b) * colorValue;

        Color currentColor = new Color(initialColor.r + redNeeded, initialColor.g + greenNeeded, initialColor.b + blueNeeded);

        ocean.transform.localScale = new Vector3(oceanValue, oceanValue, oceanValue);
        Material atmosphereMat = atmosphere.GetComponent<MeshRenderer>().material;
        atmosphereMat.color = new Color(atmosphereMat.color.r, atmosphereMat.color.g, atmosphereMat.color.b, atmosphereValue);

        Material continentMat = continent.GetComponent<MeshRenderer>().material;
        continentMat.color = currentColor;

        if(interacting) {
            UIController.instance.UpdatePlanetText(this);
        }
    }

    public void Interact(PlayerController p) {
        interacting = true;
        UIController.instance.InteractPlanet(this);
    }

    public void Leave(PlayerController p) {
        interacting = false;
        UIController.instance.LeavePlanet(this);
    }

    public float GetHydrogen(bool asPercent) {
        if(asPercent) {
            return hydrogen / maxLevels;
        }
        else return hydrogen;
    }
    public float GetOxygen(bool asPercent) {
        if (asPercent) {
            return oxygen / maxLevels;
        }
        else return oxygen;
    }
    public float GetNitrogen(bool asPercent) {
        if (asPercent) {
            return nitrogen / maxLevels;
        }
        else return nitrogen;
    }
    public float GetCarbon(bool asPercent) {
        if (asPercent) {
            return carbon / maxLevels;
        }
        else return carbon;
    }

    public void SetName(string newName) {
        planetName = newName;
    }

    public string GetName() {
        return planetName;
    }

    public bool IsFinished() {
        return this.hydrogen >= maxLevels && this.oxygen >= maxLevels && this.nitrogen >= maxLevels && this.carbon >= maxLevels;
    }

    public bool WasOnceFinished() {
        return onceFinished;
    }

    public void TriggerEvent() {
        timeEventTriggered = Time.time;
        eventTriggered = true;
    }

    public bool IsEventTriggered() {
        return eventTriggered;
    }

    public void Damage(float damagePercent) {
        float damageValue = damagePercent * maxLevels;
        hydrogen -= damageValue;
        oxygen -= damageValue;
        nitrogen -= damageValue;
        carbon -= damageValue;

        this.hydrogen = Mathf.Clamp(this.hydrogen, 0, maxLevels);
        this.oxygen = Mathf.Clamp(this.oxygen, 0, maxLevels);
        this.nitrogen = Mathf.Clamp(this.nitrogen, 0, maxLevels);
        this.carbon = Mathf.Clamp(this.carbon, 0, maxLevels);

        UpdateAppearance();
    }
}
