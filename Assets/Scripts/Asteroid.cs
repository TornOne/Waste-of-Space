using System;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public Vector3 velocity;
	public Vector3 spinVelocity;
	public int destroyBufferWidth = 10;
	int xMin, xMax, yMin, yMax;
	Vector2Int lastPos = new Vector2Int(int.MaxValue, int.MaxValue);

	void Start() {
		xMin = GridManager.instance.bounds.xMin - destroyBufferWidth;
		yMin = GridManager.instance.bounds.yMin - destroyBufferWidth;
		xMax = GridManager.instance.bounds.xMax + destroyBufferWidth;
		yMax = GridManager.instance.bounds.yMax + destroyBufferWidth;
	}

	void Update() {
		//Move
		transform.position += velocity * Time.deltaTime;
		transform.Rotate(spinVelocity * Time.deltaTime);
		float x = transform.position.x;
		float y = transform.position.z;
		Vector2Int newPos = new Vector2Int(Convert.ToInt32(x), Convert.ToInt32(y));

		if (newPos != lastPos) {
			//Maybe destroy self
			if (x < xMin || x > xMax || y < yMin || y > yMax) {
				AsteroidSpawner.instance.RemoveAsteroid(this);
				Destroy(gameObject);
			}

			//Maybe hit a piece
			Piece piece = GridManager.instance[newPos];
			if (piece != null) {
				Vector2Int hitDir = lastPos - newPos;
				int dir = Piece.DirectionIndex(hitDir);
				if (dir == -1) {
					Vector3 moveDelta = -velocity * Time.deltaTime;
					dir = Piece.GetDirection(moveDelta.x, moveDelta.z);
				}
				Debug.Log($"Hit {piece} from {dir}");
				piece.GetHit(dir);
				Destroy(gameObject);
			}

			//Finally update position
			lastPos = newPos;
		}
	}
}
