using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class DoorScriptYPos : Item
{

    //Public Variables 
    public GameObject door;
    public float openOffset = 2f;
    public float openDuration = 2f;
    public float slideSpeed = 3f;
    
    //Private Variables
    private Vector3 closedPosition;
    private bool isOpening = false;
    
    void Start()
    {
        door.SetActive(true);
        closedPosition = door.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && (!isOpening))
        {
            Debug.Log("Coroutine called");
            StartCoroutine(DoorOpening());
        }
    }

    IEnumerator DoorOpening()
    {
        isOpening = true;
        Vector3 openPosition = closedPosition + new Vector3(0f, openOffset, 0f);
        
        
        door.transform.position = openPosition;

        yield return new WaitForSeconds(openDuration);
        door.transform.position = closedPosition;
        isOpening = false;
       
    }
}


