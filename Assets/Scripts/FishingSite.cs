using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSite : MonoBehaviour
{
    public float spotDistance;
    private CircleCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        coll.radius = spotDistance;
    }
}
