using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCircleController : MonoBehaviour {


    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;
    void Start()
    {
        //target = new Vector3(-99, -99, -99);
        startPosition = transform.position;
    }
    void Update()
    {
        print(transform.position+" : "+target);
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
        
        if(transform.position == target)
        {
            // Check what can be instantiated
            int floorX = (int)(transform.position.x - 0.5f);
            int floorY = (int)(transform.position.y - 0.5f);
            Vector3 prefabPos = transform.position;
            int colIndex = floorX + 8;
            int rowIndex = Mathf.Abs(floorY - 3);
            int layerIndex = 0;
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
            GameObject prefabToInstantiate = ApplicationModel.onePrefab;
            //print("Was there : "+"X="+floorX + "Y=" + floorY);
            if (GameController.board3D[layerIndex, rowIndex, colIndex] == 0) // checking if player clicked on an empty cell
            {
                GameController.board3D[layerIndex, rowIndex, colIndex] = GameController.turn + 1;
                prefabToInstantiate = ApplicationModel.onePrefab;
            }
            else
            {
                int newNumberOfOrbs = (GameController.board3D[layerIndex, rowIndex, colIndex]) % 10 + 1;
                GameController.board3D[layerIndex, rowIndex, colIndex] = GameController.turn + newNumberOfOrbs;
                switch (newNumberOfOrbs)
                {
                    case 1:
                        // Make ONE
                        prefabToInstantiate = ApplicationModel.onePrefab;
                        break;
                    case 2:
                        // Make TWO
                        prefabToInstantiate = ApplicationModel.twoPrefab;
                        break;
                    case 3:
                        // Make THREE
                        prefabToInstantiate = ApplicationModel.threePrefab;
                        break;
                    case 4:
                        // Make Four
                        prefabToInstantiate = ApplicationModel.fourPrefab;
                        break;
                    case 5:
                        // Make Four
                        prefabToInstantiate = ApplicationModel.fivePrefab;
                        break;
                }
            }
            var instantiatedPrefab = Instantiate(prefabToInstantiate, prefabPos, Quaternion.identity);
            
            //Debug.Log((turn / 10) - 1);
            //Color colorPlayer = playerColorArray[(turn / 10) - 1];
            if (instantiatedPrefab.transform.childCount == 0)
            {
                instantiatedPrefab.transform.GetComponent<Renderer>().material.color = transform.GetComponent<Renderer>().material.color;
            }
            else
            {
                foreach (Transform child in instantiatedPrefab.transform)
                {
                    var childRenderer = child.GetComponent<Renderer>();
                    childRenderer.material.color = transform.GetComponent<Renderer>().material.color;
                }
            }
            if (GameController.orbArr[layerIndex, rowIndex, colIndex] != null)
                GameController.orbArr[layerIndex, rowIndex, colIndex].transform.GetComponent<ColorChanger>().killDill();
            GameController.orbArr[layerIndex, rowIndex, colIndex] = instantiatedPrefab;
            //print("Check at: " + layerIndex + ", " + rowIndex + ", " + colIndex);
            GameController.checkThreshold(layerIndex, rowIndex, colIndex, floorY, floorX);            
            killDill();
        }
    }

    public void killDill()
    {
        Destroy(gameObject);
    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = 0.3f;
        target = destination;
        print("lala: " + startPosition + ", " + target);
    }
}
