using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginGameController : MonoBehaviour {

    public Button playButton, instructionButton;
    public Dropdown playerCountDropdown;
	// Use this for initialization
	void Start () {
        Button playBtn = playButton.GetComponent<Button>();
        Button instructionBtn = instructionButton.GetComponent<Button>();
        Dropdown countDropdown = playerCountDropdown.GetComponent<Dropdown>();
        playBtn.onClick.AddListener(delegate { LoadGame(countDropdown.value); });
        instructionBtn.onClick.AddListener(LoadInstruction);
	}
    
    void LoadGame(int value)
    {
        ApplicationModel.playerCount = value + 2;
        SceneManager.LoadScene("MainGame");
    }
    
    void LoadInstruction()
    {
        SceneManager.LoadScene("Instruction Scene");
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
