using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
	[SerializeField]
	GameObject startPoint;
	[SerializeField]
	GameObject goalPoint;

	[SerializeField]
	float deathHeight = -100f;
	[HideInInspector]
	public Player player;

	public UnityEvent OnEndReached = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
		goalPoint.GetComponent<EndPoint>().OnEndReached.AddListener(EndReached);
    }

    // Update is called once per frame
    void Update()
    {
		if(player.transform.position.y < deathHeight){
			Respawn();
		}
    }

	public void AlignTo(Transform hole){
		transform.position = new Vector3(hole.position.x, transform.position.y, hole.position.z)-startPoint.transform.position;
		Vector3 euler = transform.eulerAngles;
		transform.rotation = Quaternion.Euler(euler.x, hole.eulerAngles.y, euler.z);
	}
	public void Move(Vector3 point){
		transform.position += point;
	}
	public Transform GetGoalPoint(){
		return goalPoint.transform;
	}
	public Transform GetStartPoint(){
		return startPoint.transform;
	}

	public void Respawn(){
		player.transform.position = startPoint.transform.position;
	}

	public void EndReached(){
		OnEndReached.Invoke();
	}
}
