using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{


    private string text;

    public string Text { get => text; set => text = value; }

    public override string ToString()
    {
        return Text;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Player got in contact with " + this);
            //Debug.Log("Add interaction: " + it);
            GameController.getInstance().addInteractable(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Player no longer in contact with " + this);
            //Debug.Log("Remove interaction: " + it);
            GameController.getInstance().removeInteractable(this);
        }
    }

    public abstract void OnInteract();
}
