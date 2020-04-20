using UnityEngine;

public class Engine : Piece {
	public float energyPerSecond = 1;

	void Awake() => enabled = false;

	void Update() {
		float energyCost = energyPerSecond * Time.deltaTime;
		if (Core.instance.Energy >= energyCost) {
			Core.instance.Energy -= energyCost;
			Core.instance.score += energyCost;
		}
	}

	//Can't rotate an engine
	public override void Rotate(bool clockwise) { }
}
