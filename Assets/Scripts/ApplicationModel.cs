using UnityEngine;

public class ApplicationModel {

    static public GameObject onePrefab, twoPrefab, threePrefab, fourPrefab, fivePrefab, splitCirclePrefab;
    static public int playerCount = 0;
    static public int playerWon = 0;
    static public int[,,] board3D = new int[2, 8, 7];
    static public GameObject[,,] orbArr = new GameObject[2, 8, 7];
    static int frameCounter=0;
    static public AudioClip stableSound, unstableSound, bombSound;
}
