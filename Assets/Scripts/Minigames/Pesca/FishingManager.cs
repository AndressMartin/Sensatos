using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingManager : SingletonInstance<FishingManager>
{
    public enum FishingStates {Idle, Fishing, ShootingHook}
    public FishingStates fishingStates = FishingStates.Idle;
    public Hook hook;
    public FishingRod fishingRod;
    public Transform hookTarget;
    public Slider fishSlider;
    public FishingSpot fishingSpot;
    public GameObject fishingObject;
    public GameObject fishingUI;
    public MinigameTrigger minigameTrigger;
    private Vector2 targetPos;
    //Parabole Animation
    protected float animationTime;

    //Points
    private int points;
    private float pointDecrease;
    //Difficulty
    public float pointStarterDecrease = 0.3f;
    //Spot movement
    public Vector3 vectorMagnitude;
    public float multiplier;
    public Vector3 originalSpotPosition;
    private float spotChangeTime;
    public float spotChangeTimeStarter = 1.8f;
    //Camera

    void Start()
    {
        if (hook.hookCollided != null)
        {
            hook.hookCollided.RemoveAllListeners();
            hook.hookCollided.AddListener(ManageHookCollisions);
        }

    }

    void Update()
    {
        fishingRod.FollowHook(hook.gameObject);
        if (Input.GetKeyDown(KeyCode.E) && fishingStates == FishingStates.Idle)
        {
            ShootHook();
            //PlayerMovement.GetInstance().canMove = false;
            points += (int)fishSlider.value;
            fishingObject.SetActive(true);
        }
        if (fishingStates == FishingStates.ShootingHook)
        {
            //Debug.Log($"{animationTime}, multiplied = {animationTime * .5f}");
            animationTime += Time.deltaTime;
            animationTime = animationTime % 5f;
            hook.gameObject.transform.position = MathParabola.Parabola(fishingRod.transform.position, hookTarget.position, 3f, animationTime * .5f);
        }
        if (fishingStates == FishingStates.Fishing)
        {
            fishSlider.value = points;
            if(fishSlider.value >= fishSlider.maxValue)
            {
                EndFishingGame();
            }
            hook.MoveHook();
            DecreasePoints();
            MoveSpot();
        }
    }

    private void EndFishingGame()
    {
        fishingStates = FishingStates.Idle;
        //PlayerMovement.GetInstance().canMove = true;
        Debug.Log("Congratulations! You got a generic fish :(");
        fishingObject.SetActive(false);
        fishingUI.SetActive(false);
        minigameTrigger.canShowPopUp = true;
        fishingSpot.transform.position = originalSpotPosition;
    }

    void ShootHook()
    {
        fishingStates = FishingStates.ShootingHook;
    }

    void StopHook()
    {
        fishingStates = FishingStates.Fishing;
    }
    void ManageHookCollisions()
    {
        if (fishingStates == FishingStates.ShootingHook)
        {
            StopHook();
            fishingUI.SetActive(true);
            minigameTrigger.canShowPopUp = false;
        }
        if (fishingStates == FishingStates.Fishing)
        {
            Fish();
        }
    }
    private void Fish()
    {
        points++;
        Debug.Log("On fish! " + points);

    }

    private void DecreasePoints()
    {
        pointDecrease -= Time.deltaTime;
        spotChangeTime -= Time.deltaTime;
        if (pointDecrease <= 0f)
        {
            if (hook.collidedWithFish == false && points>= 0) points--;
            pointDecrease = pointStarterDecrease;
        }
        if (spotChangeTime <= 0f)
        {
            SetDestination();
            spotChangeTime = spotChangeTimeStarter;
        }

    }

    private void MoveSpot()
    {       
        fishingSpot.transform.position = fishingSpot.transform.position + (vectorMagnitude * multiplier * Time.deltaTime);
    }

    private bool insideBounds()
    {
        return fishingSpot.inBounds;
    }

    private void SetDestination()
    {
        if (insideBounds()) 
        {
            vectorMagnitude = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized;
            multiplier = UnityEngine.Random.Range(1.2f, 1.5f);
        }
        else
        {
            vectorMagnitude = (originalSpotPosition - fishingSpot.transform.position).normalized;
            multiplier = UnityEngine.Random.Range(1.5f, 2f);
        }
    }
}
