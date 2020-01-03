using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject trackedObject;
    public float cameraPanSpeed = 1f;

    void Update()
    {
        Vector2 myPos = (Vector2)transform.position;
        Vector2 trackedObjectPos = (Vector2)trackedObject.transform.position;
        if(Vector2.Distance(myPos, trackedObjectPos) > 0.01){
            Vector2 lerped = Vector2.Lerp(myPos, trackedObjectPos, Time.deltaTime * cameraPanSpeed);
            transform.position = new Vector3(lerped.x, lerped.y, transform.position.z); 
        }
    }
}
