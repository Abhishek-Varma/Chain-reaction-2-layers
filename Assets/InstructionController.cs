using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour {

    public Button backButton;
    // Use this for initialization
    void Start () {
        Button backBtn = backButton.GetComponent<Button>();
        backBtn.onClick.AddListener(goToHomeScreen);
    }
	
    void goToHomeScreen()
    {
        SceneManager.LoadScene("Home Scene");
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goToHomeScreen();
        }
    }
}
