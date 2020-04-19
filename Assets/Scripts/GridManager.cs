﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	Camera cam;
	public static GridManager instance;

	readonly Dictionary<Vector2Int, Piece> tiles = new Dictionary<Vector2Int, Piece>();

	float lastTilePlacedTime = 0;
	public float tilePlaceDelay = 1;
	Piece tileHeld;

	public Piece this[int x, int y] {
		get => this[new Vector2Int(x, y)];
		set => this[new Vector2Int(x, y)] = value;
	}

	public Piece this[Vector2Int vec] {
		get => tiles.TryGetValue(vec, out Piece value) ? value : null;
		set => tiles[vec] = value;
	}

	void Awake() => instance = this;

	void Start() => cam = Camera.main;

	void Update() {
		if (tileHeld is null && Time.time - lastTilePlacedTime > tilePlaceDelay) {
			tileHeld = PieceSpawner.instance.GetRandomPiece();
		}

		if (tileHeld != null) {
			Vector2Int cursorPos = GetTileFromCursor();
			tileHeld.transform.position = new Vector3(cursorPos.x, 0, cursorPos.y);

			//Handle rotations
			if (Input.GetAxisRaw("Rotation") > 0.5f || Input.GetAxisRaw("ScrollWheel") > 0.5f) {
				tileHeld.Rotate(clockwise: true);
			} else if (Input.GetAxisRaw("Rotation") < -0.5f || Input.GetAxisRaw("ScrollWheel") < -0.5f) {
				tileHeld.Rotate(clockwise: false);
			}

			//Handle placement and discarding
			if (Input.GetMouseButtonDown(0) && IsValidPlacement(tileHeld, cursorPos)) {
				tileHeld.Place(cursorPos);
				lastTilePlacedTime = Time.time;
				tileHeld = null;
			} else if (Input.GetMouseButtonDown(1)) {
				Destroy(tileHeld.gameObject);
				tileHeld = null;
			}
		}

		Debug.Log(GetTileFromCursor());
	}

	bool IsValidPlacement(Piece piece, Vector2Int tile) {
		//Can't be occupied
		if (this[tile] != null) {
			return false;
		}

		//Connectors must match
		bool hasValidConnector = false;
		for (int dir = 0; dir < 4; dir++) {
			Piece adjacent = tiles[tile + Piece.directions[dir]];
			if (adjacent is null) {
				continue;
			} else if (adjacent.hasConnector[(dir + 2) % 4] == piece.hasConnector[dir]) { //Both connectors either are or aren't
				if (piece.hasConnector[dir]) { //Both connectors are
					hasValidConnector = true;
				}
			} else {
				return false;
			}
		}

		return hasValidConnector;
	}

	public Vector2Int GetTileFromCursor() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 tileLoc = ray.GetPoint(ray.origin.y / -ray.direction.y);
		Debug.Log(tileLoc);
		return new Vector2Int(Convert.ToInt32(tileLoc.x), Convert.ToInt32(tileLoc.z));
	}
}
