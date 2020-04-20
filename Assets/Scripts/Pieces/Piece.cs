using System;
using UnityEngine;

public class Piece : MonoBehaviour {
	protected Vector2Int position;
	[SerializeField]
	int _health = 2;
	public int Health {
		get => _health;
		set {
			_health = value;
			if (_health <= 0) {
				Destroy();
			}
		}
	}
	public int connectorDamageReceived = 2;
	public int deadEndDamageReceived = 1;
	public bool[] hasConnector; //up, right, down, left

	public static readonly Vector2Int[] directions = new Vector2Int[4] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
	public static int DirectionIndex(Vector2Int direction) => Array.IndexOf(directions, direction);
	public static int GetDirection(float deltaX, float deltaY) {
		float angle = Mathf.Atan2(deltaY, deltaX);
		if (angle < Mathf.PI * -0.75f) {
			return 3;
		} else if (angle < Mathf.PI * -0.25f) {
			return 2;
		} else if (angle < Mathf.PI * 0.25f) {
			return 1;
		} else if (angle < Mathf.PI * 0.75f) {
			return 0;
		} else {
			return 3;
		}
	}

	public virtual void GetHit(int direction) => Health -= hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived;

	//Actually start functioning
	public virtual void Place(Vector2Int position) {
		this.position = position;
		GridManager.instance[position] = this;
		//TODO: Actually start functioning
	}

	public virtual void Rotate(bool clockwise) {
		transform.Rotate(Vector3.up, clockwise ? 90 : -90);
		RotateSlots(hasConnector, clockwise);
	}

	protected void RotateSlots(bool[] slots, bool clockwise) {
		if (clockwise) {
			bool temp = slots[3];
			for (int dir = 3; dir > 0; dir--) {
				slots[dir] = slots[dir - 1];
			}
			slots[0] = temp;
		} else {
			bool temp = slots[0];
			for (int dir = 0; dir < 3; dir++) {
				slots[dir] = slots[dir + 1];
			}
			slots[3] = temp;
		}
	}

	void Destroy() {
		GridManager.instance.RemovePiece(position);
		Destroy(gameObject); //TODO: Destruction animations, falling down, followed by random spin and scale lerp to 0
		//TODO: Remove piece and everything no longer connected to the core and make appropriate calculative adjustments
	}
}
