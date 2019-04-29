using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishFlare() {
        Debug.Log("Clearing the screen.");
        UIController.instance.ClearScreens();
    }
}
