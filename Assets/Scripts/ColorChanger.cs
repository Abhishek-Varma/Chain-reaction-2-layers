using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public GameObject screenBoard;
    private int prevTurn, currTurn;
    private Vector3 currPos, target;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void killDill()
    {
        Destroy(gameObject);
    }
}
