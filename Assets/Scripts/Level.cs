using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

	[SerializeField]
	float icarusDistance = 150f;
	[SerializeField]
	float icarusSpeed = 20f;
	bool icarusTransition = false;
	Scene icarusScene;
	float icarusProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
		goalPoint.GetComponent<EndPoint>().OnEndReached.AddListener(EndReached);
    }

    // Update is called once per frame
    void Update()
    {
		if(player != null && player.transform.position.y < deathHeight){
			Respawn();
		}
		// shoot level to be unloaded up to the sky
		if(icarusTransition) {
			float moveAmount = icarusSpeed * Time.deltaTime;
			if(icarusProgress + moveAmount >= icarusDistance) { // if at our destination, do cleanup of transition
				Debug.Log("Unloading scene " + icarusScene.name);
				SceneManager.UnloadSceneAsync(icarusScene);
				icarusTransition = false;
			}
			icarusProgress += moveAmount;
			Vector3 movement = Vector3.up * moveAmount;

			// move both levels up by the amount required each update step
			foreach(GameObject g in icarusScene.GetRootGameObjects()) {
				g.transform.position += movement;
			}
		}
    }

	public void AlignTo(Transform hole){
		Vector3 euler = transform.eulerAngles - startPoint.transform.eulerAngles;
		transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z);
		transform.position = new Vector3(hole.position.x, transform.position.y, hole.position.z) - startPoint.transform.position;
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

	public void Respawn(bool reposition=true){
		if(reposition) {
			player.transform.position = startPoint.transform.position;
		}
		player.rb.velocity = Vector3.zero;
		player.rb.angularVelocity = Vector3.zero;
	}

	public void EndReached(){
		OnEndReached.Invoke();
	}

	public void StartIcarus(Scene scene){
		icarusScene = scene;
		icarusProgress = 0;
		icarusTransition = true;
	}
}
