﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public PlayerCamera cam;
    public float speed = 3;

    [HideInInspector]
    public Vector3 moveDirection;

	[HideInInspector]
    public Rigidbody rb;

	public UnityEvent OnEndPointTrigger = new UnityEvent();

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

        Transform camClone = cam.transform;
        camClone.localEulerAngles = new Vector3(0, camClone.localEulerAngles.y, 0);
        rb.velocity += camClone.TransformDirection(moveDirection);
    }

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("End Point")){
			OnEndPointTrigger.Invoke();
		}
	}
}
