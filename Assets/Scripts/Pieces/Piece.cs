using System;
using System.Collections.Generic;
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

	//Actually start functioning
	public virtual void Place(Vector2Int position) {
		enabled = true;
		this.position = position;
		GridManager.instance[position] = this;
	}

	public virtual void GetHit(int direction) => Health -= hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived;

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

	ICollection<Piece> GetDisconnectedPieces() {
		Dictionary<Vector2Int, Piece> originalTiles = GridManager.instance.tiles;
		Dictionary<Vector2Int, Piece> disconnectedPieces = new Dictionary<Vector2Int, Piece>(originalTiles);

		HashSet<Vector2Int> processed = new HashSet<Vector2Int> { Vector2Int.zero };
		Stack<Vector2Int> toBeProcessed = new Stack<Vector2Int>();
		toBeProcessed.Push(Vector2Int.zero);
		disconnectedPieces.Remove(Vector2Int.zero);

		while (toBeProcessed.Count > 0) {
			Vector2Int processing = toBeProcessed.Pop();
			Piece procPiece = originalTiles[processing];

			for (int dir = 0; dir < 4; dir++) {
				Vector2Int adjacent = processing + directions[dir];
				if (!processed.Contains(adjacent) && originalTiles.TryGetValue(adjacent, out Piece adjPiece) && adjacent != position && procPiece.hasConnector[dir] && adjPiece.hasConnector[(dir + 2) % 4]) {
					toBeProcessed.Push(adjacent);
					processed.Add(adjacent);
					disconnectedPieces.Remove(adjacent);
				}
			}
		}

		return disconnectedPieces.Values;
	}

	void Destroy() {
		//Disable pieces and find debris center
		ICollection<Piece> disconnectedPieces = GetDisconnectedPieces();
		Vector2Int center = Vector2Int.zero;
		foreach (Piece disconnectedPiece in disconnectedPieces) {
			center += disconnectedPiece.position;
			disconnectedPiece.Disable();
		}

		//Parent stuff to debris
		Debris debris = Instantiate(GridManager.instance.debris, new Vector3((float)center.x / disconnectedPieces.Count, 0, (float)center.y / disconnectedPieces.Count), Quaternion.identity);
		foreach (Piece disconnectedPiece in disconnectedPieces) {
			disconnectedPiece.transform.parent = debris.transform;
		}
	}

	protected virtual void Disable() {
		GridManager.instance.RemovePiece(position);
		enabled = false;
	}
}
