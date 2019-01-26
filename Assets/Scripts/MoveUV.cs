using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUV : MonoBehaviour
{
    public float xSpeed = 1, ySpeed = 1;
    // Start is called before the first frame update

    Vector2 offset;

    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += xSpeed * Time.deltaTime;
        offset.y += ySpeed * Time.deltaTime;
        foreach(Material mat in rend.materials)
        {
            mat.SetTextureOffset("_MainTex", offset);
        }
    }
}
