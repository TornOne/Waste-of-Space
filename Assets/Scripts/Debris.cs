using System.Collections;
using UnityEngine;

public class Debris : MonoBehaviour {
	public float descendSpeed;
	public float spinSpeed;
	public float shrinkTime;

	void Start() => StartCoroutine(Descend());

	IEnumerator Descend() {
		while (transform.position.y > -1) {
			transform.position -= new Vector3(0, descendSpeed * Time.deltaTime);
			yield return null;
		}
		StartCoroutine(SpinAndShrink(Time.time));
		while (true) {
			transform.position -= new Vector3(0, descendSpeed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator SpinAndShrink(float startTime) {
		Vector3 spin = Random.onUnitSphere * spinSpeed;
		float t = 0;
		while (t < 1) {
			t = (Time.time - startTime) / shrinkTime;
			transform.localScale = Vector3.one * Mathf.Lerp(1, 0, t);
			transform.Rotate(spin * Time.deltaTime);
			yield return null;
		}

		Destroy(gameObject);
	}
}
