using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsText : MonoBehaviour {
    public TextMeshProUGUI introText;
    int page = 0;
    // Start is called before the first frame update
    void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            ++page;
            if (page == 1) {
                float percent = 0;
                percent = GameObject.Find("GameController").GetComponent<GameController>().GetPercent();
                introText.text = "You restored " + percent + "% <color=#FF9696>Life</color> to the system.\n\n" +
                    "Game by Jake White (@squirrelboyVGC)";
            }
            if (page == 2) {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
