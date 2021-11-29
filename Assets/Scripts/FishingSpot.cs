using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    public bool inBounds = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FishingSite")
        {
            Debug.Log("Enter");
            inBounds = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "FishingSite")
        {
            Debug.Log("Leave");
            inBounds = false;
        }
    }
}
