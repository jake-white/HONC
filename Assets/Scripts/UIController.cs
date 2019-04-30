using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public TextMeshProUGUI planetText, leaveButton, completion, rightScreenText, cockpitnametext;
    public Image cockpitnamebar, meters, leftscreen, rightscreen;
    public Image hydrogen, oxygen, nitrogen, carbon, marker, controls;
    public TMP_InputField planetName;
    public GameObject cockpit;
    bool nameUp = false;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start() {
        ClearScreens();
    }
    
    void Update() {
        hydrogen.rectTransform.anchorMax = new Vector2(1, PlayerController.instance.GetHydrogen());
        oxygen.rectTransform.anchorMax = new Vector2(1, PlayerController.instance.GetOxygen());
        nitrogen.rectTransform.anchorMax = new Vector2(1, PlayerController.instance.GetNitrogen());
        carbon.rectTransform.anchorMax = new Vector2(1, PlayerController.instance.GetCarbon());
    }

    public void InteractPlanet(Planet p) {
        cockpit.gameObject.SetActive(false);
        meters.gameObject.SetActive(true);
        leftscreen.gameObject.SetActive(false);
        rightscreen.gameObject.SetActive(false);
        leaveButton.gameObject.SetActive(true);
        planetName.gameObject.SetActive(true);
        planetText.gameObject.SetActive(true);
        UpdatePlanetText(p);
    }

    public void UpdatePlanetText(Planet p) {
        planetText.text = "";
        planetText.text += "\n--Levels--";
        planetText.text += "\nHydrogen: " + Mathf.Floor(p.GetHydrogen(true) * 100) + "%";
        planetText.text += "\nOxygen: " + Mathf.Floor(p.GetOxygen(true) * 100) + "%";
        planetText.text += "\nNitrogen: " + Mathf.Floor(p.GetNitrogen(true) * 100) + "%";
        planetText.text += "\nCarbon: " + Mathf.Floor(p.GetCarbon(true) * 100) + "%";
    }

    public void LeavePlanet(Planet p) {
        cockpit.gameObject.SetActive(true);
        meters.gameObject.SetActive(false);
        leftscreen.gameObject.SetActive(true);
        rightscreen.gameObject.SetActive(true);
        leaveButton.gameObject.SetActive(false);
        planetName.gameObject.SetActive(false);
        planetText.gameObject.SetActive(false);
        planetText.text = "";
        PlayerController.instance.ForceLeave();
    }

    public void UpdateFinishedText(int finished, int max) {
        completion.text = finished + "/" + max;
    }

    public string GetPlanetNameText() {
        return planetName.text;
    }

    public void SetPlanetNameText(string newName) {
        planetName.text = newName;
    }

    public void SetScreen(SpaceEvent.SpaceEventType type, Planet target) {
        if(type == SpaceEvent.SpaceEventType.Asteroid) {
            leftscreen.GetComponent<Animator>().Play("Asteroid");
            rightScreenText.text = "Asteroid approaching " + target.GetName() + "!";
        }
        if (type == SpaceEvent.SpaceEventType.Comet) {
            leftscreen.GetComponent<Animator>().Play("Comet");
            rightScreenText.text = "Comet approaching " + target.GetName() + "!";
        }
        else if (type == SpaceEvent.SpaceEventType.SolarFlare) {
            leftscreen.GetComponent<Animator>().Play("SolarFlare");
            rightScreenText.text = "Solar flare striking " + target.GetName() + "!";
        }
    }

    public void ClearScreens() {
        leftscreen.GetComponent<Animator>().Play("Idle");
        rightScreenText.text = "";
    }

    public void SetCockpitName(string name) {
        cockpitnametext.text = name;
    }

    public void CockpitNameUp() {
        if(!nameUp && cockpitnamebar.isActiveAndEnabled) {
            cockpitnamebar.GetComponent<Animator>().Play("NameUp");
            nameUp = true;
        }
    }

    public void CockpitNameDown() {
        if (nameUp && cockpitnamebar.isActiveAndEnabled) {
            UIController.instance.SetCockpitName("");
            cockpitnamebar.GetComponent<Animator>().Play("NameDown");
            nameUp = false;
        }
    }

    public void FinalSequence() {
        //turning off the canvas
        gameObject.SetActive(false);
    }

    public void SetMarker(Vector2 screenPoint) {
        if(!marker.gameObject.activeSelf) {
            marker.gameObject.SetActive(true);
        }
        Vector2 canvasPos;
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPoint, Camera.main, out canvasPos);
        if (canvasPos.x < -Screen.width/2) {
            canvasPos.x = -Screen.width/2 + marker.GetComponent<RectTransform>().rect.width/2;
        }
        if (canvasPos.y < -Screen.height/2) {
            canvasPos.y = -Screen.height/2 + marker.GetComponent<RectTransform>().rect.height/2;
        }
        if (canvasPos.x > Screen.width/2) {
            canvasPos.x = Screen.width/2 - marker.GetComponent<RectTransform>().rect.width / 2;
        }
        if (canvasPos.y > Screen.height/2) {
            canvasPos.y = Screen.height/2 - marker.GetComponent<RectTransform>().rect.height / 2;
        }
        marker.GetComponent<RectTransform>().localPosition = canvasPos;
    }

    public void UnSetMarker() {
        if (marker.gameObject.activeSelf) {
            marker.gameObject.SetActive(false);
        }
    }

    public void ShowControls(bool show) {
        controls.gameObject.SetActive(show);
    }
}
