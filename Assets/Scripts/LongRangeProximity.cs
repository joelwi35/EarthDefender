using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeProximity : MonoBehaviour {

    private GameController gameController;

    // Use this for initialization
    void Start () {

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find game controller script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Long Range Sensor triggered by " + other.tag);
        if (other.CompareTag("MotherShip"))
        {
            Debug.Log("Show mother ship alert!");
            gameController.ShowMotherShipProximityAlert();
        }        
    }
}
