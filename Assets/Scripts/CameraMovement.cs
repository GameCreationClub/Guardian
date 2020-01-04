using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform trackedObject;
    public float cameraPanSpeed = 1f;

    private void Update()
    {
        if (trackedObject != null)
        {
            Vector2 myPos = transform.position;
            Vector2 trackedObjectPos = trackedObject.position;

            if (Vector2.Distance(myPos, trackedObjectPos) > 0.01f)
            {
                Vector2 lerped = Vector2.Lerp(myPos, trackedObjectPos, Time.deltaTime * cameraPanSpeed);
                transform.position = new Vector3(lerped.x, lerped.y, transform.position.z);
            }
        }
    }

    public void GoToPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
