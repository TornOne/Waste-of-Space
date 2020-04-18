using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	Camera cam;
	readonly Dictionary<Vector2Int, Piece> tiles = new Dictionary<Vector2Int, Piece>();

	public Piece this[int x, int y] {
		get => this[new Vector2Int(x, y)];
		set => this[new Vector2Int(x, y)] = value;
	}

	public Piece this[Vector2Int vec] {
		get => tiles.TryGetValue(vec, out Piece value) ? value : null;
		set => tiles[vec] = value;
	}

	void Start() {
		cam = Camera.main;
	}

	void Update() {
		Debug.Log(GetTileFromCursor());
	}

	public Vector2Int GetTileFromCursor() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 tileLoc = ray.GetPoint(-ray.origin.z / ray.direction.z);
		Debug.Log(tileLoc);
		return new Vector2Int((int)tileLoc.x, (int)tileLoc.y);
	}
}
