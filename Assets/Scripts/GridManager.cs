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
	public RectInt bounds = new RectInt(0, 0, 0, 0);

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
		}

		Vector2Int cursorPos = GetTileFromCursor();
		bool isValidPlacement = IsValidPlacement(tileHeld, cursorPos);

		if (tileHeld != null) {
			tileHeld.transform.position = new Vector3(cursorPos.x, 0, cursorPos.y);

			//Handle rotations
			if (Input.GetButtonDown("Rotate Clockwise") || Input.GetAxisRaw("ScrollWheel") < 0) {
				tileHeld.Rotate(clockwise: true);
			} else if (Input.GetButtonDown("Rotate Counterclockwise") || Input.GetAxisRaw("ScrollWheel") > 0) {
				tileHeld.Rotate(clockwise: false);
			}

			//Handle placement and discarding
			if (Input.GetMouseButtonDown(0) && isValidPlacement) {
				tileHeld.Place(cursorPos);
				lastTilePlacedTime = Time.time;
				tileHeld = null;
			} else if (Input.GetMouseButtonDown(1)) {
				Destroy(tileHeld.gameObject);
				lastTilePlacedTime = Time.time;
				tileHeld = null;
			}
		}
	}

	public void UpdateBounds() {
		int minX = 0;
		int maxX = 0;
		int minY = 0;
		int maxY = 0;

		foreach (Vector2Int tileLoc in _tiles.Keys) {
			if (tileLoc.x < minX) {
				minX = tileLoc.x;
			} else if (tileLoc.x > maxX) {
				maxX = tileLoc.x;
			}
			if (tileLoc.y < minY) {
				minY = tileLoc.y;
			} else if (tileLoc.y > maxY) {
				maxY = tileLoc.y;
			}
		}

		bounds = new RectInt(minX, minY, maxX - minX, maxY - minY);
	}

	bool IsValidPlacement(Piece piece, Vector2Int tile) {
		//Always valid if no piece selected
		if (piece is null) {
			Cursor.instance.UpdateCursor(tile, true, new bool[4] { false, false, false, false });
			return true;
		}

		//Can't be occupied
		if (this[tile] != null) {
			Cursor.instance.UpdateCursor(tile, false, new bool[4] { false, false, false, false });
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

		Cursor.instance.UpdateCursor(tile, hasValidConnector, invalidConnectors);

		return hasValidConnector && !Array.Exists(invalidConnectors, x => x);
	}

	public Vector2Int GetTileFromCursor() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 tileLoc = ray.GetPoint(ray.origin.y / -ray.direction.y);
		return new Vector2Int(Convert.ToInt32(tileLoc.x), Convert.ToInt32(tileLoc.z));
	}
}
