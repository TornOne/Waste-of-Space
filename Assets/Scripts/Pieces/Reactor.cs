using UnityEngine;

public class Reactor : Piece {
	public float energyPerSecond = 5;

	void Awake() => enabled = false;

	void Update() => Core.instance.Energy += energyPerSecond * Time.deltaTime;
}
