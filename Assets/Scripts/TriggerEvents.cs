using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class TriggerEvent : UnityEvent<Collider> { }

public class TriggerEvents : MonoBehaviour {
	public TriggerEvent TriggerEnter = new TriggerEvent();
	public TriggerEvent TriggerStay = new TriggerEvent();
	public TriggerEvent TriggerExit = new TriggerEvent();

	void OnTriggerEnter(Collider collider) { TriggerEnter.Invoke(collider); }
	void OnTriggerStay(Collider collider) { TriggerStay.Invoke(collider); }
	void OnTriggerExit(Collider collider) { TriggerExit.Invoke(collider); }
}
