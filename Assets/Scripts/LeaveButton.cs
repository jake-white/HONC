using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LeaveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Color hoverColor = new Color32(255, 150, 150, 255);
    Color normalColor = new Color32(255, 255, 255, 255);

    void Start() {
        
    }
    
    void Update() {

    }
    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<TextMeshProUGUI>().color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<TextMeshProUGUI>().color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        UIController.instance.LeavePlanet(PlayerController.instance.GetCurrentPlanet());
    }
}
