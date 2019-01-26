using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField]
	GameObject startPoint;
	[SerializeField]
	GameObject goalPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void AlignTo(Transform hole){
		transform.position = new Vector3(hole.position.x, transform.position.y, hole.position.z);
		Vector3 euler = transform.eulerAngles;
		transform.rotation = Quaternion.Euler(euler.x, hole.eulerAngles.y, euler.z);
	}
	public void Move(Vector3 point){
		transform.position += point;
	}
	public Transform GetGoalPoint(){
		return goalPoint.transform;
	}
}
