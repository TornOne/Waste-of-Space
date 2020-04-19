﻿using System;
using UnityEngine;

public class Piece : MonoBehaviour {
	Vector2Int position;
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

	public virtual void GetHit(int direction) => Health -= hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived;

	//Actually start functioning
	public virtual void Place(Vector2Int position) {
		this.position = position;
		//TODO: Actually start functioning
	}

	void Destroy() {
		//TODO: Remove piece and everything no longer connected to the core and make appropriate calculative adjustments
	}
}
