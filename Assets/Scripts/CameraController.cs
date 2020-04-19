using UnityEngine;

public class CameraController : MonoBehaviour {
	Camera cam;
	public float moveSpeed, zoomSpeed;

	void Awake() => cam = GetComponent<Camera>();

	void Update() {
		Vector3 totalTranslate = new Vector3(1, 0, 1) * Input.GetAxisRaw("Horizontal") + new Vector3(-1, 0, 1) * Input.GetAxisRaw("Vertical");
		transform.position += totalTranslate.normalized * moveSpeed * Time.deltaTime;

		cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.GetAxisRaw("Zoom") * zoomSpeed * Time.deltaTime, 1, 20);
	}
}
