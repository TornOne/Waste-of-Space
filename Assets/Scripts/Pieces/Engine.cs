using UnityEngine;

public class Engine : Piece {
	public float scorePerSecond = 1;

	void Awake() => enabled = false;

	void Update() => Core.instance.score += scorePerSecond * Time.deltaTime;

	//Can't rotate an engine
	public override void Rotate(bool clockwise) { }
}
