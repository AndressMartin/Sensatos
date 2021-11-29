using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hook : MonoBehaviour
{
    public UnityEvent hookCollided;
    public FishingManager fishingManager;

    public float collisionTick;
    private float collisionStarterTick = .15f;
    public bool collidedWithFish;
    //Hook Movement
    float horizontal;
    float vertical;
    float hookSpeed = 3f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        fishingManager = FishingManager.GetInstance();
        rb = GetComponent<Rigidbody2D>();
        if (hookCollided == null)
        {
            hookCollided = new UnityEvent();
        }
        collisionTick = collisionStarterTick;
    }

    // Update is called once per frame
    void Update()
    {
        if (collidedWithFish)
        {
            if (collisionTick > 0f)
            {
                collisionTick -= Time.deltaTime;
            }
            else
            {
                collidedWithFish = false;
            }
        }
    }
    public void MoveHook()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        rb.velocity = new Vector2(horizontal, vertical).normalized * (hookSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FishingSpot" && fishingManager.fishingStates == FishingManager.FishingStates.ShootingHook)
        {
            Debug.Log("colidiu corretamente");
            transform.position = collision.transform.position;
            hookCollided.Invoke();
            fishingManager.fishingSpot = collision.transform.GetComponent<FishingSpot>();
            fishingManager.fishingSpot.inBounds = true;
            fishingManager.originalSpotPosition = collision.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "FishingSpot" && fishingManager.fishingStates == FishingManager.FishingStates.Fishing && collidedWithFish == false)
        {
            Debug.Log("Colisao peixe");
            collidedWithFish = true;
            collisionTick = collisionStarterTick;
            hookCollided.Invoke();
        }
    }
}
