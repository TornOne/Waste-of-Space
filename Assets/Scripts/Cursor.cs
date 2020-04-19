using UnityEngine;

public class Cursor : MonoBehaviour {
	public static Cursor instance;

	public GameObject[] mismatchedConnectors;
	public GameObject regularCursor;
	public GameObject invalidCursor;

	void Awake() => instance = this;

	public void UpdateCursor(Vector2Int location, bool isValid, bool[] invalidConnectors) {
		transform.position = new Vector3(location.x, 0, location.y);

		regularCursor.SetActive(isValid);
		invalidCursor.SetActive(!isValid);
		for (int dir = 0; dir < 4; dir++) {
			mismatchedConnectors[dir].SetActive(invalidConnectors[dir]);
		}
	}
}
