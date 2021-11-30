using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    public float triggerMinigameDistance;
    private CircleCollider2D minigameTriggerColl;
    public GameObject popUp;
    public bool canShowPopUp = true;
    // Start is called before the first frame update
    void Start()
    {
        popUp.SetActive(false);
        minigameTriggerColl = transform.GetComponent<CircleCollider2D>();
        minigameTriggerColl.radius = triggerMinigameDistance;
    }
    private void Update()
    {
        if (canShowPopUp == false) popUp.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (canShowPopUp) popUp.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            popUp.SetActive(false);
        }
    }
}
