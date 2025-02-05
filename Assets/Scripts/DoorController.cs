using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractionController
{
    private bool open;
    private GameObject doorObject;
    private Collider doorCollider;
    private Quaternion doorOpenRotation = Quaternion.Euler(0, 90, 0);
    private Quaternion doorClosedRotation = Quaternion.Euler(0, 0, 0);
    private float slerpSpeed = 5f;

    public bool Open
    {
        get => open; set
        {
            if(open != value)
            {
                open = value;
                handleDoorOpenChanged();
            }
        }
    }

    public override void OnInteract()
    {
        Open = !open;
    }

    void handleDoorOpenChanged()
    {
        doorCollider.enabled = !open;
        if (open)
        {
            Debug.Log("Door Opened");
            Text = "Close Door";
        }
        else
        {
            Debug.Log("Door Closed");
            Text = "Open Door";
        }
    }

    void Start()
    {
        doorObject = transform.Find("Door").gameObject;
        if (doorObject)
        {
            doorCollider = doorObject.GetComponent<Collider>();
        }
        else
        {
            Debug.LogWarning("Door Object not found");
        }
        Open = false;
        Text = "Open Door";
    }

    void Update()
    {
        if (open)
            doorObject.transform.rotation = Quaternion.Slerp(doorObject.transform.rotation, doorOpenRotation, Time.deltaTime * slerpSpeed);
        else
            doorObject.transform.rotation = Quaternion.Slerp(doorObject.transform.rotation, doorClosedRotation, Time.deltaTime * slerpSpeed);
    }
}
