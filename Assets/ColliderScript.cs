using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
	[SerializeField]
	[Range(0, 180)]
	float wallFloorSeparationThreshold = 45f;
	[SerializeField]
	float collisionSpeedThreshold = 0.1f;
	[SerializeField]
	ParticleSystem groundParticles;

    public AudioClip hitSound;

	void OnCollisionEnter(Collision collision) {
		ContactPoint[] cps = new ContactPoint[collision.contactCount];
		collision.GetContacts(cps);
		foreach(ContactPoint cp in cps){
			float angle = Vector3.Angle(cp.point - transform.position, Vector3.down);
			if(angle < wallFloorSeparationThreshold && collision.impulse.sqrMagnitude > collisionSpeedThreshold){
				groundParticles.Play();
                AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, Globals.fxVolume);
			}
		}
	}
}
