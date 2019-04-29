using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    Color hoverColor = new Color32(255, 150, 150, 255);
    Color normalColor = new Color32(255, 255, 255, 255);
    bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Image>().color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().color = normalColor;
        clicked = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        clicked = true;
    }
    public void OnPointerUp(PointerEventData eventData) {
        clicked = false;
    }

    public bool IsClicked() {
        return clicked;
    }
}
