using System;
using UnityEngine;

public class Core : Piece {
	public static Core instance;

	//TODO: More stats (asteroids destroyed, blocks placed, energy produced)
	public float score = 0;
	float _energy;
	public float Energy {
		get => _energy;
		set {
			if (_energy >= 1 && value < 1) {
				IsEnergyLow?.Invoke(true);
			} else if (_energy < 1 && value >= 1) {
				IsEnergyLow?.Invoke(false);
			}

			_energy = value;
		}
	}
	public float energyPerSecond = 1;

	public Action<bool> IsEnergyLow;

	void Awake() => instance = this;

	void Update() => Energy += energyPerSecond * Time.deltaTime;

	protected override void Disable() {
		float finalScore = score;
		//TODO: Initialize game over sequence
	}
}
