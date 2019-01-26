using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;
    public float distance, speed, normalHeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        Vector2 pPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 cPos = new Vector2(transform.position.x, transform.position.z);
        //transform.LookAt(player.transform);
        float offsetHeight = player.moveDirection.z;
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, player.transform.position.y + normalHeight - offsetHeight, transform.position.z), Time.deltaTime * speed);
        
        
        //transform.position = player.transform.position - (transform.forward * distance);

        transform.localEulerAngles = new Vector3(45-Input.GetAxis("Vertical")*10, transform.localEulerAngles.y, Input.GetAxis("Horizontal") * 10);
        transform.position = player.transform.position - (Vector3.forward * distance) + new Vector3(0, normalHeight, 0);// * Input.GetAxis("Vertical") 

        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
    }
}
