using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class CollisionEvent : UnityEvent<Collision> { }

public class CollisionEvents : MonoBehaviour {
	public CollisionEvent CollisionEnter = new CollisionEvent();
	public CollisionEvent CollisionStay = new CollisionEvent();
	public CollisionEvent CollisionExit = new CollisionEvent();

	void OnCollisionEnter(Collision collision) { CollisionEnter.Invoke(collision); }
	void OnCollisionStay(Collision collision) { CollisionStay.Invoke(collision); }
	void OnCollisionExit(Collision collision) { CollisionExit.Invoke(collision); }
}
