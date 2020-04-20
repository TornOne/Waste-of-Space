using UnityEngine;

public class LightController : MonoBehaviour {
	public float revolutionDuration;
	float spinSpeed;

	void Start() => spinSpeed = 1 / revolutionDuration;

	void Update() => transform.localEulerAngles += new Vector3(0, spinSpeed);
}
