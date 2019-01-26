using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerCamera cam;
    public float speed = 3;

    [HideInInspector]
    public Vector3 moveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxis("Horizontal") * speed;
        moveDirection.z = Input.GetAxis("Vertical")* speed;

        rb.velocity += moveDirection;// cam.transform.TransformDirection(moveDirection);
    }
}
