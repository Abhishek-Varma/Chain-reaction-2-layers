using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Button backButton;
    public GameObject onePrefab, twoPrefab, threePrefab, fourPrefab, fivePrefab, splitCirclePrefab;
    public Text playerWonText;
    public AudioClip stableSound, unstableSound, bombSound;
    static private AudioSource audioSource;
    static public Color[] playerColorArray;
    private GameObject prefabToInstantiate;
    static public int[, ,] board3D = new int[2, 8, 7];
    static public int[,,] copyBoard3D = new int[2, 8, 7];
    private int[] playerStatus;
    static public GameObject[,,] orbArr = new GameObject[2, 8, 7];
    static private GameObject[,,] copyOrbArr = new GameObject[2, 8, 7];
    static public int turn, flag, playerCount, countFrom, maxPlayerCount, playerWon;
    private int rowIndex, colIndex, layerIndex, floorX, floorY;
    private int movePlayed = -1;
    private static Boolean playerClicked;
    //static private int test = -1;
	// Use this for initialization
	void Start () {
        playerClicked = false;
        audioSource = GetComponent<AudioSource>();
        playerWonText.text = "";
        playerWon = 0;
        playerCount = ApplicationModel.playerCount;
        playerStatus = new int[playerCount];
        for (int i = 0; i < playerCount; i++)
            playerStatus[i] = 1; // 1 == Alive
        maxPlayerCount = playerCount * 10;
        turn = 10;
        flag = 0;
        countFrom = 0;
        layerIndex = 0;
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 8; j++)
                for (int k = 0; k < 7; k++)
                {
                    board3D[i, j, k] = 0;
                    orbArr[i, j, k] = null;
                }
        playerColorArray = new Color[8];
        playerColorArray[0] = new Color(246F / 255, 54F / 255, 54F / 255, 1);
        playerColorArray[1] = new Color(10F / 255, 223F / 255, 223F / 255, 1);
        playerColorArray[2] = new Color(55F / 255, 219F / 255, 83F / 255, 1);
        playerColorArray[3] = new Color(224F / 255, 234F / 255, 45F / 255, 1);
        playerColorArray[4] = new Color(87F / 255, 119F / 255, 236F / 255, 1);
        playerColorArray[5] = new Color(244F / 255, 187F / 255, 39F / 255, 1);
        playerColorArray[6] = new Color(232F / 255, 103F / 255, 197F / 255, 1);
        playerColorArray[7] = new Color(191F / 255, 54F / 255, 22F / 255, 1);

        playerColorArray[0] = new Color(0.965F, 0.212F, 0.212F, 1); //(246F / 255, 54F / 255, 54F / 255, 1)
        playerColorArray[1] = new Color(0.039F, 0.875F, 0.875F, 1); //10F / 255, 223F / 255, 223F / 255, 1
        playerColorArray[2] = new Color(0.216F, 0.859F, 0.325F, 1); //55F / 255, 219F / 255, 83F / 255, 1
        playerColorArray[3] = new Color(0.878F, 0.918F, 0.176F, 1); //224F / 255, 234F / 255, 45F / 255, 1
        playerColorArray[4] = new Color(0.341F, 0.467F, 0.925F, 1); //87F / 255, 119F / 255, 236F / 255, 1
        playerColorArray[5] = new Color(0.957F, 0.733F, 0.153F, 1); //244F / 255, 187F / 255, 39F / 255, 1
        playerColorArray[6] = new Color(0.910F, 0.404F, 0.773F, 1); //232F / 255, 103F / 255, 197F / 255, 1
        playerColorArray[7] = new Color(0.750F, 0.212F, 0.086F, 1); //191F / 255, 54F / 255, 22F / 255, 1
        ApplicationModel.onePrefab = onePrefab;
        ApplicationModel.twoPrefab = twoPrefab;
        ApplicationModel.threePrefab = threePrefab;
        ApplicationModel.fourPrefab = fourPrefab;
        ApplicationModel.fivePrefab = fivePrefab;
        ApplicationModel.splitCirclePrefab = splitCirclePrefab;
        ApplicationModel.stableSound = stableSound;
        ApplicationModel.unstableSound = unstableSound;
        ApplicationModel.bombSound = bombSound;
        Camera.main.backgroundColor = playerColorArray[0];
        Button backBtn = backButton.GetComponent<Button>();
        backBtn.onClick.AddListener(loadMainMenu);
    }
	
	// Update is called once per frame
	void Update () {

        //print("In Update");
        GameObject[] splitCircles;
        //splitCircles = GameObject.FindGameObjectsWithTag("Split Circle");
        //print("Split circ len: "+splitCircles.Length);

        if(movePlayed>=0)
        {
            ++movePlayed;
            splitCircles = GameObject.FindGameObjectsWithTag("Split Circle");
            if (splitCircles.Length != 0)
            {
                //waitForNextTurn = true;
                movePlayed = 0;
                //print("Split circ len: " + splitCircles.Length);
            }
            if (movePlayed == 20)
            {
                if (countFrom >= playerCount)
                {
                    checkWhoIsDead();   
                }
                else
                {
                    turn += 10;
                    Camera.main.backgroundColor = playerColorArray[(turn / 10) - 1];
                }
                movePlayed = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            loadMainMenu();
        }
        if (Input.GetMouseButtonDown(0) && playerWon == 0 && movePlayed==-1)
        {
            flag = 0;
            Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(clickPoint);
            floorX = (int)Mathf.Floor(clickPoint.x);
            floorY = (int)Mathf.Floor(clickPoint.y);
            Vector3 prefabPos = new Vector3(floorX + 0.5f, floorY + 0.5f, 0.0f);
            //rowIndex = floorY + 4;
            colIndex = floorX + 8;
            rowIndex = Mathf.Abs(floorY - 3);
            //print("floorY:" + floorY);
            if (floorY < 4 && floorY > -5)
            {
                if (floorX <= -1 && floorX >= -8) // left board
                {
                    layerIndex = 0;
                    colIndex = floorX + 8;
                }
                else if (floorX >= 1 && floorX <= 7)// right board
                {
                    layerIndex = 1;
                    colIndex = floorX - 1;
                }
                // Copy board if everything is alright
                playerClicked = true;
                if (board3D[layerIndex, rowIndex, colIndex] == 0) // checking if player clicked on an empty cell
                {
                    board3D[layerIndex, rowIndex, colIndex] = turn + 1;
                    prefabToInstantiate = onePrefab;
                    flag = 1;
                }
                else
                {
                    flag = 1;
                    switch (board3D[layerIndex, rowIndex, colIndex] - turn)
                    {
                        case 1:
                            // Make TWO
                            board3D[layerIndex, rowIndex, colIndex] += 1;
                            prefabToInstantiate = twoPrefab;
                            break;
                        case 2:
                            // Make THREE
                            board3D[layerIndex, rowIndex, colIndex] += 1;
                            prefabToInstantiate = threePrefab;
                            break;
                        case 3:
                            // Make FOUR
                            board3D[layerIndex, rowIndex, colIndex] += 1;
                            prefabToInstantiate = fourPrefab;
                            break;
                        case 4:
                            // Make FIVE
                            board3D[layerIndex, rowIndex, colIndex] += 1;
                            prefabToInstantiate = fivePrefab;
                            break;
                        default:
                            flag = 0;
                            break;
                    }
                }
                if (flag != 0)
                {
                    //test = 0;
                    var instantiatedPrefab = Instantiate(prefabToInstantiate, prefabPos, Quaternion.identity);
                    Debug.Log((turn / 10) - 1);
                    Color colorPlayer = playerColorArray[(turn / 10) - 1];
                    if (instantiatedPrefab.transform.childCount == 0)
                    {
                        instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
                    }
                    else
                    {
                        foreach (Transform child in instantiatedPrefab.transform)
                        {
                            var childRenderer = child.GetComponent<Renderer>();
                            childRenderer.material.color = colorPlayer;
                        }
                    }
                    if (orbArr[layerIndex, rowIndex, colIndex] != null)
                        orbArr[layerIndex, rowIndex, colIndex].transform.GetComponent<ColorChanger>().killDill();
                    orbArr[layerIndex, rowIndex, colIndex] = instantiatedPrefab;
                    checkThreshold(layerIndex, rowIndex, colIndex, floorY, floorX);
                    movePlayed = 0;
                    ++countFrom;
                    //turn %= maxPlayerCount;   
                }
            }
        }
    }
    void loadMainMenu()
    {
        SceneManager.LoadScene("Home Scene");
    }
    void checkWhoIsDead()
    {
        //print("In checkWhoIsDead, turn="+turn);
        int countAlive = 0, playerNum=0, flag=0;
        for(int pl = 10; pl <= maxPlayerCount; pl+=10)
        {
            flag = 0;
            if (playerStatus[(pl / 10) - 1] == 0)
                continue;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (board3D[i, j, k] - pl >= 1 && board3D[i,j,k] - pl <= 5)
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 1)
                        break;
                }
                if (flag == 1)
                    break;
            }
            if (flag == 0)
                playerStatus[(pl / 10) - 1] = 0;
        }
        for (int i = 0; i < playerCount; i++)
            if (playerStatus[i] == 1)
            {
                ++countAlive;
                playerNum = i + 1;
            }
        int d = 0;
        if (countAlive == 1)
        {
            playerWon = playerNum;
            ApplicationModel.playerWon = playerWon;
            playerWonText.text = "Player : " + playerWon.ToString() + " wins!";
            Camera.main.backgroundColor = Color.black;
        }
        else
        {
            do
            {
                ++d;
                if (d == 10)
                {
                    break;
                }
                turn %= maxPlayerCount;
                turn += 10;
                //print("do while" + turn);
            } while (playerStatus[(turn / 10) - 1] == 0);
            Camera.main.backgroundColor = playerColorArray[(turn / 10) - 1];
        }
        //print("In checkWhoIsDead(EXIT), turn=" + turn);
    }

    public int getTurn()
    {
        return turn;
    }

    static public void checkThreshold(int i,int j, int k, int floorY, int floorX)
    {
        //test++;
        //prevTest++;
        //print("test : "+ test);
        if(board3D[i,j,k]%10 == 2)
        {
            // Animate
            if ((j == 0 && k == 0) || (j == 0 && k == 6) || (j == 7 && k == 0) || (j == 7 && k == 6))
            {
                foreach (Transform child in orbArr[i, j, k].transform)
                {
                    child.GetComponent<Animator>().enabled = true;
                }
                if(playerClicked)
                    audioSource.PlayOneShot(ApplicationModel.unstableSound, 1F);
            }
            else
            {
                if(playerClicked)
                    audioSource.PlayOneShot(ApplicationModel.stableSound, 1F);
            }
            playerClicked = false;   
        }
        else if (board3D[i, j, k]%10 == 3)
        {
            if ((j == 0 && k == 0) || (j == 0 && k == 6) || (j == 7 && k == 0) || (j == 7 && k == 6))
            {
                //orbArr[i, j, k].transform.GetComponent<ColorChanger>().killDill();
                playerClicked = false;
                board3D[i, j, k] = 0;
                audioSource.PlayOneShot(ApplicationModel.bombSound, 1F);
                splitFactory(i, j, k, floorY, floorX);
                orbArr[i, j, k].transform.GetComponent<ColorChanger>().killDill();
                orbArr[i, j, k] = null;
            }
            else if (j == 0 || k == 0 || j == 7 || k == 6)
            {
                // Animate
                foreach (Transform child in orbArr[i, j, k].transform)
                {
                    child.GetComponent<Animator>().enabled = true;
                }
                if(playerClicked)
                    audioSource.PlayOneShot(ApplicationModel.unstableSound, 1F);
            }
            else
            {
                if(playerClicked)
                    audioSource.PlayOneShot(ApplicationModel.stableSound, 1F);
            }
            playerClicked = false;
        }
        else if(board3D[i,j,k]%10 == 4)
        {
            if(j==0 || k==0 || j==7 || k==6)
            {
                //orbArr[i, j, k].transform.GetComponent<ColorChanger>().killDill();
                playerClicked = false;
                board3D[i, j, k] = 0;
                audioSource.PlayOneShot(ApplicationModel.bombSound, 1F);
                splitFactory(i, j, k, floorY, floorX);
                orbArr[i, j, k].transform.GetComponent<ColorChanger>().killDill();
                orbArr[i, j, k] = null;
            }
            else
            {
                // Animate
                foreach (Transform child in orbArr[i, j, k].transform)
                {
                    child.GetComponent<Animator>().enabled = true;
                }
                if(playerClicked)
                    audioSource.PlayOneShot(ApplicationModel.unstableSound, 1F);
            }
            playerClicked = false;
        }
        else if(board3D[i,j,k]%10 == 5)
        {
            //orbArr[i, j, k].transform.GetComponent<ColorChanger>().killDill();
            playerClicked = false;
            board3D[i, j, k] = 0;
            audioSource.PlayOneShot(ApplicationModel.bombSound, 1F);
            splitFactory(i, j, k, floorY, floorX);
            orbArr[i, j, k].transform.GetComponent<ColorChanger>().killDill();
            orbArr[i, j, k] = null;
        }
        else
        {
            if(playerClicked)
                audioSource.PlayOneShot(ApplicationModel.stableSound, 1F);
            playerClicked = false;
        }
        //--test;
    }

    static private void splitFactory(int i, int j, int k, int floorY, int floorX)
    {
        Vector3 prefabPos = new Vector3(floorX + 0.5f, floorY + 0.5f, 0.0f);
        Color colorPlayer = Color.red;
        if (orbArr[i, j, k].transform.childCount == 0)
        {
            colorPlayer = orbArr[i, j, k].transform.GetComponent<Renderer>().material.color;
        }
        else
        {
            foreach (Transform child in orbArr[i, j, k].transform)
            {
                colorPlayer = child.GetComponent<Renderer>().material.color;
            }
        }
        if (j - 1 >= 0)
        {
            var instantiatedPrefab = Instantiate(ApplicationModel.splitCirclePrefab, prefabPos, Quaternion.identity);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
            instantiatedPrefab.transform.GetComponent<SplitCircleController>().SetDestination(new Vector3(floorX + 0.5f, floorY + 1.5f, 0.0f), 0.4f);
            //splitMe(i, j - 1, k, floorY + 1, floorX);
        }
        if (k - 1 >= 0)
        {
            var instantiatedPrefab = Instantiate(ApplicationModel.splitCirclePrefab, prefabPos, Quaternion.identity);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
            instantiatedPrefab.transform.GetComponent<SplitCircleController>().SetDestination(new Vector3(floorX - 0.5f, floorY + 0.5f, 0.0f), 0.4f);
            //splitMe(i, j, k - 1, floorY, floorX - 1);
        }
        if (k + 1 <= 6)
        {
            var instantiatedPrefab = Instantiate(ApplicationModel.splitCirclePrefab, prefabPos, Quaternion.identity);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
            instantiatedPrefab.transform.GetComponent<SplitCircleController>().SetDestination(new Vector3(floorX + 1.5f, floorY + 0.5f, 0.0f), 0.4f);
            //splitMe(i, j, k + 1, floorY, floorX + 1);
        }
        if (j + 1 <= 7)
        {
            var instantiatedPrefab = Instantiate(ApplicationModel.splitCirclePrefab, prefabPos, Quaternion.identity);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
            instantiatedPrefab.transform.GetComponent<SplitCircleController>().SetDestination(new Vector3(floorX + 0.5f, floorY - 0.5f, 0.0f), 0.4f);
            //splitMe(i, j + 1, k, floorY - 1, floorX);
        }
        if (i - 1 == 0)
        {
            var instantiatedPrefab = Instantiate(ApplicationModel.splitCirclePrefab, prefabPos, Quaternion.identity);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
            instantiatedPrefab.transform.GetComponent<SplitCircleController>().SetDestination(new Vector3(floorX - 8.5f, floorY + 0.5f, 0.0f), 0.4f);
            //splitMe(i - 1, j, k, floorY, floorX - 9);
        }
        if (i + 1 == 1)
        {
            var instantiatedPrefab = Instantiate(ApplicationModel.splitCirclePrefab, prefabPos, Quaternion.identity);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            instantiatedPrefab.transform.GetComponent<Renderer>().material.color = colorPlayer;
            instantiatedPrefab.transform.GetComponent<SplitCircleController>().SetDestination(new Vector3(floorX + 9.5f, floorY + 0.5f, 0.0f), 0.4f);
            //splitMe(i + 1, j, k, floorY, floorX + 9);
        }        
    }
    
}
