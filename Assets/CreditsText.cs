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
                int percent = 0;
                percent = (int) Mathf.Floor(GameObject.Find("GameController").GetComponent<GameController>().GetPercent());
                introText.text = "You restored " + percent + "% <color=#FF9696>Life</color> to the system.\n\n" +
                    "Game by Jake White (@squirrelboyVGC). Thanks to Ben Busche for the music tracks!";
            }
            if (page == 2) {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
