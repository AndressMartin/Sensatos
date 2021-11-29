using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public void FollowHook(GameObject target)
    {
        Vector2 rodPos = transform.position;
        Vector2 targetPos = target.gameObject.transform.position;
        Vector2 direction = targetPos - rodPos;
        transform.right = direction;
    }
}
