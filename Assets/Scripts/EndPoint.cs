using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndPoint : MonoBehaviour
{
	public UnityEvent OnEndReached = new UnityEvent();

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")){
			OnEndReached.Invoke();
		}
	}
}
