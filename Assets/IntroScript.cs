using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroScript : MonoBehaviour
{
    public TextMeshProUGUI introText;
    int page = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            ++page;
            if(page == 1) {
                introText.text = "The last system of your contract, System No.1876, should be an easy one. \n\nAs usual, planets are our objective. Moons don't make money. And hit asteroids for extra elements.";
            }
            if(page == 2) {
                introText.text = "We'll bring you home when all 7 planets sustain <color=#FF9696>Life</color>.\n\n" +
                    "Deliver the building blocks of <color=#FF9696>Life</color> using WASD + mouse to move and SHIFT to warp.";

            }
            if(page == 3) {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
