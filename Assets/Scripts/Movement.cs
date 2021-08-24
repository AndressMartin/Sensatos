using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    float horizontal;
    float vertical;
    public float runSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        Move();

    }
    void Move()
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * runSpeed;
    }




   public void UpdateRunSpeed(int velocidade)
    {
        runSpeed = velocidade * 2;
    }
}


