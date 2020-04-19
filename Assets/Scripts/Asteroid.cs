using UnityEngine;

public class Asteroid : MonoBehaviour {
	public Vector3 velocity;
	public Vector3 spinVelocity;

	void Update() {
		transform.position += velocity * Time.deltaTime;
		transform.Rotate(spinVelocity * Time.deltaTime);
	}
}
