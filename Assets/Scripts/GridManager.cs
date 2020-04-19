using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	Camera cam;
	public static GridManager instance;

	readonly Dictionary<Vector2Int, Piece> _tiles = new Dictionary<Vector2Int, Piece>();

	float lastTilePlacedTime = 0;
	public float tilePlaceDelay = 1;
	Piece tileHeld;
	Vector2Int lastCursorPos;
	bool isValidPlacement;

	public Piece this[int x, int y] {
		get => this[new Vector2Int(x, y)];
		set => this[new Vector2Int(x, y)] = value;
	}

	public Piece this[Vector2Int vec] {
		get => _tiles.TryGetValue(vec, out Piece value) ? value : null;
		set => _tiles[vec] = value;
	}

	void Awake() => instance = this;

	void Start() {
		cam = Camera.main;
		Core core = PieceSpawner.instance.CreateCore();
		core.transform.position = Vector3.zero;
		core.Place(Vector2Int.zero);
	}

	void Update() {
		if (tileHeld is null && Time.time - lastTilePlacedTime > tilePlaceDelay) {
			tileHeld = PieceSpawner.instance.GetRandomPiece();
			lastCursorPos = new Vector2Int(int.MinValue, int.MinValue); //Invalidate cursor position
		}

		Vector2Int cursorPos = GetTileFromCursor();
		if (cursorPos != lastCursorPos) {
			isValidPlacement = IsValidPlacement(tileHeld, cursorPos);
		}

		void PlaceOrDiscardTile() {
			lastTilePlacedTime = Time.time;
			lastCursorPos = new Vector2Int(int.MinValue, int.MinValue);
			tileHeld = null;
		}

		if (tileHeld != null) {
			//Handle rotations
			if (Input.GetButtonDown("Rotate Clockwise") || Input.GetAxisRaw("ScrollWheel") < 0) {
				tileHeld.Rotate(clockwise: true);
			} else if (Input.GetButtonDown("Rotate Counterclockwise") || Input.GetAxisRaw("ScrollWheel") > 0) {
				tileHeld.Rotate(clockwise: false);
			}

			//Handle placement and discarding
			if (Input.GetMouseButtonDown(0) && isValidPlacement) {
				tileHeld.Place(cursorPos);
				PlaceOrDiscardTile();
			} else if (Input.GetMouseButtonDown(1)) {
				Destroy(tileHeld.gameObject);
				PlaceOrDiscardTile();
			}
		}
	}

	bool IsValidPlacement(Piece piece, Vector2Int tile) {
		//Always valid if no piece selected
		if (piece is null) {
			return true;
		}

		//Can't be occupied
		if (this[tile] != null) {
			return false;
		}

		//Connectors must match
		bool hasValidConnector = false;
		bool[] invalidConnectors = new bool[4];
		for (int dir = 0; dir < 4; dir++) {
			Piece adjacent = this[tile + Piece.directions[dir]];
			if (adjacent is null) {
				continue;
			} else if (adjacent.hasConnector[(dir + 2) % 4] == piece.hasConnector[dir]) { //Both connectors either are or aren't
				if (piece.hasConnector[dir]) { //Both connectors are
					hasValidConnector = true;
				}
			} else {
				invalidConnectors[dir] = true;
			}
		}

		//Also update visuals
		tileHeld.transform.position = new Vector3(tile.x, 0, tile.y);
		Cursor.instance.UpdateCursor(tile, hasValidConnector, invalidConnectors);

		return hasValidConnector;
	}

	public Vector2Int GetTileFromCursor() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 tileLoc = ray.GetPoint(ray.origin.y / -ray.direction.y);
		return new Vector2Int(Convert.ToInt32(tileLoc.x), Convert.ToInt32(tileLoc.z));
	}
}
