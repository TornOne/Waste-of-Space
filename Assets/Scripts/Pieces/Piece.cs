using System;
using UnityEngine;

public class Piece : MonoBehaviour {
	Vector2Int position;
	int _health;
	public int Health {
		get => _health;
		set {
			_health = value;
			if (_health <= 0) {
				Destroy();
			}
		}
	}
	public int damageReceived = 1;
	public bool[] hasConnector; //up, right, down, left

	static readonly Vector2Int[] directions = new Vector2Int[4] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
	public static int DirectionIndex(Vector2Int direction) => Array.IndexOf(directions, direction);

	public virtual void GetHit() => Health -= damageReceived;

	void Destroy() {
		//TODO: Remove piece and everything no longer connected to the core and make appropriate calculative adjustments
	}
}
