using System;
using UnityEngine;

public class Core : Piece {
	public static Core instance;

	UIController ui;

	//TODO: More stats (asteroids destroyed, blocks placed, energy produced)
	float _score;
	public float Score {
		get => _score;
		set {
			ui.Score = value;
			_score = value;
		}
	}
	float _energy;
	public float Energy {
		get => _energy;
		set {
			if (_energy >= 1 && value < 1) {
				IsEnergyLow?.Invoke(true);
			} else if (_energy < 1 && value >= 1) {
				IsEnergyLow?.Invoke(false);
			}

			ui.Energy = value;
			_energy = value;
		}
	}
	public float energyPerSecond = 1;

	public Action<bool> IsEnergyLow;

	void Awake() {
		instance = this;
		ui = FindObjectOfType<UIController>();
	}

	void Update() => Energy += energyPerSecond * Time.deltaTime;

	protected override void Disable() {
		float finalScore = Score;
		//TODO: Initialize game over sequence
	}
}
